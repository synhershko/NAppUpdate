using System;
using System.IO;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Tasks;
using NAppUpdate.Framework.Utils;

namespace NAppUpdate.Framework.Conditions
{
	[Serializable]
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

		public bool IsMet(IUpdateTask task)
		{
			var localPath = !string.IsNullOrEmpty(LocalPath) ? LocalPath : Utils.Reflection.GetNauAttribute(task, "LocalPath") as string;

			// local path is invalid, we can't check for anything so we will return as if the condition was met
			if (string.IsNullOrEmpty(localPath))
				return true;

			var fullPath = FileSystem.GetFullPath(localPath);

			// if the local file does not exist, checksums don't match vacuously
			if (!File.Exists(fullPath))
				return false;

			if ("sha256".Equals(ChecksumType, StringComparison.InvariantCultureIgnoreCase))
			{
				var sha256 = FileChecksum.GetSHA256Checksum(fullPath);
				if (!string.IsNullOrEmpty(sha256) && sha256.Equals(Checksum, StringComparison.InvariantCultureIgnoreCase))
					return true;
			}

			// TODO: Support more checksum algorithms (although SHA256 isn't known to have collisions, other are more commonly used)

			return false;
		}
	}
}
