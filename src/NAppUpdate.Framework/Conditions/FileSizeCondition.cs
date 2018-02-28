using System;
using System.IO;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Utils;

namespace NAppUpdate.Framework.Conditions
{
	[Serializable]
	public class FileSizeCondition : IUpdateCondition
	{
		[NauField("localPath",
			"The local path of the file to check. If not set but set under a FileUpdateTask, the LocalPath of the task will be used. Otherwise this condition will be ignored."
			, false)]
		public string LocalPath { get; set; }

		[NauField("size", "File size to compare with (in bytes)", true)]
		public long FileSize { get; set; }

		[NauField("what", "Comparison action to perform. Accepted values: above, is, below. Default: below.", false)]
		public string ComparisonType { get; set; }

		public bool IsMet(Tasks.IUpdateTask task)
		{
			if (FileSize <= 0)
				return true;

			var localPath = !string.IsNullOrEmpty(LocalPath)
				? LocalPath
				: Utils.Reflection.GetNauAttribute(task, "LocalPath") as string;

			// local path is invalid, we can't check for anything so we will return as if the condition was met
			if (string.IsNullOrEmpty(localPath))
				return true;

			var fullPath = FileSystem.GetFullPath(localPath);

			long localFileSize = 0;
			if (File.Exists(fullPath))
			{
				var fi = new FileInfo(fullPath);
				localFileSize = fi.Length;
			}

			switch (ComparisonType)
			{
				case "above":
					return FileSize < localFileSize;
				case "is":
					return FileSize == localFileSize;
			}
			return FileSize > localFileSize;
		}
	}
}
