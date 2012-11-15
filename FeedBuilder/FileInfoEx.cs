using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
namespace FeedBuilder
{

	public class FileInfoEx
	{
		private FileInfo myFileInfo;
		private string myFileVersion;

		private string myHash;
		public FileInfo FileInfo {
			get { return myFileInfo; }
		}

		public string FileVersion {
			get { return myFileVersion; }
		}

		public string Hash {
			get { return myHash; }
		}

        public string RelativeName { get; private set; }

		public FileInfoEx(string fileName, int rootDirLength)
		{
			myFileInfo = new FileInfo(fileName);
			myFileVersion = FileVersionInfo.GetVersionInfo(fileName).FileVersion;
			myHash = NAppUpdate.Framework.Utils.FileChecksum.GetSHA256Checksum(fileName);
            RelativeName = fileName.Substring(rootDirLength + 1);
		}
	}
}
