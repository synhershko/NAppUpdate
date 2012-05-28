using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NAppUpdate.Framework.Common;

namespace NAppUpdate.Framework.Tasks
{
    [UpdateTaskAlias("fileUpdate")]
    public class FileUpdateTask : IUpdateTask
    {
        public FileUpdateTask()
        {
            UpdateConditions = new Conditions.BooleanCondition();
        }

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

        internal string tempFile;
        private string destinationFile;

        #region IUpdateTask Members

        public string Description { get; set; }

        public Conditions.BooleanCondition UpdateConditions { get; set; }

        public bool Prepare(Sources.IUpdateSource source)
        {
            if (string.IsNullOrEmpty(LocalPath))
                return true; // Errorneous case, but there's nothing to prepare to...

            string fileName;
            if (!string.IsNullOrEmpty(UpdateTo))
                fileName = UpdateTo;
            else
                fileName = LocalPath;

            tempFile = null;

            try
            {
                string tempFileLocal = Path.Combine(UpdateManager.Instance.TempFolder, Guid.NewGuid().ToString());
                if (!source.GetData(fileName, UpdateManager.Instance.BaseUrl, ref tempFileLocal))
                    return false;

                tempFile = tempFileLocal;
            }
            catch (Exception ex)
            {
                throw new UpdateProcessFailedException("Couldn't get Data from source", ex);
            }

            if (!string.IsNullOrEmpty(Sha256Checksum))
            {
                string checksum = Utils.FileChecksum.GetSHA256Checksum(tempFile);
                if (!checksum.Equals(Sha256Checksum))
                    return false;
            }

            return true;
        }

        public bool Execute()
        {
            if (string.IsNullOrEmpty(LocalPath))
                return true;

            destinationFile = Path.Combine(Path.GetDirectoryName(UpdateManager.Instance.ApplicationPath), LocalPath);

			if (!Directory.Exists(Path.GetDirectoryName(destinationFile)))
				Utils.FileSystem.CreateDirectoryStructure(Path.GetDirectoryName(destinationFile), false);

            // Create a backup copy if target exists
            if (File.Exists(destinationFile))
                File.Copy(destinationFile, Path.Combine(UpdateManager.Instance.BackupFolder, LocalPath));

            // Only enable execution if the apply attribute was set to hot-swap
            if (CanHotSwap)
            {
                try
                {
                    if (File.Exists(destinationFile))
                        File.Delete(destinationFile);
                    File.Move(tempFile, destinationFile);
                	tempFile = null;
                }
                catch //(Exception ex)
                {
					// TODO: Don't rethrow, but rather revert and make this a cold update?
					//throw new UpdateProcessFailedException("Couldn't move hot-swap file into position", ex);

					// Seems to work quite nicely as an alternative to the exception
					CanHotSwap = false;
				}
            }
            return true;
        }

    	public IEnumerator<KeyValuePair<string, object>> GetColdUpdates()
    	{
            if (tempFile != null && !string.IsNullOrEmpty(LocalPath) && !CanHotSwap)
					yield return new KeyValuePair<string, object>(LocalPath, tempFile);
    	}

    	public bool Rollback()
        {
            if (string.IsNullOrEmpty(destinationFile))
                return true;

            // Copy the backup copy back to its original position
            if (File.Exists(destinationFile))
                File.Delete(destinationFile);
            File.Copy(Path.Combine(UpdateManager.Instance.BackupFolder, LocalPath), destinationFile);

            return true;
        }

        public bool MustRunPrivileged() {
            if (File.Exists(destinationFile)) {
                return !Utils.PermissionsCheck.HaveWritePermissionsForFileOrFolder(destinationFile);
            } else {
                return false;
            }
        }
        
        #endregion
    }
}
