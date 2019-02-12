using System;
using NAppUpdate.Framework.Common;

namespace NAppUpdate.Framework.Sources
{
	public interface IUpdateSource
	{
		string GetUpdatesFeed(); // TODO: return a the feed as a stream
		void GetData(string filePath, string basePath, Action<UpdateProgressInfo> onProgress, ref string tempLocation);
	}
}
