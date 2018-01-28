using NAppUpdate.Framework.Sources;

namespace NAppUpdate.Tests.Integration
{
	using Xunit;

	public class SimpleWebSourceTests
	{
		[Fact]
		public void can_download_ansi_feed()
		{
			const string expected = "NHibernate.Profiler-Build-";

			var ws = new SimpleWebSource("http://builds.hibernatingrhinos.com/latest/nhprof");
			var str = ws.GetUpdatesFeed();

			Assert.Equal(expected, str.Substring(0, expected.Length));
		}
	}
}
