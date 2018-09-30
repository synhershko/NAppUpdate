using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAppUpdate.Framework.Sources;

namespace NAppUpdate.Tests.Integration
{
	[TestClass]
	public class SimpleWebSourceTests
	{
		[TestMethod]
		public void can_download_ansi_feed()
		{
			Assert.Inconclusive("This url contains wrong data");
			const string expected = "NHibernate.Profiler-Build-";

			var ws = new SimpleWebSource("http://builds.hibernatingrhinos.com/latest/nhprof");
			var str = ws.GetUpdatesFeed();

			Assert.AreEqual(expected, str.Substring(0, expected.Length));
		}
	}
}
