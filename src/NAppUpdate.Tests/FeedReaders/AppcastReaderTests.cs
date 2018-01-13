using System;
using System.Linq;
using NAppUpdate.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NAppUpdate.Tests.Unit
{
	[TestClass]
	public class AppcastReaderTests
	{
		const string ZuneUpdateFeed =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<rss version=""2.0"" xmlns:appcast=""http://www.adobe.com/xml-namespaces/appcast/1.0"">
  <channel>
    <title>Zune Social Tagger Update Feed</title>
    <link>http://github.com/leetreveil/Zune-Social-Tagger/downloads</link>
    <description>Link an album to the zune social with any album available on the zune marketplace!</description>
    <item>
      <title>Zune Social Tagger</title>
      <link>http://github.com/leetreveil/Zune-Social-Tagger/downloads</link>
      <description>.WMA Support and other minor bug fixes</description>
      <pubDate>Sunday, 27 December 2009 00:35:00 GMT</pubDate>
      <appcast:version>1.2</appcast:version>
      <enclosure url=""http://cloud.github.com/downloads/leetreveil/Zune-Social-Tagger/Zune_Social_Tagger_1.2.zip"" length=""865843"" type=""application/octet-stream"" />
    </item>
  </channel>
</rss>";

		[TestMethod]
		public void Should_be_able_to_get_the_description_from_the_update()
		{
			var reader = new NAppUpdate.Framework.FeedReaders.AppcastReader();
			var updates = reader.Read(ZuneUpdateFeed);

			Assert.AreEqual(1, updates.Count());
			Assert.AreEqual(".WMA Support and other minor bug fixes", updates.First().Description);
		}
	}
}
