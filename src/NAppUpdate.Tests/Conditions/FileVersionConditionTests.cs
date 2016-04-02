using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAppUpdate.Framework.Conditions;

namespace NAppUpdate.Tests.Conditions
{
	[TestClass]
	public class FileVersionConditionTests
	{
		[TestMethod]
		public void ShouldAbortGracefullyOnUnversionedFiles()
		{
			var tempFile = Path.GetTempFileName();
			File.WriteAllText(tempFile, "foo");

			var cnd = new FileVersionCondition { ComparisonType = "is", LocalPath = tempFile, Version = "1.0.0.0" };
			Assert.IsTrue(cnd.IsMet(null));
		}
	}
}
