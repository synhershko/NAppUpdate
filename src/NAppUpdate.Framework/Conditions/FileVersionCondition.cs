using System;
using System.IO;
using System.Diagnostics;
using NAppUpdate.Framework.Common;

namespace NAppUpdate.Framework.Conditions
{
    [UpdateConditionAlias("version")]
    public class FileVersionCondition : IUpdateCondition
    {
        [NauField("localPath",
            "The local path of the file to check. If not set but set under a FileUpdateTask, the LocalPath of the task will be used. Otherwise this condition will be ignored."
            , false)]
        public string LocalPath { get; set; }

        [NauField("version", "Version string to check against", true)]
        public string Version { get; set; }

        [NauField("what", "Comparison action to perform. Accepted values: above, is, below. Default: below.", false)]
        public string ComparisonType { get; set; }

        #region IUpdateCondition Members

        public bool IsMet(Tasks.IUpdateTask task)
        {
            string localPath = !string.IsNullOrEmpty(LocalPath)
                                   ? LocalPath
                                   : Utils.Reflection.GetNauAttribute(task, "LocalPath") as string;
            if (string.IsNullOrEmpty(localPath) || !File.Exists(localPath))
                return true;

        	var versionInfo = FileVersionInfo.GetVersionInfo(localPath);
			if (versionInfo.FileVersion == null) return true; // perform the update if no version info is found
			
			string versionString = versionInfo.FileVersion.Replace(", ", ".");
            Version localVersion = new Version(versionString);
            Version updateVersion = Version != null ? new Version(Version) : new Version();

            switch (ComparisonType)
            {
                case "above":
                    return updateVersion < localVersion;
                case "is":
                    return updateVersion == localVersion;
                default:
                    return updateVersion > localVersion;
            }
        }

        #endregion
    }
}
