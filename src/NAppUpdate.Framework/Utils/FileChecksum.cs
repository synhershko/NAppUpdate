using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace NAppUpdate.Framework.Utils
{
	public static class FileChecksum
	{
		public static string GetSHA256Checksum(string filePath)
		{
			using (FileStream stream = File.OpenRead(filePath))
			{
				SHA256Managed sha = new SHA256Managed();
				byte[] checksum = sha.ComputeHash(stream);
				return BitConverter.ToString(checksum).Replace("-", string.Empty);
			}
		}

		public static string GetSHA256Checksum(byte[] fileData)
		{
			SHA256Managed sha = new SHA256Managed();
			byte[] checksum = sha.ComputeHash(fileData);
			return BitConverter.ToString(checksum).Replace("-", String.Empty);
		}
	}
}
