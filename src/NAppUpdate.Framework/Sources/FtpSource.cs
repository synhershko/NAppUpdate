using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web.UI.WebControls;
using NAppUpdate.Framework.Common;

namespace NAppUpdate.Framework.Sources
{
	public class FtpSource : IUpdateSource
	{
		public string HostUrl { get; set; }

		private string feedPath { get; }

		private string feedBasePath => Path.GetDirectoryName(feedPath);

		private int bufferSize = 2048;

		private NetworkCredential credentials { get; }

		/// <param name="hostUrl">Url of ftp server ("ftp://somesite.com/")</param>
		/// <param name="feedPath">Local ftp path to feed ("/path/to/feed.xml")</param>
		/// <param name="login">Login of ftp user, if needed</param>
		/// <param name="password">Password of ftp user, if needed</param>
		public FtpSource(string hostUrl, string feedPath, string login = null, string password = null)
		{
			HostUrl = hostUrl;
			this.feedPath = feedPath;

			if (login != null && password != null)
			{
				credentials = new NetworkCredential(login, password);
			}
		}

		private void TryConnectToHost()
		{
			var ftpConnRequest = (FtpWebRequest)FtpWebRequest.Create(HostUrl);
			ftpConnRequest.Method = WebRequestMethods.Ftp.ListDirectory;
			ftpConnRequest.UsePassive = true;
			ftpConnRequest.KeepAlive = false;
			ftpConnRequest.Credentials = credentials;

			try
			{
				ftpConnRequest.GetResponse();
			}
			catch (WebException e)
			{
				throw new WebException($"Failed to connect to host: {HostUrl}. Error message: {e.Message}");
			}
		}

		/// <summary>
		/// Downloads remote file from ftp
		/// </summary>
		/// <param name="path">Path to file on server (example: "/path/to/file.txt")</param>
		/// <param name="localPath">Path on local machine, where file should be saved (if not provided, Temp folder will be used)</param>
		/// <returns>Path to saved on disk file (Temp folder)</returns>
		private string DownloadRemoteFile(string path, string localPath = null)
		{
			try
			{
				string pathToSave = localPath ?? Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
				// Create an FTP Request
				var ftpRequest =
					(FtpWebRequest)FtpWebRequest.Create(
						$@"{HostUrl}{(HostUrl.EndsWith("/") ? string.Empty : "/")}{path}");
				ftpRequest.Credentials = credentials;
				// Set options
				ftpRequest.UseBinary = true;
				ftpRequest.UsePassive = true;
				ftpRequest.KeepAlive = true;
				// Specify the Type of FTP Request 
				ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;

				// Establish Return Communication with the FTP Server 
				using (var ftpResponse = (FtpWebResponse)ftpRequest.GetResponse())
				{
					// Get the FTP Server's Response Stream 
					using (var ftpStream = ftpResponse.GetResponseStream())
					{
						// Save data on disk
						using (var localFileStream = new FileStream(pathToSave, FileMode.Create))
						{
							byte[] byteBuffer = new byte[bufferSize];
							int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);

							while (bytesRead > 0)
							{
								localFileStream.Write(byteBuffer, 0, bytesRead);
								bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
							}
						}
					}
				}

				return pathToSave;
			}
			catch (Exception ex)
			{
				throw new WebException(
					$"An error occurred when trying to download file {Path.GetFileName(path)}: {ex.Message}");
			}
		}

		#region IUpdateSource Members

		public String GetUpdatesFeed()
		{
			TryConnectToHost();

			string feedFilePath = DownloadRemoteFile(feedPath);
			string data = File.ReadAllText(feedFilePath);

			// Remove byteorder mark if necessary
			int indexTagOpening = data.IndexOf('<');
			if (indexTagOpening > 0)
			{
				data = data.Substring(indexTagOpening);
			}

			return data;
		}

		public Boolean GetData(String filePath, String basePath, Action<UpdateProgressInfo> onProgress,
			ref String tempLocation)
		{
			DownloadRemoteFile(Path.Combine(feedBasePath, filePath), tempLocation);
			return true;
		}

		#endregion
	}
}
