using System;
using System.Text;
using System.Collections.Generic;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Conditions;
using NAppUpdate.Framework.Tasks;

namespace NAppUpdate.Tests.FeedReaders
{
	using Xunit;

	/// <summary>
	/// Summary description for NauXmlFeedReaderTest
	/// </summary>
	public class NauXmlFeedReaderTests
	{
		[Fact]
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

			Assert.True(updates.Count == 1);

			var task = updates[0] as FileUpdateTask;
			Assert.NotNull(task);
			Assert.False(task.CanHotSwap);
			Assert.Equal("test.dll", task.LocalPath);
			Assert.Equal("remoteFile.dll", task.UpdateTo);
			Assert.Null(task.Sha256Checksum);
			Assert.NotNull(task.Description);

			Assert.Equal(1, task.UpdateConditions.ChildConditionsCount);

			var cnd = task.UpdateConditions.Degrade() as FileExistsCondition;
			Assert.NotNull(cnd);
			Assert.Equal("otherFile.dll", cnd.LocalPath);
		}

		[Fact]
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

			Assert.True(updates.Count == 1);

			var task = updates[0] as FileUpdateTask;
			Assert.NotNull(task);
			Assert.True(task.CanHotSwap);

			Assert.Equal(1, task.UpdateConditions.ChildConditionsCount);

			var cnd = task.UpdateConditions.Degrade() as FileVersionCondition;
			Assert.NotNull(cnd);
			Assert.Null(cnd.LocalPath);

			Assert.Equal("below", cnd.ComparisonType);
			Assert.Equal("1.0.176.0", cnd.Version);
		}

		[Fact]
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

			Assert.True(updates.Count == 1);

			var task = updates[0] as FileUpdateTask;
			Assert.NotNull(task);
			Assert.True(task.CanHotSwap);

			Assert.Equal(1, task.UpdateConditions.ChildConditionsCount);

			var cnd = task.UpdateConditions.Degrade() as OSCondition;
			Assert.NotNull(cnd);

			Assert.Equal(64, cnd.OsBits);
		}
	}
}
