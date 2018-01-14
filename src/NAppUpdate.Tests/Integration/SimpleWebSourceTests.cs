using System;
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
			const string expected = "<!doctype html>";

			var ws = new SimpleWebSource("http://builds.hibernatingrhinos.com/latest/nhprof");
			var str = ws.GetUpdatesFeed();
			Assert.IsNotNull(str);
			str = str.Trim();
			Assert.IsTrue(str.StartsWith(expected, StringComparison.InvariantCultureIgnoreCase));
		}
	}
}
