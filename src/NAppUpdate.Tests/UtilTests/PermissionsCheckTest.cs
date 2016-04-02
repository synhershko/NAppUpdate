using NAppUpdate.Framework.Utils;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NAppUpdate.Tests
{
	/// <summary>
	///This is a test class for PermissionsCheckTest and is intended
	///to contain all PermissionsCheckTest Unit Tests
	///</summary>
	[TestClass()]
	public class PermissionsCheckTest
	{
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		// 
		//You can use the following additional attributes as you write your tests:
		//
		//Use ClassInitialize to run code before running the first test in the class
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion

		/// <summary>
		/// Test whether HaveWritePermissionsForFolder correctly returns on folder for which write permissions are permitted
		///</summary>
		[TestMethod()]
		public void HaveWritePermissionsForFolderTest()
		{
			string path = Path.GetTempPath(); //Guaranteed writable (I believe)
			bool expected = true; // TODO: Initialize to an appropriate value
			bool actual;
			actual = PermissionsCheck.HaveWritePermissionsForFolder(path);
			Assert.AreEqual(expected, actual);
		}

		/// <summary>
		/// Test whether HaveWritePermissionsForFolder correctly returns on folder for which write permissions are not granted
		/// </summary>
		[TestMethod()]
		public void HaveWritePermissionsForFolderDeniedTest()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.System);
			bool expected = false; // TODO: Initialize to an appropriate value
			bool actual;
			actual = PermissionsCheck.HaveWritePermissionsForFolder(path);
			Assert.AreEqual(expected, actual);
		}
	}
}
