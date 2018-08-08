using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using FluentFTP;
using NAppUpdate.Framework.Common;

namespace NAppUpdate.Framework.Sources
{
	public class FtpSource : IUpdateSource
	{
		public string HostUrl { get; set; }

		private string feedPath { get; }

		private string feedBasePath => Path.GetDirectoryName(feedPath);

		private FtpClient ftpClient { get; }

		/// <param name="hostUrl">Url of ftp server ("ftp://somesite.com/")</param>
		/// <param name="feedPath">Local ftp path to feed ("/path/to/feed.xml")</param>
		/// <param name="login">Login of ftp user, if needed</param>
		/// <param name="password">Password of ftp user, if needed</param>
		public FtpSource(string hostUrl, string feedPath, string login = null, string password = null)
		{
			ftpClient = new FtpClient(hostUrl);
			HostUrl = hostUrl;
			this.feedPath = feedPath;

			if (login != null && password != null)
			{
				ftpClient.Credentials = new NetworkCredential(login, password);
			}
		}

		private void TryConnectToHost()
		{
			try
			{
				ftpClient.Connect();
			}
			catch(Exception e)
			{
				throw new WebException($"Failed to connect to host: {HostUrl}. Error message: {e.Message}");
			}
		}

		#region IUpdateSource Members

		public String GetUpdatesFeed()
		{
			TryConnectToHost();

			string data = null;

			using (var fileStream = ftpClient.OpenRead(feedPath, FtpDataType.ASCII, true))
			{
				using (var streamReader = new StreamReader(fileStream))
				{
					data = streamReader.ReadToEnd();
				}
			}

			// Remove byteorder mark if necessary
			int indexTagOpening = data.IndexOf('<');
			if (indexTagOpening > 0)
			{
				data = data.Substring(indexTagOpening);
			}

			return data;
		}

		public Boolean GetData(String filePath, String basePath, Action<UpdateProgressInfo> onProgress, ref String tempLocation)
		{
			ftpClient.DownloadFile(tempLocation, Path.Combine(feedBasePath, filePath));
			return true;
		}

		#endregion
	}
}
