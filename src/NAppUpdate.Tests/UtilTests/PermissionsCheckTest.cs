using NAppUpdate.Framework.Utils;
using System.IO;
using System;

namespace NAppUpdate.Tests
{
	using Xunit;

	/// <summary>
	///This is a test class for PermissionsCheckTest and is intended
	///to contain all PermissionsCheckTest Unit Tests
	///</summary>
	public class PermissionsCheckTest
	{
		/// <summary>
		/// Test whether HaveWritePermissionsForFolder correctly returns on folder for which write permissions are permitted
		///</summary>
		[Fact]
		public void HaveWritePermissionsForFolderTest()
		{
			string path = Path.GetTempPath(); //Guaranteed writable (I believe)
			bool expected = true; // TODO: Initialize to an appropriate value
			bool actual;
			actual = PermissionsCheck.HaveWritePermissionsForFolder(path);
			Assert.Equal(expected, actual);
		}

		/// <summary>
		/// Test whether HaveWritePermissionsForFolder correctly returns on folder for which write permissions are not granted
		/// </summary>
		[Fact]
		public void HaveWritePermissionsForFolderDeniedTest()
		{
			string path = Environment.GetFolderPath(Environment.SpecialFolder.System);
			bool expected = false; // TODO: Initialize to an appropriate value
			bool actual;
			actual = PermissionsCheck.HaveWritePermissionsForFolder(path);
			Assert.Equal(expected, actual);
		}
	}
}
