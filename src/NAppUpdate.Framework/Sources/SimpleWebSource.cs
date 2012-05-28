using System;
using System.Collections.Generic;
using System.Text;

using NAppUpdate.Framework.Utils;
using System.Net;
using System.IO;

namespace NAppUpdate.Framework.Sources
{
    public class SimpleWebSource : IUpdateSource
    {
    	public IWebProxy Proxy { get; set; }
		public string FeedUrl { get; set; }

		public SimpleWebSource()
		{
			Proxy = null;
		}

        public SimpleWebSource(string feedUrl)
        {
			FeedUrl = feedUrl;
			Proxy = null;
        }

        #region IUpdateSource Members

        public string GetUpdatesFeed()
        {
            string data = null;

            try
            {
        	var request = WebRequest.Create(FeedUrl);
			request.Method = "GET";
            request.Proxy = Proxy;
			using (var response = request.GetResponse())
			{
				if (response == null) return null;
				using (var reader = new StreamReader(response.GetResponseStream(), true))
				{
					data = reader.ReadToEnd();
				}
			}

            }
            catch (WebException e)
            {
                Console.WriteLine ("WebException: " + e.Message);
            }
            catch (UriFormatException e)
            {
                Console.WriteLine ("UriFormatWebException: " + e.Message);
            }
        	return data;
        }

        public bool GetData(string url, string baseUrl, ref string tempLocation)
        {
            FileDownloader fd = null;
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
                fd = new FileDownloader(url);
            else if (Uri.IsWellFormedUriString(baseUrl, UriKind.Absolute))
                fd = new FileDownloader(new Uri(new Uri(baseUrl, UriKind.Absolute), url));

            if (fd == null)
                throw new ArgumentException("The requested URI does not look valid: " + url, "url");

        	fd.Proxy = Proxy;

            if (string.IsNullOrEmpty(tempLocation) || !Directory.Exists(Path.GetDirectoryName(tempLocation)))
                // WATCHOUT!!! Files downloaded to a path specified by GetTempFileName may be deleted on
                // application restart, and as such cannot be relied on for cold updates, only for hot-swaps or
                // files requiring pre-processing
                tempLocation = Path.GetTempFileName();

            return fd.DownloadToFile(tempLocation);
        }

        #endregion
    }
}
