using System;
using System.IO;
using System.Diagnostics;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Utils;

namespace NAppUpdate.Framework.Conditions
{
	[Serializable]
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

		public bool IsMet(Tasks.IUpdateTask task)
		{
			var localPath = !string.IsNullOrEmpty(LocalPath)
				? LocalPath
				: Utils.Reflection.GetNauAttribute(task, "LocalPath") as string;

			// local path is invalid, we can't check for anything so we will return as if the condition was met
			if (string.IsNullOrEmpty(localPath))
				return true;

			var fullPath = FileSystem.GetFullPath(localPath);

			// if the file doesn't exist it has a null version, and therefore the condition result depends on the ComparisonType
			if (!File.Exists(fullPath))
				return ComparisonType.Equals("below", StringComparison.InvariantCultureIgnoreCase);

			var versionInfo = FileVersionInfo.GetVersionInfo(fullPath);
			if (versionInfo.FileVersion == null) return true; // perform the update if no version info is found

			var localVersion = new Version(versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart);
			var updateVersion = Version != null ? new Version(Version) : new Version();

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

		public override string ToString()
		{
			return "Version " + ComparisonType + " " + Version;
		}
	}
}
