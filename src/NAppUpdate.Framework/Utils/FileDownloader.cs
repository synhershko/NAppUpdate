using System;
using System.Globalization;
using System.IO;
using System.Net;
using NAppUpdate.Framework.Common;

namespace NAppUpdate.Framework.Utils
{
	public sealed class FileDownloader
	{
		private readonly Uri _uri;
		private const int _bufferSize = 1024;
		public IWebProxy Proxy { get; set; }

		public FileDownloader()
		{
			Proxy = null;
		}


		public FileDownloader(string url)
		{
			_uri = new Uri(url);
		}

		public FileDownloader(Uri uri)
		{
			_uri = uri;
		}

		public byte[] Download()
		{
			using (var client = new WebClient())
				return client.DownloadData(_uri);
		}

		public bool DownloadToFile(string tempLocation)
		{
			return DownloadToFile(tempLocation, null);
		}

		public bool DownloadToFile(string tempLocation, Action<UpdateProgressInfo> onProgress)
		{
			var request = WebRequest.Create(_uri);
			request.Proxy = Proxy;

			using (var response = request.GetResponse())
			using (var tempFile = File.Create(tempLocation))
			{
				using (var responseStream = response.GetResponseStream())
				{
					if (responseStream == null)
						return false;

					long downloadSize = response.ContentLength;
					long totalBytes = 0;
					var buffer = new byte[_bufferSize];
					const int reportInterval = 1;
					DateTime stamp = DateTime.Now.Subtract(new TimeSpan(0, 0, reportInterval));
					int bytesRead;
					do
					{
						bytesRead = responseStream.Read(buffer, 0, buffer.Length);
						totalBytes += bytesRead;
						tempFile.Write(buffer, 0, bytesRead);

						if (onProgress == null || !(DateTime.Now.Subtract(stamp).TotalSeconds >= reportInterval)) continue;
						ReportProgress(onProgress, totalBytes, downloadSize);
						stamp = DateTime.Now;
					} while (bytesRead > 0 && !UpdateManager.Instance.ShouldStop);

					ReportProgress(onProgress, totalBytes, downloadSize);
					return totalBytes == downloadSize;
				}
			}
		}

		private void ReportProgress(Action<UpdateProgressInfo> onProgress, long totalBytes, long downloadSize)
		{
			if (onProgress != null) onProgress(new DownloadProgressInfo
			{
				DownloadedInBytes = totalBytes,
				FileSizeInBytes = downloadSize,
				Percentage = (int)((totalBytes / downloadSize) * 100),
				Message = string.Format("Downloading... ({0} / {1} completed)", ToFileSizeString(totalBytes), ToFileSizeString(downloadSize)),
				StillWorking = totalBytes == downloadSize,
			});
		}

		private string ToFileSizeString(long size)
		{
			if (size < 1000) return String.Format("{0} bytes", size);
			if (size < 1000000) return String.Format("{0:F1} KB", (size / 1000));
			if (size < 1000000000) return String.Format("{0:F1} MB", (size / 1000000));
			if (size < 1000000000000) return String.Format("{0:F1} GB", (size / 1000000000));
			if (size < 1000000000000000) return String.Format("{0:F1} TB", (size / 1000000000000));
			return size.ToString(CultureInfo.InvariantCulture);
		}

		/*
		public void DownloadAsync(Action<byte[]> finishedCallback)
		{
			DownloadAsync(finishedCallback, null);
		}

		public void DownloadAsync(Action<byte[]> finishedCallback, Action<long, long> progressChangedCallback)
		{
			using (var client = new WebClient())
			{
				if (progressChangedCallback != null)
					client.DownloadProgressChanged += (sender, args) => progressChangedCallback(args.BytesReceived, args.TotalBytesToReceive);

				client.DownloadDataCompleted += (sender, args) => finishedCallback(args.Result);
				client.DownloadDataAsync(_uri);
			}
		}*/
	}
}