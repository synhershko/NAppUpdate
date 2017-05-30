using System;
using System.Diagnostics;
using System.IO;

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
		private static readonly string[] SizeSuffixes =
				   { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
		public string getFileSize(int decimalPlaces = 1)
		{
			long value = myFileInfo.Length;
			if (value == 0) { return "0.0 bytes"; }

			// mag is 0 for bytes, 1 for KB, 2, for MB, etc.
			int mag = (int)Math.Log(value, 1024);

			// 1L << (mag * 10) == 2 ^ (10 * mag) 
			// [i.e. the number of bytes in the unit corresponding to mag]
			decimal adjustedSize = (decimal)value / (1L << (mag * 10));

			// make adjustment when the value is large enough that
			// it would round up to 1000 or more
			if (Math.Round(adjustedSize, decimalPlaces) >= 1000)
			{
				mag += 1;
				adjustedSize /= 1024;
			}

			return $"{adjustedSize.ToString("n" + decimalPlaces.ToString())} {SizeSuffixes[mag]}";
		}

		public string RelativeName { get; private set; }

		public FileInfoEx(string fileName, int rootDirLength)
		{
			myFileInfo = new FileInfo(fileName);
			var verInfo = FileVersionInfo.GetVersionInfo(fileName);
			myFileVersion = new Version(verInfo.FileMajorPart, verInfo.FileMinorPart, verInfo.FileBuildPart, verInfo.FilePrivatePart).ToString();
			RelativeName = fileName.Substring(rootDirLength);
			while (RelativeName.StartsWith(@"\"))
				RelativeName = RelativeName.Substring(1);//Just in case the file name starts with a \
			myHash = NAppUpdate.Framework.Utils.FileChecksum.GetSHA256Checksum(fileName);

		}
	}
}
