using System;
using System.IO;
using System.Windows;
using System.Diagnostics;
using NAppUpdate.Updater.Zip;

namespace NAppUpdate.Updater
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		public App()
		{
			base.Startup += App_Startup;
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			Debugger.Launch();
			string[] args = Environment.GetCommandLineArgs();

			var compressedUpdateFile = args[2];
			var appPath = args[1];

			if (!File.Exists(compressedUpdateFile))
				throw new FileNotFoundException("could not find the update file, did it download correctly? " +
												compressedUpdateFile);

			if (!File.Exists(appPath))
				throw new FileNotFoundException("could not find the application file " + appPath);


			//TODO: wrap in try catch because of potential violations
			foreach (var process in Process.GetProcessesByName(Path.GetFileNameWithoutExtension(appPath)))
				process.WaitForExit();

			ExtractAndStartApplication(compressedUpdateFile, appPath);

			//TODO: wrap in try catch as it could fail if we do not have access rights
			if (File.Exists(compressedUpdateFile))
				File.Delete(compressedUpdateFile);

			Application.Current.Shutdown();
		}

		private void ExtractAndStartApplication(string updateFilePath, string applicationFilePath)
		{
			var extractor = new ZipFileExtractor(updateFilePath);
			extractor.ExtractTo(Environment.CurrentDirectory);

			Process.Start(applicationFilePath);
		}
	}
}
