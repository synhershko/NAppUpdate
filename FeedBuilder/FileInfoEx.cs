using System.Diagnostics;
using System.IO;
using NAppUpdate.Framework.Utils;

namespace FeedBuilder
{
	public class FileInfoEx
	{
		private readonly FileInfo myFileInfo;
		private readonly string myFileVersion;
		private readonly string myHash;

		public FileInfo FileInfo
		{
			get { return myFileInfo; }
		}

		public string FileVersion
		{
			get { return myFileVersion; }
		}

		public string Hash
		{
			get { return myHash; }
		}

		public FileInfoEx(string fileName)
		{
			myFileInfo = new FileInfo(fileName);
			myFileVersion = FileVersionInfo.GetVersionInfo(fileName).FileVersion;
			myHash = FileChecksum.GetSHA256Checksum(fileName);
		}
	}
}