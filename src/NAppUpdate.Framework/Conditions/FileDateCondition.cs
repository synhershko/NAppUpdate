using System;
using System.IO;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Utils;

namespace NAppUpdate.Framework.Conditions
{
	[Serializable]
	public class FileDateCondition : IUpdateCondition
	{
		public FileDateCondition()
		{
			Timestamp = DateTime.MinValue;
		}

		[NauField("localPath",
			"The local path of the file to check. If not set but set under a FileUpdateTask, the LocalPath of the task will be used. Otherwise this condition will be ignored."
			, false)]
		public string LocalPath { get; set; }

		[NauField("timestamp", "Date-time to compare with", true)]
		public DateTime Timestamp { get; set; }

		[NauField("what", "Comparison action to perform. Accepted values: newer, is, older. Default: older.", false)]
		public string ComparisonType { get; set; }

		public bool IsMet(Tasks.IUpdateTask task)
		{
			if (Timestamp == DateTime.MinValue)
				return true;

			string localPath = !string.IsNullOrEmpty(LocalPath)
								   ? LocalPath
								   : Utils.Reflection.GetNauAttribute(task, "LocalPath") as string;

			// local path is invalid, we can't check for anything so we will return as if the condition was met
			if (string.IsNullOrEmpty(localPath))
				return true;

			var fullPath = FileSystem.GetFullPath(localPath);

			// if the file doesn't exist it has a null timestamp, and therefore the condition result depends on the ComparisonType
			if (!File.Exists(fullPath))
				return ComparisonType.Equals("older", StringComparison.InvariantCultureIgnoreCase);

			// File timestamps seem to be off by a little bit (conversion rounding?), so the code below
			// gets around that
			var dt = File.GetLastWriteTime(fullPath);
			var localPlus = dt.AddSeconds(2).ToFileTimeUtc();
			var localMinus = dt.AddSeconds(-2).ToFileTimeUtc();
			var remoteFileDateTime = Timestamp.ToFileTimeUtc();

			bool result;
			switch (ComparisonType)
			{
				case "newer":
					result = localMinus > remoteFileDateTime;
					break;
				case "is":
					result = localMinus <= remoteFileDateTime && remoteFileDateTime <= localPlus;
					break;
				default:
					result = localPlus < remoteFileDateTime;
					break;
			}
			return result;
		}

		public override string ToString()
		{
			return "Timestamp " + ComparisonType + " " + Timestamp;
		}
	}
}
