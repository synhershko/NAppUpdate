using NAppUpdate.Framework.Utils;

namespace NAppUpdate.Tests.Integration
{
	using Xunit;

	public class FileDownloaderTests
	{
		[Fact]
		public void Should_be_able_to_download_a_small_file_from_the_internet()
		{
			var fileDownloader = new FileDownloader("http://www.google.co.uk/intl/en_uk/images/logo.gif");

			byte[] fileData = fileDownloader.Download();

			Assert.True(fileData.Length > 0);
		}
	}
}
