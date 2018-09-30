using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Conditions;
using NAppUpdate.Framework.Tasks;

namespace NAppUpdate.Tests.FeedReaders
{
	/// <summary>
	/// Summary description for NauXmlFeedReaderTest
	/// </summary>
	[TestClass]
	public class NauXmlFeedReaderTests
	{
		[TestMethod]
		public void NauReaderCanReadFeed1()
		{
			const string NauUpdateFeed =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<Feed>
  <Title>My application</Title>
  <Link>http://myapp.com/</Link>
  <Tasks>
    <FileUpdateTask localPath=""test.dll"" updateTo=""remoteFile.dll"">
      <Description>update details</Description>
      <Conditions>
        <FileExistsCondition localPath=""otherFile.dll"" />
      </Conditions>
    </FileUpdateTask>
  </Tasks>
</Feed>";

			var reader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReader();
			IList<IUpdateTask> updates = reader.Read(NauUpdateFeed);

			Assert.IsTrue(updates.Count == 1);

			var task = updates[0] as FileUpdateTask;
			Assert.IsNotNull(task);
			Assert.IsFalse(task.CanHotSwap);
			Assert.AreEqual("test.dll", task.LocalPath);
			Assert.AreEqual("remoteFile.dll", task.UpdateTo);
			Assert.IsNull(task.Sha256Checksum);
			Assert.IsNotNull(task.Description);

			Assert.AreEqual(1, task.UpdateConditions.ChildConditionsCount);

			var cnd = task.UpdateConditions.Degrade() as FileExistsCondition;
			Assert.IsNotNull(cnd);
			Assert.AreEqual("otherFile.dll", cnd.LocalPath);
		}

		[TestMethod]
		public void NauReaderCanReadFeed2()
		{
			const string NauUpdateFeed =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<Feed>
  <Title>My application</Title>
  <Link>http://myapp.com/</Link>
  <Tasks>
    <FileUpdateTask localPath=""test.dll"" updateTo=""remoteFile.dll"" hotswap=""true"">
      <Description>update details</Description>
      <Conditions>
        <FileVersionCondition what=""below"" version=""1.0.176.0"" />
      </Conditions>
    </FileUpdateTask>
  </Tasks>
</Feed>";

			var reader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReader();
			IList<IUpdateTask> updates = reader.Read(NauUpdateFeed);

			Assert.IsTrue(updates.Count == 1);

			var task = updates[0] as FileUpdateTask;
			Assert.IsNotNull(task);
			Assert.IsTrue(task.CanHotSwap);

			Assert.AreEqual(1, task.UpdateConditions.ChildConditionsCount);

			var cnd = task.UpdateConditions.Degrade() as FileVersionCondition;
			Assert.IsNotNull(cnd);
			Assert.IsNull(cnd.LocalPath);

			Assert.AreEqual("below", cnd.ComparisonType);
			Assert.AreEqual("1.0.176.0", cnd.Version);
		}

		[TestMethod]
		public void NauReaderCanReadFeed3()
		{
			const string NauUpdateFeed =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<Feed>
  <Title>My application</Title>
  <Link>http://myapp.com/</Link>
  <Tasks>
    <FileUpdateTask localPath=""test.dll"" updateTo=""remoteFile.dll"" hotswap=""true"">
      <Description>update details</Description>
      <Conditions>
        <OSCondition bit=""64"" />
      </Conditions>
    </FileUpdateTask>
  </Tasks>
</Feed>";

			var reader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReader();
			IList<IUpdateTask> updates = reader.Read(NauUpdateFeed);

			Assert.IsTrue(updates.Count == 1);

			var task = updates[0] as FileUpdateTask;
			Assert.IsNotNull(task);
			Assert.IsTrue(task.CanHotSwap);

			Assert.AreEqual(1, task.UpdateConditions.ChildConditionsCount);

			var cnd = task.UpdateConditions.Degrade() as OSCondition;
			Assert.IsNotNull(cnd);

			Assert.AreEqual(64, cnd.OsBits);
		}

		[TestMethod]
		public void NauReaderCanReadFeed4()
		{
			const string NauUpdateFeed =
				@"<?xml version=""1.0"" encoding=""utf-8""?>
<Feed>
  <Title>My application</Title>
  <Link>http://myapp.com/</Link>
  <Tasks>
    <FileUpdateTask localPath=""test.dll"" updateTo=""remoteFile.dll"" hotswap=""true"" version=""1.0.0.0"">
      <Description>update details</Description>
      <Conditions>
        <OSCondition bit=""64"" />
      </Conditions>
    </FileUpdateTask>
  </Tasks>
</Feed>";

			var reader = new NAppUpdate.Framework.FeedReaders.NauXmlFeedReader();
			IList<IUpdateTask> updates = reader.Read(NauUpdateFeed);

			Assert.IsTrue(updates.Count == 1);

			var task = updates[0] as FileUpdateTask;
			Assert.IsNotNull(task);
			Assert.IsTrue(task.CanHotSwap);

			Assert.AreEqual(1, task.UpdateConditions.ChildConditionsCount);

			var cnd = task.UpdateConditions.Degrade() as OSCondition;
			Assert.IsNotNull(cnd);

			Assert.AreEqual(64, cnd.OsBits);
		}
	}
}
