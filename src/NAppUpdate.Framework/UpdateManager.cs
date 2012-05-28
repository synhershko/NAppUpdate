using System.Collections.Generic;
using System.IO;
using System;
using System.Threading;
using NAppUpdate.Framework.FeedReaders;
using NAppUpdate.Framework.Sources;
using NAppUpdate.Framework.Tasks;

namespace NAppUpdate.Framework
{
	/// <summary>
	/// An UpdateManager class is a singleton class handling the update process from start to end for a consumer application
	/// </summary>
    public sealed class UpdateManager
    {
        #region Singleton Stuff

		/// <summary>
		/// Defaut ctor
		/// </summary>
        private UpdateManager()
        {
            State = UpdateProcessState.NotChecked;
            UpdatesToApply = new List<IUpdateTask>();
            TempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            UpdateProcessName = "NAppUpdateProcess";
			UpdateExecutableName = "foo.exe"; // Naming it updater.exe seem to trigger the UAC, and we don't want that
            ApplicationPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            BackupFolder = Path.Combine(Path.GetDirectoryName(ApplicationPath) ?? string.Empty, "Backup");
        }

		/// <summary>
		/// The singleton update manager instance to used by consumer applications
		/// </summary>
        public static UpdateManager Instance
        {
            get { return instance; }
        }
		private static readonly UpdateManager instance = new UpdateManager();

        #endregion

		/// <summary>
		/// State of the update process
		/// </summary>
        public enum UpdateProcessState
        {
            NotChecked,
            Checked,
            Prepared,
            AppliedSuccessfully,
            RollbackRequired,
        }

        public string TempFolder { get; set; }
        public string UpdateProcessName { get; set; }
        public System.IO.Stream IconStream
        {
            get { 
                return System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("NAppUpdate.Framework.updateicon.ico"); 
            }
        }
		
		/// <summary>
		/// The name for the executable file to extract and run cold updates with. Default is foo.exe. You can change
		/// it to whatever you want, but pay attention to names like "updater.exe" and "installer.exe" - they will trigger
		/// an UAC prompt in all cases.
		/// </summary>
		public string UpdateExecutableName { get; set; }
        
        internal readonly string ApplicationPath;

		/// <summary>
		/// Path to the backup folder used by this update process
		/// </summary>
        public string BackupFolder
        {
            set
            {
                if (State == UpdateProcessState.NotChecked || State == UpdateProcessState.Checked)
                {
                    string path = value.TrimEnd(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                    _backupFolder = Path.IsPathRooted(path) ? path : Path.Combine(TempFolder, path);
                }
                else
                    throw new ArgumentException("BackupFolder can only be specified before update has started");
            }
            get
            {
                return _backupFolder;
            }
        }
		private string _backupFolder;

        internal string BaseUrl { get; set; }
        public IList<IUpdateTask> UpdatesToApply { get; private set; }
		public int UpdatesAvailable { get { return UpdatesToApply == null ? 0 : UpdatesToApply.Count; } }
        public UpdateProcessState State { get; private set; }
		public string LatestError { get; set; }

        public IUpdateSource UpdateSource { get; set; }
        public IUpdateFeedReader UpdateFeedReader { get; set; }

        private Thread _updatesThread;
        internal volatile bool ShouldStop;
        public bool IsWorking { get { return _updatesThread != null && _updatesThread.IsAlive; } }

        #region Step 1 - Check for updates

		/// <summary>
		/// Check for update synchronously, using the default update source
		/// </summary>
		/// <returns>true if successful and updates exist</returns>
        public bool CheckForUpdates()
        {
            return CheckForUpdates_Internal(UpdateSource, null, null);
        }

		/// <summary>
		/// Check for updates synchronously
		/// </summary>
		/// <param name="source">An update source to use</param>
		/// <returns>true if successful and updates exist</returns>
        public bool CheckForUpdates(IUpdateSource source)
        {
            return CheckForUpdates_Internal(source ?? UpdateSource, null, null);
        }

		/// <summary>
		/// Check for updates synchronouly, for a callback that simply checks the 
        /// number of updates available.
		/// </summary>
		/// <param name="source">Updates source to use</param>
		/// <param name="callback">Callback function to call when done</param>
		/// <returns>true if successful and updates exist</returns>
        public bool CheckForUpdates(IUpdateSource source, Action<int> callback)
        {
            return CheckForUpdates_Internal(source, callback, null);
        }

        /// <summary>
		/// Check for updates synchronouly, for a callback that processes the list
        /// update tasks.
		/// </summary>
		/// <param name="source">Updates source to use</param>
		/// <param name="callback">Callback function to call when done</param>
		/// <returns>true if successful and updates exist</returns>
        public bool CheckForUpdates(IUpdateSource source, Action<IList<IUpdateTask>> callback)
        {
            return CheckForUpdates_Internal(source, null, callback);
        }

        /// <summary>
		/// Check for updates synchronouly
		/// </summary>
		/// <param name="source">Updates source to use</param>
		/// <param name="callback">Callback function to call when done</param>
		/// <returns>true if successful and updates exist</returns>
        private bool CheckForUpdates_Internal(IUpdateSource source, Action<int> countCallback, Action<IList<IUpdateTask>> listCallback)
        {
        	LatestError = null;

            if (UpdateFeedReader == null)
                throw new ArgumentException("An update feed reader is required; please set one before checking for updates");

            if (source == null)
                throw new ArgumentException("An update source was not specified");

            lock (UpdatesToApply)
            {
                UpdatesToApply.Clear();
                var tasks = UpdateFeedReader.Read(source.GetUpdatesFeed());
                foreach (var t in tasks)
                {
                    if (ShouldStop) return false;
                    if (t.UpdateConditions.IsMet(t)) // Only execute if all conditions are met
                        UpdatesToApply.Add(t);
                }
            }

            if (ShouldStop) return false;

            State = UpdateProcessState.Checked;
            if (countCallback != null) countCallback.BeginInvoke(UpdatesToApply.Count, null, null);
            if (listCallback != null) listCallback.BeginInvoke(UpdatesToApply, null, null);

            if (UpdatesToApply.Count > 0)
                return true;

            return false;
        }

		/// <summary>
		/// Check for updates asynchronously
		/// </summary>
		/// <param name="callback">Callback function to call when done</param>
        public void CheckForUpdateAsync(Action<int> callback)
        {
            CheckForUpdateAsync_Internal(UpdateSource, callback, null);
        }

		/// <summary>
		/// Check for updates asynchronously
		/// </summary>
		/// <param name="callback">Callback function to call when done</param>
        public void CheckForUpdateAsync(IUpdateSource source, Action<int> callback)
        {
            CheckForUpdateAsync_Internal(source, callback, null);
        }

		/// <summary>
		/// Check for updates asynchronously
		/// </summary>
		/// <param name="callback">Callback function to call when done</param>
        public void CheckForUpdateAsync(Action<IList<IUpdateTask>> callback)
        {
            CheckForUpdateAsync_Internal(UpdateSource, null, callback);
        }

		/// <summary>
		/// Check for updates asynchronously
		/// </summary>
		/// <param name="source">Update source to use</param>
		/// <param name="callback">Callback function to call when done</param>
        public void CheckForUpdateAsync(IUpdateSource source, Action<IList<IUpdateTask>> callback)
        {
            CheckForUpdateAsync_Internal(source, null, callback);
        }

		/// <summary>
		/// Check for updates asynchronously
		/// </summary>
		/// <param name="source">Update source to use</param>
		/// <param name="callback">Callback function to call when done</param>
        private void CheckForUpdateAsync_Internal(IUpdateSource source, Action<int> countCallback, Action<IList<IUpdateTask>> listCallback)
        {
        	if (IsWorking) return;

        	_updatesThread = new Thread(delegate()
        	                            	{
												try
												{
                                                    CheckForUpdates_Internal(source, countCallback, listCallback);
												}
												catch (Exception ex)
												{
													// TODO: Better error handling
													LatestError = ex.ToString();
                                                    if (countCallback != null) 
                                                        countCallback.BeginInvoke(-1, null, null);
                                                    if (listCallback != null)
                                                        listCallback.BeginInvoke(null, null, null);
												}
        	                            	}) {IsBackground = true};
        	_updatesThread.Start();
        }

        #endregion

        #region Step 2 - Prepare to execute update tasks

		/// <summary>
		/// Prepare updates synchronously
		/// </summary>
		/// <returns>true if successful</returns>
        public bool PrepareUpdates()
        {
            return PrepareUpdates(null);
        }

		/// <summary>
		/// Prepare updates synchronously, calling the provided callback when done
		/// </summary>
		/// <param name="callback">A callback function to execute when done</param>
		/// <returns>true if successful</returns>
        private bool PrepareUpdates(Action<bool> callback)
        {
            // TODO: Support progress updates

			LatestError = null;

            lock (UpdatesToApply)
            {
                if (UpdatesToApply.Count == 0)
                {
                    if (callback != null) callback.BeginInvoke(false, null, null);
                    return false;
                }

                if (!Directory.Exists(TempFolder))
                    Directory.CreateDirectory(TempFolder);

				foreach (var t in UpdatesToApply)
				{
					if (ShouldStop || !t.Prepare(UpdateSource))
						return false;
				}

            	State = UpdateProcessState.Prepared;
            }

            if (ShouldStop) return false;

            if (callback != null) callback.BeginInvoke(true, null, null);
            return true;
        }

		/// <summary>
		/// Prepare updates asynchronously
		/// </summary>
		/// <param name="callback">callback function to call when done</param>
        public void PrepareUpdatesAsync(Action<bool> callback)
        {
        	if (IsWorking) return;

        	_updatesThread = new Thread(delegate()
        	                            	{
        	                            		try
        	                            		{
        	                            			PrepareUpdates(callback);
        	                            		}
        	                            		catch (Exception ex)
        	                            		{
													// TODO: Better error handling
        	                            			LatestError = ex.ToString();
        	                            			callback.BeginInvoke(false, null, null);
        	                            		}
        	                            	}) {IsBackground = true};

        	_updatesThread.Start();
        }

        #endregion

        #region Step 3 - Apply updates

        /// <summary>
        /// Starts the updater executable and sends update data to it, and relaunch the caller application as soon as its done
        /// </summary>
        /// <returns>True if successful (unless a restart was required</returns>
        public bool ApplyUpdates()
        {
            return ApplyUpdates(true);
        }

    	/// <summary>
    	/// Starts the updater executable and sends update data to it
    	/// </summary>
		/// <param name="relaunchApplication">true if relaunching the caller application is required; false otherwise</param>
    	/// <returns>True if successful (unless a restart was required</returns>
    	public bool ApplyUpdates(bool relaunchApplication)
        {
            return ApplyUpdates(relaunchApplication, false, false);
        }

       	/// <summary>
    	/// Starts the updater executable and sends update data to it
    	/// </summary>
		/// <param name="relaunchApplication">true if relaunching the caller application is required; false otherwise</param>
        /// <param name="updaterDoLogging">true if the updater writes to a log file; false otherwise</param>
        /// <param name="updaterShowConsole">true if the updater shows the console window; false otherwise</param>
    	/// <returns>True if successful (unless a restart was required</returns>
    	public bool ApplyUpdates(bool relaunchApplication, bool updaterDoLogging, bool updaterShowConsole)
        {
            lock (UpdatesToApply)
            {
				LatestError = null;
            	bool revertToDefaultBackupPath = true;

                // Set current directory the the application directory
                // this prevents the updater from writing to e.g. c:\windows\system32
                // if the process is started by autorun on windows logon.
                Environment.CurrentDirectory = Path.GetDirectoryName(ApplicationPath);

                // Make sure the current backup folder is accessible for writing from this process
                string backupParentPath = Path.GetDirectoryName(BackupFolder) ?? string.Empty;
                if (Directory.Exists(backupParentPath) && Utils.PermissionsCheck.HaveWritePermissionsForFolder(backupParentPath))
				{
					// Remove old backup folder, in case this same folder was used previously,
					// and it wasn't removed for some reason
					try
					{
						if (Directory.Exists(BackupFolder))
							Directory.Delete(BackupFolder, true);
						revertToDefaultBackupPath = false;
					}
					catch (UnauthorizedAccessException)
					{
					}

					// Attempt to (re-)create the backup folder
					try
					{
						Directory.CreateDirectory(BackupFolder);

                        if (!Utils.PermissionsCheck.HaveWritePermissionsForFolder(BackupFolder))
                            revertToDefaultBackupPath = true;
					}
					catch (UnauthorizedAccessException)
					{
						// We're having permissions issues with this folder, so we'll attempt
						// using a backup in a default location
						revertToDefaultBackupPath = true;
					}
				}
				
				if (revertToDefaultBackupPath)
				{
					_backupFolder = Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
						UpdateProcessName + "UpdateBackups");

					try
					{
						Directory.CreateDirectory(BackupFolder);
					}
					catch (UnauthorizedAccessException ex)
					{
						// We can't backup, so we abort
						LatestError = ex.ToString();
						return false;
					}
				}

                bool runPrivileged = false;
            	var executeOnAppRestart = new Dictionary<string, object>();
                State = UpdateProcessState.RollbackRequired;
                foreach (var task in UpdatesToApply)
                {
					// First, execute the task
                    if (!task.Execute())
                    {
                        // TODO: notify about task execution failure using exceptions
                    	continue;
                    }

                    // run updater privileged if required
                    runPrivileged = runPrivileged || task.MustRunPrivileged();

					// Add any pending cold updates to the list
                	var en = task.GetColdUpdates();
					while (en.MoveNext())
					{
						// Last write wins
						executeOnAppRestart[en.Current.Key] = en.Current.Value;
					}
                }

                // If an application restart is required
                if (executeOnAppRestart.Count > 0)
                {
                    // Add some environment variables to the dictionary object which will be passed to the updater
                    executeOnAppRestart["ENV:AppPath"] = ApplicationPath;
					executeOnAppRestart["ENV:WorkingDirectory"] = Environment.CurrentDirectory;
                    executeOnAppRestart["ENV:TempFolder"] = TempFolder;
                    executeOnAppRestart["ENV:BackupFolder"] = BackupFolder;
                    executeOnAppRestart["ENV:RelaunchApplication"] = relaunchApplication;

					if (!Directory.Exists(TempFolder))
						Directory.CreateDirectory(TempFolder);

					var updStarter = new UpdateStarter(Path.Combine(TempFolder, UpdateExecutableName),
                                                executeOnAppRestart, UpdateProcessName, runPrivileged);
                    updStarter.SetOptions(updaterDoLogging, updaterShowConsole);
                    bool createdNew;
                    using (var _ = new Mutex(true, UpdateProcessName, out createdNew))
                    {
                        if (!updStarter.Start())
                            return false;

                        Environment.Exit(0);
                    }
                }

                State = UpdateProcessState.AppliedSuccessfully;
                UpdatesToApply.Clear();
            }

            return true;
        }

        #endregion

		/// <summary>
		/// Rollback executed updates in case of an update failure
		/// </summary>
        public void RollbackUpdates()
        {
            lock (UpdatesToApply)
            {
                foreach (var task in UpdatesToApply)
                {
                    task.Rollback();
                }

                State = UpdateProcessState.NotChecked;
            }
        }

		/// <summary>
		/// Abort update process, cancelling whatever background process currently taking place without waiting for it to complete
		/// </summary>
        public void Abort()
        {
            Abort(false);
        }

		/// <summary>
		/// Abort update process, cancelling whatever background process currently taking place
		/// </summary>
		/// <param name="waitForTermination">If true, blocks the calling thread until the current process terminates</param>
		public void Abort(bool waitForTermination)
		{
			ShouldStop = true;
			if (waitForTermination && _updatesThread != null && _updatesThread.IsAlive)
			{
				_updatesThread.Join(); // TODO perhaps we should support timeout here instead of per process
				_updatesThread = null;
			}
		}

        /// <summary>
        /// Delete the temp folder as a whole and fail silently
        /// </summary>
        public void CleanUp()
        {
            Abort(true);

            lock (UpdatesToApply)
            {
                UpdatesToApply.Clear();
                State = UpdateProcessState.NotChecked;

                try
                {
                    Directory.Delete(TempFolder, true);
                }
                catch { }

                try
                {
                    Directory.Delete(BackupFolder, true);
                }
                catch { }
            }
        }

        /*
        public void DownloadUpdateAsync(Action<bool> finishedCallback)
        {
            FileDownloader fileDownloader = GetFileDownloader();

            fileDownloader.DownloadAsync(downloadedData =>
                                             {
                                                 //validate that the downloaded data is actually valid and not erroneous
                                                 this.UpdateData = downloadedData;
                                                 finishedCallback(true);
                                             });
        }

        public void DownloadUpdateAsync(Action<bool> finishedCallback, Action<int> progressPercentageCallback)
        {
            FileDownloader fileDownloader = GetFileDownloader();

            fileDownloader.DownloadAsync(downloadedData =>
            {
                //TODO: validate that the downloaded data is actually valid and not erroneous
                this.UpdateData = downloadedData;
                finishedCallback(true);
            },
            (arg1, arg2) => progressPercentageCallback((int)(100 * (arg1) / arg2)));
        }
         */
    }
}