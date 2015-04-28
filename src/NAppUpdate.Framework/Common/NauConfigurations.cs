using System;
using System.Collections.Generic;
using System.IO;

namespace NAppUpdate.Framework.Common
{
	[Serializable]
	public class NauConfigurations
	{
		public string TempFolder { get; set; }
        public string RelaunchAppArgs { get; set; }

		/// <summary>
		/// Path to the backup folder used by the update process
		/// </summary>
		public string BackupFolder
		{
			set
			{
				if (UpdateManager.Instance.State == UpdateManager.UpdateProcessState.NotChecked
					|| UpdateManager.Instance.State == UpdateManager.UpdateProcessState.Checked)
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
		internal string _backupFolder;

		public string UpdateProcessName { get; set; }

		/// <summary>
		/// The name for the executable file to extract and run cold updates with. Default is foo.exe. You can change
		/// it to whatever you want, but pay attention to names like "updater.exe" and "installer.exe" - they will trigger
		/// an UAC prompt in all cases.
		/// </summary>
		public string UpdateExecutableName { get; set; }

		/// <summary>
		/// A list of files (relative paths only) to be copied along with the NAppUpdate DLL and updater host
		/// when performing cold updates. You need to set this only when you have a custom IUpdateTask that
		/// takes dependency of an external DLL, or require other files side by side with them.
		/// Custom IUpdateTasks taking dependencies of external DLLs which may require cold update, MUST reside
		/// in an external class-library, never in the application EXE, for that reason.
		/// </summary>
		public List<string> DependenciesForColdUpdate { get; set; }
	}
}
