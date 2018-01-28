using System.IO;
using NAppUpdate.Framework.Conditions;

namespace NAppUpdate.Tests.Conditions
{
	using Xunit;

	public class FileVersionConditionTests
	{
		[Fact]
		public void ShouldAbortGracefullyOnUnversionedFiles()
		{
			var tempFile = Path.GetTempFileName();
			File.WriteAllText(tempFile, "foo");

			var cnd = new FileVersionCondition { ComparisonType = "is", LocalPath = tempFile, Version = "1.0.0.0" };
			Assert.True(cnd.IsMet(null));
		}
	}
}
