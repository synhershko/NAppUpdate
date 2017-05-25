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

		public string RelativeName { get; private set; }

		public FileInfoEx(string fileName, int rootDirLength)
		{
			myFileInfo = new FileInfo(fileName);
			var verInfo = FileVersionInfo.GetVersionInfo(fileName);
			myFileVersion = new System.Version(verInfo.FileMajorPart, verInfo.FileMinorPart, verInfo.FileBuildPart, verInfo.FilePrivatePart).ToString();
			myHash = NAppUpdate.Framework.Utils.FileChecksum.GetSHA256Checksum(fileName);

			RelativeName = fileName.Substring(rootDirLength);
			while (RelativeName.StartsWith(@"\"))
				RelativeName = RelativeName.Substring(1);//Just in case the file name starts with a \
		}
	}
}
