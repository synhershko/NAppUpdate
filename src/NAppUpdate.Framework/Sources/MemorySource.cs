using System;
using System.Collections.Generic;
using NAppUpdate.Framework.Common;

namespace NAppUpdate.Framework.Sources
{
	public class MemorySource : IUpdateSource
	{
		private readonly Dictionary<Uri, string> tempFiles;

		public MemorySource(string feedString)
		{
			this.Feed = feedString;
			this.tempFiles = new Dictionary<Uri, string>();
		}

		public string Feed { get; set; }

		public void AddTempFile(Uri uri, string path)
		{
			tempFiles.Add(uri, path);
		}

		#region IUpdateSource Members

		public string GetUpdatesFeed()
		{
			return Feed;
		}

		public void GetData(string filePath, string basePath, Action<UpdateProgressInfo> onProgress, ref string tempFile)
		{
			Uri uriKey = null;

			if (Uri.IsWellFormedUriString(filePath, UriKind.Absolute))
				uriKey = new Uri(filePath);
			else if (Uri.IsWellFormedUriString(basePath, UriKind.Absolute))
				uriKey = new Uri(new Uri(basePath, UriKind.Absolute), filePath);

            if (uriKey == null)
                throw new ApplicationException($"Unable to create Uri where filePath is '{filePath}' and basePath is '{basePath}'");
            if (!tempFiles.ContainsKey(uriKey))
                throw new ApplicationException($"Uri '${uriKey}' not found in tempFiles");

            tempFile = tempFiles[uriKey];
		}

		#endregion
	}
}
