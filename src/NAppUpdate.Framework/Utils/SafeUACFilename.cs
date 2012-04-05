using System;
using System.Collections.Generic;
using System.Text;

namespace NAppUpdate.Framework.Utils
{
	public static class SafeUACFilename
	{
		public static string GetFilename(string filename)
		{
			string[] installerNames = {"installer", "install", "setup", "updater", "update"};

			foreach (string installerName in installerNames)
			{
				filename = ReplaceEx.Replace(filename, installerName, string.Empty, StringComparison.OrdinalIgnoreCase);
			}
			if (string.IsNullOrEmpty(filename)) { filename = "foo"; }
			return filename;
		}
	}
}
