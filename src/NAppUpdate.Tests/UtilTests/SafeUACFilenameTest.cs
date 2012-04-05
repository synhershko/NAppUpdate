using NAppUpdate.Framework.Utils;
using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NAppUpdate.Tests
{
	/// <summary>
	///This is a test class for SafeUACFilenameTest and is intended
	///to contain all SafeUACFilename Unit Tests
	/// </summary>
	[TestClass]
	public class SafeUACFilenameTest
	{
		[TestMethod]
		public void SafeFilenameTest()
		{
			string testFilename = "ThisIsASafeUACFilename";
			string safeFilename = SafeUACFilename.GetFilename(testFilename);
			Assert.AreEqual(testFilename, safeFilename);
		}

		[TestMethod]
		public void UnSafeFilenameTest()
		{
			string testFilename = "MySoftwareUpdater";
			string safeFilename = SafeUACFilename.GetFilename(testFilename);
			Assert.AreNotEqual(testFilename, safeFilename);
		}

		[TestMethod]
		public void SafeBlankFilenameTest()
		{
			string testFilename = "Update";
			string safeFilename = SafeUACFilename.GetFilename(testFilename);
			Assert.AreNotEqual(testFilename, safeFilename);
			Assert.AreEqual(safeFilename, "foo");
		}
	}
}
