using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System;
using System.Threading;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.FeedReaders;
using NAppUpdate.Framework.Sources;
using NAppUpdate.Framework.Tasks;
using NAppUpdate.Framework.Utils;

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
			IsWorking = false;
			State = UpdateProcessState.NotChecked;
			UpdatesToApply = new List<IUpdateTask>();
			ApplicationPath = Process.GetCurrentProcess().MainModule.FileName;
			UpdateFeedReader = new NauXmlFeedReader();
			Logger = new Logger();
			Config = new NauConfigurations
						{
							TempFolder = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()),
							UpdateProcessName = "NAppUpdateProcess",
							UpdateExecutableName = "foo.exe", // Naming it updater.exe seem to trigger the UAC, and we don't want that
						};

			// Need to do this manually here because the BackupFolder property is protected using the static instance, which we are
			// in the middle of creating
			string backupPath = Path.Combine(Path.GetDirectoryName(ApplicationPath) ?? string.Empty, "Backup" + DateTime.Now.Ticks);
			backupPath = backupPath.TrimEnd(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
			Config._backupFolder = Path.IsPathRooted(backupPath) ? backupPath : Path.Combine(Config.TempFolder, backupPath);
		}

		static UpdateManager() { }

		/// <summary>
		/// The singleton update manager instance to used by consumer applications
		/// </summary>
		public static UpdateManager Instance
		{
			get { return instance; }
		}
		private static readonly UpdateManager instance = new UpdateManager();
		// ReSharper disable NotAccessedField.Local
		private static Mutex _shutdownMutex;
		// ReSharper restore NotAccessedField.Local

		#endregion

		/// <summary>
		/// State of the update process
		/// </summary>
		[Serializable]
		public enum UpdateProcessState
		{
			NotChecked,
			Checked,
			Prepared,
			AfterRestart,
			AppliedSuccessfully,
			RollbackRequired,
		}

		internal readonly string ApplicationPath;

		public NauConfigurations Config { get; set; }

		internal string BaseUrl { get; set; }
		internal IList<IUpdateTask> UpdatesToApply { get; private set; }
		public int UpdatesAvailable { get { return UpdatesToApply == null ? 0 : UpdatesToApply.Count; } }
		public UpdateProcessState State { get; private set; }

		public IUpdateSource UpdateSource { get; set; }
		public IUpdateFeedReader UpdateFeedReader { get; set; }

		public Logger Logger { get; private set; }

		public IEnumerable<IUpdateTask> Tasks { get { return UpdatesToApply; } }

		internal volatile bool ShouldStop;

		public bool IsWorking { get { return _isWorking; } private set { _isWorking = value; } }
		private volatile bool _isWorking;

		#region Progress reporting

		public event ReportProgressDelegate ReportProgress;
		private void TaskProgressCallback(UpdateProgressInfo currentStatus, IUpdateTask task)
		{
			if (ReportProgress == null) return;

			currentStatus.TaskDescription = task.Description;
			currentStatus.TaskId = UpdatesToApply.IndexOf(task) + 1;

			//This was an assumed int, which meant we never reached 100% with an odd number of tasks
			float taskPerc = 100F / UpdatesToApply.Count;
			currentStatus.Percentage = (int)Math.Round((currentStatus.Percentage * taskPerc / 100) + (currentStatus.TaskId - 1) * taskPerc);

			ReportProgress(currentStatus);
		}

		#endregion

		#region Step 1 - Check for updates

		/// <summary>
		/// Check for updates synchronously
		/// </summary>
		public void CheckForUpdates()
		{
			if (IsWorking)
			{
				throw new InvalidOperationException("Another update process is already in progress");
			}
			else if (UpdateFeedReader == null)
			{
				throw new InvalidOperationException("UpdateFeedReader must be set before calling CheckForUpdates()");
			}
			else if (UpdateSource == null)
			{
				throw new InvalidOperationException("UpdateSource must be set before calling CheckForUpdates()");
			}

			using (WorkScope.New(isWorking => IsWorking = isWorking))
			{
				if (State != UpdateProcessState.NotChecked)
				{
					throw new InvalidOperationException("Already checked for updates; to reset the current state call CleanUp()");
				}

				lock (UpdatesToApply)
				{
					UpdatesToApply.Clear();
                    float num = 0f;
                    var tasks = UpdateFeedReader.Read(UpdateSource.GetUpdatesFeed());
                    int count = tasks.Count;
                    foreach (var t in tasks)
					{
						if (ShouldStop)
							throw new UserAbortException();
                        this.ReportProgress(new UpdateProgressInfo     {
                         Message = "Checking",
                             Percentage = (int)((num += 1f) / (float)count * 100f),
                             TaskDescription = t.Description,
                             TaskId = (int)num,
                             StillWorking = true
                         });
                        if (t.UpdateConditions == null || t.UpdateConditions.IsMet(t)) // Only execute if all conditions are met
							UpdatesToApply.Add(t);
					}
				}

				State = UpdateProcessState.Checked;
			}
		}

		/// <summary>
		/// Check for updates asynchronously
		/// </summary>
		/// <param name="callback">Callback function to call when done; can be null</param>
		/// <param name="state">Allows the caller to preserve state; can be null</param>
		public IAsyncResult BeginCheckForUpdates(AsyncCallback callback, Object state)
		{
			// Create IAsyncResult object identifying the 
			// asynchronous operation
			var ar = new UpdateProcessAsyncResult(callback, state);

			// Use a thread pool thread to perform the operation
			ThreadPool.QueueUserWorkItem(o =>
			{
				try
				{
					// Perform the operation; if sucessful set the result
					CheckForUpdates();
					ar.SetAsCompleted(null, false);
				}
				catch (Exception e)
				{
					// If operation fails, set the exception
					ar.SetAsCompleted(e, false);
				}
			}, ar);

			return ar;  // Return the IAsyncResult to the caller
		}

		/// <summary>
		/// Block until previously-called CheckForUpdates complete
		/// </summary>
		/// <param name="asyncResult"></param>
		public void EndCheckForUpdates(IAsyncResult asyncResult)
		{
			// Wait for operation to complete, then return or throw exception
			var ar = (UpdateProcessAsyncResult)asyncResult;
			ar.EndInvoke();
		}

		#endregion

		#region Step 2 - Prepare to execute update tasks

		/// <summary>
		/// Prepare updates synchronously
		/// </summary>
		public void PrepareUpdates()
		{
			if (IsWorking)
				throw new InvalidOperationException("Another update process is already in progress");

			using (WorkScope.New(isWorking => IsWorking = isWorking))
			{
				lock (UpdatesToApply)
				{
					if (State != UpdateProcessState.Checked)
						throw new InvalidOperationException("Invalid state when calling PrepareUpdates(): " + State);

					if (UpdatesToApply.Count == 0)
						throw new InvalidOperationException("No updates to prepare");

					if (!Directory.Exists(Config.TempFolder))
					{
						Logger.Log("Creating Temp directory {0}", Config.TempFolder);
						Directory.CreateDirectory(Config.TempFolder);
					}
					else
					{
						Logger.Log("Using existing Temp directory {0}", Config.TempFolder);
					}

					foreach (var task in UpdatesToApply)
					{
						if (ShouldStop)
							throw new UserAbortException();

						var t = task;
						task.ProgressDelegate += status => TaskProgressCallback(status, t);
                        this.TaskProgressCallback(new UpdateProgressInfo{
                                              Message = "Preparing"}, t);
                        try
						{
							task.Prepare(UpdateSource);
						}
						catch (Exception ex)
						{
							task.ExecutionStatus = TaskExecutionStatus.FailedToPrepare;
							Logger.Log(ex);
							throw new UpdateProcessFailedException("Failed to prepare task: " + task.Description, ex);
						}

						task.ExecutionStatus = TaskExecutionStatus.Prepared;
					}

					State = UpdateProcessState.Prepared;
				}
			}
		}

		/// <summary>
		/// Prepare updates asynchronously
		/// </summary>
		/// <param name="callback">Callback function to call when done; can be null</param>
		/// <param name="state">Allows the caller to preserve state; can be null</param>
		public IAsyncResult BeginPrepareUpdates(AsyncCallback callback, Object state)
		{
			// Create IAsyncResult object identifying the 
			// asynchronous operation
			var ar = new UpdateProcessAsyncResult(callback, state);

			// Use a thread pool thread to perform the operation
			ThreadPool.QueueUserWorkItem(o =>
			{
				try
				{
					// Perform the operation; if sucessful set the result
					PrepareUpdates();
					ar.SetAsCompleted(null, false);
				}
				catch (Exception e)
				{
					// If operation fails, set the exception
					ar.SetAsCompleted(e, false);
				}
			}, ar);

			return ar;  // Return the IAsyncResult to the caller
		}

		/// <summary>
		/// Block until previously-called PrepareUpdates complete
		/// </summary>
		/// <param name="asyncResult"></param>
		public void EndPrepareUpdates(IAsyncResult asyncResult)
		{
			// Wait for operation to complete, then return or throw exception
			var ar = (UpdateProcessAsyncResult)asyncResult;
			ar.EndInvoke();
		}

		#endregion

		#region Step 3 - Apply updates

		/// <summary>
		/// Starts the updater executable and sends update data to it, and relaunch the caller application as soon as its done
		/// </summary>
		/// <returns>True if successful (unless a restart was required</returns>
		public void ApplyUpdates()
		{
			ApplyUpdates(true);
		}

		/// <summary>
		/// Starts the updater executable and sends update data to it
		/// </summary>
		/// <param name="relaunchApplication">true if relaunching the caller application is required; false otherwise</param>
		/// <returns>True if successful (unless a restart was required</returns>
		public void ApplyUpdates(bool relaunchApplication)
		{
			ApplyUpdates(relaunchApplication, false, false);
		}

		/// <summary>
		/// Starts the updater executable and sends update data to it
		/// </summary>
		/// <param name="relaunchApplication">true if relaunching the caller application is required; false otherwise</param>
		/// <param name="updaterDoLogging">true if the updater writes to a log file; false otherwise</param>
		/// <param name="updaterShowConsole">true if the updater shows the console window; false otherwise</param>
		/// <returns>True if successful (unless a restart was required</returns>
		public void ApplyUpdates(bool relaunchApplication, bool updaterDoLogging, bool updaterShowConsole)
		{
			if (IsWorking)
				throw new InvalidOperationException("Another update process is already in progress");

			lock (UpdatesToApply)
			{
				using (WorkScope.New(isWorking => IsWorking = isWorking))
				{
					bool revertToDefaultBackupPath = true;

					// Set current directory the the application directory
					// this prevents the updater from writing to e.g. c:\windows\system32
					// if the process is started by autorun on windows logon.
					// ReSharper disable AssignNullToNotNullAttribute
					Environment.CurrentDirectory = Path.GetDirectoryName(ApplicationPath);
					// ReSharper restore AssignNullToNotNullAttribute

					// Make sure the current backup folder is accessible for writing from this process
					string backupParentPath = Path.GetDirectoryName(Config.BackupFolder) ?? string.Empty;
					if (Directory.Exists(backupParentPath) && PermissionsCheck.HaveWritePermissionsForFolder(backupParentPath))
					{
						// Remove old backup folder, in case this same folder was used previously,
						// and it wasn't removed for some reason
						try
						{
							if (Directory.Exists(Config.BackupFolder))
								FileSystem.DeleteDirectory(Config.BackupFolder);
							revertToDefaultBackupPath = false;
						}
						catch (UnauthorizedAccessException)
						{
						}

						// Attempt to (re-)create the backup folder
						try
						{
							Directory.CreateDirectory(Config.BackupFolder);

							if (!PermissionsCheck.HaveWritePermissionsForFolder(Config.BackupFolder))
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
						Config._backupFolder = Path.Combine(
							Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
							Config.UpdateProcessName + "UpdateBackups" + DateTime.UtcNow.Ticks);

						try
						{
							Directory.CreateDirectory(Config.BackupFolder);
						}
						catch (UnauthorizedAccessException ex)
						{
							// We can't backup, so we abort
							throw new UpdateProcessFailedException("Could not create backup folder " + Config.BackupFolder, ex);
						}
					}

					bool runPrivileged = false, hasColdUpdates = false;
					State = UpdateProcessState.RollbackRequired;
					foreach (var task in UpdatesToApply)
					{
						IUpdateTask t = task;
						task.ProgressDelegate += status => TaskProgressCallback(status, t);
                        this.TaskProgressCallback(new UpdateProgressInfo{
                           Message = "Applying"}, t);
                        try
						{
							// Execute the task
							task.ExecutionStatus = task.Execute(false);
						}
						catch (Exception ex)
						{
							task.ExecutionStatus = TaskExecutionStatus.Failed; // mark the failing task before rethrowing
							throw new UpdateProcessFailedException("Update task execution failed: " + task.Description, ex);
						}

						if (task.ExecutionStatus == TaskExecutionStatus.RequiresAppRestart
							|| task.ExecutionStatus == TaskExecutionStatus.RequiresPrivilegedAppRestart)
						{
							// Record that we have cold updates to run, and if required to run any of them privileged
							runPrivileged = runPrivileged || task.ExecutionStatus == TaskExecutionStatus.RequiresPrivilegedAppRestart;
							hasColdUpdates = true;
							continue;
						}

						// We are being quite explicit here - only Successful return values are considered
						// to be Ok (cold updates are already handled above)
						if (task.ExecutionStatus != TaskExecutionStatus.Successful)
							throw new UpdateProcessFailedException("Update task execution failed: " + task.Description);
					}

					// If an application restart is required
					if (hasColdUpdates)
					{
						var dto = new NauIpc.NauDto
									{
										Configs = Instance.Config,
										Tasks = Instance.UpdatesToApply,
										AppPath = ApplicationPath,
										WorkingDirectory = Environment.CurrentDirectory,
										RelaunchApplication = relaunchApplication,
										LogItems = Logger.LogItems,
									};

						NauIpc.ExtractUpdaterFromResource(Config.TempFolder, Instance.Config.UpdateExecutableName);

						var info = new ProcessStartInfo
									{
										UseShellExecute = true,
										WorkingDirectory = Environment.CurrentDirectory,
										FileName = Path.Combine(Config.TempFolder, Instance.Config.UpdateExecutableName),
										Arguments =
											string.Format(@"""{0}"" {1} {2}", Config.UpdateProcessName,
														  updaterShowConsole ? "-showConsole" : string.Empty,
														  updaterDoLogging ? "-log" : string.Empty),
									};

						if (!updaterShowConsole)
						{
							info.WindowStyle = ProcessWindowStyle.Hidden;
							info.CreateNoWindow = true;
						}

						// If we can't write to the destination folder, then lets try elevating priviledges.
						if (runPrivileged || !PermissionsCheck.HaveWritePermissionsForFolder(Environment.CurrentDirectory))
						{
							info.Verb = "runas";
						}

						bool createdNew;
						_shutdownMutex = new Mutex(true, Config.UpdateProcessName + "Mutex", out createdNew);

						try
						{
							NauIpc.LaunchProcessAndSendDto(dto, info, Config.UpdateProcessName);
						}
						catch (Exception ex)
						{
							throw new UpdateProcessFailedException("Could not launch cold update process", ex);
						}

						Environment.Exit(0);
					}

					State = UpdateProcessState.AppliedSuccessfully;
					UpdatesToApply.Clear();
				}
			}
		}

		#endregion

		public void ReinstateIfRestarted()
		{
			if (!IsAfterRestart())
			{
				return;
			}

			lock (UpdatesToApply)
			{
				NauIpc.NauDto dto = NauIpc.ReadDto(Config.UpdateProcessName);

				if (dto == null)
				{
					return;
				}

				Config = dto.Configs;
				UpdatesToApply = dto.Tasks;
				Logger = new Logger(dto.LogItems);
				State = UpdateProcessState.AfterRestart;
			}
		}

		/// <summary>
		/// Rollback executed updates in case of an update failure
		/// </summary>
		public void RollbackUpdates()
		{
			if (IsWorking) return;

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
					if (Directory.Exists(Config.TempFolder))
						FileSystem.DeleteDirectory(Config.TempFolder);
				}
				catch { }

				try
				{
					if (Directory.Exists(Config.BackupFolder))
						FileSystem.DeleteDirectory(Config.BackupFolder);
				}
				catch { }

				ShouldStop = false;
			}
		}

		private bool IsAfterRestart()
		{
			foreach (string arg in Environment.GetCommandLineArgs())
			{
				if (arg == "-nappupdate-afterrestart")
				{
					return true;
				}
			}

			return false;
		}
	}
}