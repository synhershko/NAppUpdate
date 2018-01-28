using NAppUpdate.Framework.Tasks;

namespace NAppUpdate.Tests.Tasks
{
	using Xunit;

	public class BasicTaskTests
	{
		[Fact]
		public void TestTaskDefaultCharacteristics()
		{
			var task = new FileUpdateTask(); // just a random task object
			Assert.True(task.ExecutionStatus == TaskExecutionStatus.Pending);
		}
	}
}
