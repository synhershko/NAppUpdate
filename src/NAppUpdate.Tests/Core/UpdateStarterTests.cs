using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Tasks;
using NAppUpdate.Framework.Utils;

namespace NAppUpdate.Tests.Core
{
	using Xunit;

	public class UpdateStarterTests
	{
		[Fact]
		public void UpdaterDeploymentWorks()
		{
			var path = Path.Combine(Path.GetTempPath(), "NAppUpdate-Tests");

			NauIpc.ExtractUpdaterFromResource(path, "Foo.exe");

			Assert.True(Directory.Exists(path));
			Assert.True(File.Exists(Path.Combine(path, "Foo.exe")));
			Assert.True(File.Exists(Path.Combine(path, "NAppUpdate.Framework.dll")));

			// Cleanup test
			NAppUpdate.Framework.Utils.FileSystem.DeleteDirectory(path);
		}

		[Fact]
		public void UpdaterDeploymentAndIPCWorks()
		{
			var dto = new NauIpc.NauDto
			{
				Configs = UpdateManager.Instance.Config,
				Tasks = new List<IUpdateTask>
				{
					new FileUpdateTask {Description = "Task #1", ExecutionStatus = TaskExecutionStatus.RequiresAppRestart},
				},
				AppPath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName,
				WorkingDirectory = Environment.CurrentDirectory,
				RelaunchApplication = false,
			};

			var path = dto.Configs.TempFolder;

			if (Directory.Exists(path))
				FileSystem.DeleteDirectory(path);

			NauIpc.ExtractUpdaterFromResource(path, dto.Configs.UpdateExecutableName);
			var info = new ProcessStartInfo
			{
				UseShellExecute = true,
				WorkingDirectory = Environment.CurrentDirectory,
				FileName = Path.Combine(path, dto.Configs.UpdateExecutableName),
				Arguments = string.Format(@"""{0}"" -showConsole", dto.Configs.UpdateProcessName),
			};

			var p = NauIpc.LaunchProcessAndSendDto(dto, info, dto.Configs.UpdateProcessName);
			Assert.Null(p);
			p.WaitForExit();

			Assert.True(Directory.Exists(path));
			Assert.True(File.Exists(Path.Combine(path, "Foo.exe")));
			Assert.True(File.Exists(Path.Combine(path, "NAppUpdate.Framework.dll")));

			// Cleanup test
			FileSystem.DeleteDirectory(path);
		}
	}
}
