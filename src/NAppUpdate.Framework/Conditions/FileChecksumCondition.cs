using System;
using System.IO;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Tasks;

namespace NAppUpdate.Framework.Conditions
{
    public class FileChecksumCondition : IUpdateCondition
    {
        [NauField("localPath",
            "The local path of the file to check. If not set but set under a FileUpdateTask, the LocalPath of the task will be used. Otherwise this condition will be ignored."
            , false)]
        public string LocalPath { get; set; }

        [NauField("checksum", "Checksum expected from the file", true)]
        public string Checksum { get; set; }

        [NauField("checksumType", "Type of checksum to calculate", true)]
        public string ChecksumType { get; set; }

        #region IUpdateCondition Members

        public bool IsMet(IUpdateTask task)
        {
            string localPath = !string.IsNullOrEmpty(LocalPath) ? LocalPath : Utils.Reflection.GetNauAttribute(task, "LocalPath") as string;
            if (string.IsNullOrEmpty(localPath) || !File.Exists(localPath))
                return true;

            if ("sha256".Equals(ChecksumType, StringComparison.InvariantCultureIgnoreCase))
            {
                string sha256 = Utils.FileChecksum.GetSHA256Checksum(localPath);
                if (!string.IsNullOrEmpty(sha256) && !sha256.Equals(Checksum))
                    return true;
            }

            // TODO: Support more checksum algorithms (although SHA256 isn't known to have collisions, other are more commonly used)

            return false;
        }

        #endregion
    }
}