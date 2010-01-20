using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace leetreveil.AutoUpdate.Core.Appcast
{
    public class AppcastReader : IUpdateFeedSource
    {
        public IEnumerable<Update> Read(string url)
        {
            var document = XDocument.Load(url);

            XNamespace ns = "http://www.adobe.com/xml-namespaces/appcast/1.0";

            return document.Descendants("channel").Descendants("item").Select(
                item => new Update
                            {
                                Title = item.Element("title").Value,
                                Version = new Version(item.Element(ns + "version").Value),
                                FileUrl = item.Element("enclosure").Attribute("url").Value
                            });
        }
    }
}