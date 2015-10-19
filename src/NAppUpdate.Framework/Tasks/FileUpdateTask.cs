using System;
using System.IO;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Utils;

namespace NAppUpdate.Framework.Tasks
{
    [Serializable]
    [UpdateTaskAlias("fileUpdate")]
    public class FileUpdateTask : UpdateTaskBase
    {
        [NauField("localPath", "The local path of the file to update", true)]
        public string LocalPath { get; set; }

        [NauField("updateTo",
            "File name on the remote location; same name as local path will be used if left blank"
            , false)]
        public string UpdateTo { get; set; }

        [NauField("sha256-checksum", "SHA-256 checksum to validate the file after download (optional)", false)]
        public string Sha256Checksum { get; set; }

        [NauField("hotswap",
            "Default update action is a cold update; check here if a hot file swap should be attempted"
            , false)]
        public bool CanHotSwap { get; set; }

        private string _destinationFile, _backupFile, _tempFile;

        public override void Prepare(Sources.IUpdateSource source)
        {
            if (string.IsNullOrEmpty(LocalPath))
            {
                UpdateManager.Instance.Logger.Log(Logger.SeverityLevel.Warning, "FileUpdateTask: LocalPath is empty, task is a noop");
                return; // Errorneous case, but there's nothing to prepare to, and by default we prefer a noop over an error
            }

            string fileName;
            if (!string.IsNullOrEmpty(UpdateTo))
                fileName = UpdateTo;
            else
                fileName = LocalPath;

            _tempFile = null;

            string baseUrl = UpdateManager.Instance.BaseUrl;
            string tempFileLocal = Path.Combine(UpdateManager.Instance.Config.TempFolder, Guid.NewGuid().ToString());

            UpdateManager.Instance.Logger.Log("FileUpdateTask: Downloading {0} with BaseUrl of {1} to {2}", fileName, baseUrl, tempFileLocal);

            if (!source.GetData(fileName, baseUrl, OnProgress, ref tempFileLocal))
                throw new UpdateProcessFailedException("FileUpdateTask: Failed to get file from source");

            _tempFile = tempFileLocal;
            if (_tempFile == null)
                throw new UpdateProcessFailedException("FileUpdateTask: Failed to get file from source");

            if (!string.IsNullOrEmpty(Sha256Checksum))
            {
                string checksum = FileChecksum.GetSHA256Checksum(_tempFile);
                if (!checksum.Equals(Sha256Checksum))
                    throw new UpdateProcessFailedException(string.Format("FileUpdateTask: Checksums do not match; expected {0} but got {1}", Sha256Checksum, checksum));
            }

            _destinationFile = Path.Combine(Path.GetDirectoryName(UpdateManager.Instance.ApplicationPath), LocalPath);
            UpdateManager.Instance.Logger.Log("FileUpdateTask: Prepared successfully; destination file: {0}", _destinationFile);
        }

        public override TaskExecutionStatus Execute(bool coldRun)
        {
            if (string.IsNullOrEmpty(LocalPath))
            {
                UpdateManager.Instance.Logger.Log(Logger.SeverityLevel.Warning, "FileUpdateTask: LocalPath is empty, task is a noop");
                return TaskExecutionStatus.Successful; // Errorneous case, but there's nothing to prepare to, and by default we prefer a noop over an error
            }

            var dirName = Path.GetDirectoryName(_destinationFile);
            if (!Directory.Exists(dirName))
                Utils.FileSystem.CreateDirectoryStructure(dirName, false);

            // Create a backup copy if target exists
            if (_backupFile == null && File.Exists(_destinationFile))
            {
                if (!Directory.Exists(Path.GetDirectoryName(Path.Combine(UpdateManager.Instance.Config.BackupFolder, LocalPath))))
                    Utils.FileSystem.CreateDirectoryStructure(
                        Path.GetDirectoryName(Path.Combine(UpdateManager.Instance.Config.BackupFolder, LocalPath)), false);
                _backupFile = Path.Combine(UpdateManager.Instance.Config.BackupFolder, LocalPath);
                File.Copy(_destinationFile, _backupFile, true);
            }

            // Only allow execution if the apply attribute was set to hot-swap, or if this is a cold run
            if (CanHotSwap || coldRun)
            {
                if (File.Exists(_destinationFile))
                {
                    //if (FileSystem.IsExeRunning(_destinationFile))
                    //{
                    //    UpdateManager.Instance.Logger.Log(Logger.SeverityLevel.Warning, "Process {0} is still running", _destinationFile);
                    //    Thread.Sleep(1000); // TODO: retry a few times and throw after a while
                    //}

                    if (!PermissionsCheck.HaveWritePermissionsForFileOrFolder(_destinationFile))
                    {
                        if (coldRun)
                        {
                            UpdateManager.Instance.Logger.Log(Logger.SeverityLevel.Warning, "Don't have permissions to touch {0}", _destinationFile);
                            File.Delete(_destinationFile); // get the original exception from the system
                        }
                        CanHotSwap = false;
                    }
                }

                try
                {
                    if (File.Exists(_destinationFile))
                        File.Delete(_destinationFile);
                    File.Move(_tempFile, _destinationFile);
                    _tempFile = null;
                }
                catch (Exception ex)
                {
                    if (coldRun)
                    {
                        ExecutionStatus = TaskExecutionStatus.Failed;
                        throw new UpdateProcessFailedException("Could not replace the file", ex);
                    }

                    // Failed hot swap file tasks should now downgrade to cold tasks automatically
                    CanHotSwap = false;
                }
            }

            if (coldRun || CanHotSwap)
                // If we got thus far, we have completed execution
                return TaskExecutionStatus.Successful;

            // Otherwise, figure out what restart method to use
            if (File.Exists(_destinationFile) && !Utils.PermissionsCheck.HaveWritePermissionsForFileOrFolder(_destinationFile))
            {
                return TaskExecutionStatus.RequiresPrivilegedAppRestart;
            }
            return TaskExecutionStatus.RequiresAppRestart;
        }

        public override bool Rollback()
        {
            if (string.IsNullOrEmpty(_destinationFile))
                return true;

            // Copy the backup copy back to its original position
            if (File.Exists(_destinationFile))
                File.Delete(_destinationFile);
            File.Copy(_backupFile, _destinationFile, true);

            return true;
        }
    }
}
