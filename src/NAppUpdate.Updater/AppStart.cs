using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Tasks;
using NAppUpdate.Framework.Utils;
using System.Runtime.InteropServices;

namespace NAppUpdate.Updater
{
    internal static class AppStart
    {
        private static ArgumentsParser _args;
        private static Logger _logger;
        private static ConsoleForm _console;
        private static NauIpc.NauDto _dto;
        private static string _logFilePath = string.Empty;
        private static string _workingDir = string.Empty;
        private static bool _appRunning = true;

        private static void Main()
        {
            try
            {
                Setup();
                PerformUpdates();
            }
            catch (Exception ex)
            {
                Environment.ExitCode = Marshal.GetHRForException(ex);

                Log(ex);

                if (!_appRunning && !_args.Log && !_args.ShowConsole)
                {
                    MessageBox.Show(ex.ToString());
                }

                EventLog.WriteEntry("NAppUpdate.Updater", ex.ToString(), EventLogEntryType.Error);
            }
            finally
            {
                Teardown();
            }
        }

        private static void Setup()
        {
            _workingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            _logger = UpdateManager.Instance.Logger;
            _args = ArgumentsParser.Get();

            _args.ParseCommandLineArgs();
            if (_args.ShowConsole)
            {
                _console = new ConsoleForm();
                _console.Show();
            }

            Log("Starting to process cold updates...");

            if (_args.Log)
            {
                // Setup a temporary location for the log file, until we can get the DTO
                _logFilePath = Path.Combine(_workingDir, @"NauUpdate.log");
            }
        }

        private static void PerformUpdates()
        {
            string syncProcessName = _args.ProcessName;

            if (string.IsNullOrEmpty(syncProcessName))
            {
                throw new ArgumentException("Required command line argument is missing", "ProcessName");
            }

            Log("Update process name: '{0}'", syncProcessName);

            // Load extra assemblies to the app domain, if present
            var availableAssemblies = FileSystem.GetFiles(_workingDir, "*.exe|*.dll", SearchOption.TopDirectoryOnly);
            foreach (var assemblyPath in availableAssemblies)
            {
                Log("Loading {0}", assemblyPath);

                if (assemblyPath.Equals(Assembly.GetEntryAssembly().Location, StringComparison.InvariantCultureIgnoreCase) || assemblyPath.EndsWith("NAppUpdate.Framework.dll"))
                {
                    Log("\tSkipping (part of current execution)");
                    continue;
                }

                try
                {
                    // ReSharper disable UnusedVariable
                    var assembly = Assembly.LoadFile(assemblyPath);
                    // ReSharper restore UnusedVariable
                }
                catch (BadImageFormatException ex)
                {
                    Log("\tSkipping due to an error: {0}", ex.Message);
                }
            }

            // Connect to the named pipe and retrieve the updates list
            _dto = NauIpc.ReadDto(syncProcessName);

            // Make sure we start updating only once the application has completely terminated
            Thread.Sleep(1000); // Let's even wait a bit
            bool createdNew;
            using (var mutex = new Mutex(false, syncProcessName + "Mutex", out createdNew))
            {
                try
                {
                    if (!createdNew)
                    {
                        mutex.WaitOne();
                    }
                }
                catch (AbandonedMutexException)
                {
                    // An abandoned mutex is exactly what we are expecting...
                }
                finally
                {
                    Log("The application has terminated (as expected)");
                    _appRunning = false;
                }
            }

            _logger.LogItems.InsertRange(0, _dto.LogItems);
            _dto.LogItems = _logger.LogItems;

            // Get some required environment variables
            string appPath = _dto.AppPath;
            string appDir = _dto.WorkingDirectory ?? Path.GetDirectoryName(appPath) ?? string.Empty;
            string relaunchAppArgs = _dto.Configs.RelaunchAppArgs;

            if (!string.IsNullOrEmpty(_dto.AppPath))
            {
                _logFilePath = Path.Combine(Path.GetDirectoryName(_dto.AppPath), @"NauUpdate.log"); // now we can log to a more accessible location
            }

            if (_dto.Tasks == null)
            {
                throw new Exception("The Task list received in the dto is null");
            }
            else if (_dto.Tasks.Count == 0)
            {
                throw new Exception("The Task list received in the dto is empty");
            }

            Log("Got {0} task objects", _dto.Tasks.Count);

            // Perform the actual off-line update process
            foreach (var t in _dto.Tasks)
            {
                Log("Task \"{0}\": {1}", t.Description, t.ExecutionStatus);

                if (t.ExecutionStatus != TaskExecutionStatus.RequiresAppRestart && t.ExecutionStatus != TaskExecutionStatus.RequiresPrivilegedAppRestart)
                {
                    Log("\tSkipping");
                    continue;
                }

                Exception exception = null;

                try
                {
                    Log("\tExecuting...");
                    t.ExecutionStatus = t.Execute(true);
                }
                catch (Exception ex)
                {
                    t.ExecutionStatus = TaskExecutionStatus.Failed;
                    exception = ex;
                }

                if (t.ExecutionStatus != TaskExecutionStatus.Successful)
                {
                    string taskFailedMessage = string.Format("Update failed, task execution failed, description: {0}, execution status: {1}", t.Description, t.ExecutionStatus);
                    throw new Exception(taskFailedMessage, exception);
                }
            }

            Log("Finished successfully");
            Log("Removing backup folder");

            if (Directory.Exists(_dto.Configs.BackupFolder))
            {
                FileSystem.DeleteDirectory(_dto.Configs.BackupFolder);
            }

            // Start the application only if requested to do so
            if (_dto.RelaunchApplication)
            {
                Log("Re-launching process {0} with working dir {1}", appPath, appDir);

                bool useShellExecute = !_args.ShowConsole;

                ProcessStartInfo info = new ProcessStartInfo
                {
                    UseShellExecute = useShellExecute,
                    WorkingDirectory = appDir,
                    FileName = appPath,
                    Arguments = "-nappupdate-afterrestart" + " " + relaunchAppArgs
                };

                try
                {
                    NauIpc.LaunchProcessAndSendDto(_dto, info, syncProcessName);
                    _appRunning = true;
                }
                catch (Exception ex)
                {
                    throw new UpdateProcessFailedException("Unable to relaunch application and/or send DTO", ex);
                }
            }
        }

        private static void Teardown()
        {
            if (_args.Log)
            {
                // at this stage we can't make any assumptions on correctness of the path
                FileSystem.CreateDirectoryStructure(_logFilePath, true);
                _logger.Dump(_logFilePath);
            }

            if (_args.ShowConsole)
            {
                if (_args.Log)
                {
                    _console.WriteLine();
                    _console.WriteLine("Log file was saved to {0}", _logFilePath);
                    _console.WriteLine();
                }
                _console.WriteLine();
                _console.WriteLine("Press any key or close this window to exit.");
                _console.ReadKey();
            }

            if (_dto != null && _dto.Configs != null & !string.IsNullOrEmpty(_dto.Configs.TempFolder))
            {
                SelfCleanUp(_dto.Configs.TempFolder);
            }

            Application.Exit();
        }

        private static void SelfCleanUp(string tempFolder)
        {
            // Delete the updater EXE and the temp folder
            Log("Removing updater and temp folder... {0}", tempFolder);
            try
            {
                var info = new ProcessStartInfo
                {
                    Arguments = string.Format(@"/C ping 1.1.1.1 -n 1 -w 3000 > Nul & echo Y|del ""{0}\*.*"" & rmdir ""{0}""", tempFolder),
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    FileName = "cmd.exe"
                };

                Process.Start(info);
            }
            catch
            {
                /* ignore exceptions thrown while trying to clean up */
            }
        }

        private static void Log(string message, params object[] args)
        {
            Log(Logger.SeverityLevel.Debug, message, args);
        }

        private static void Log(Logger.SeverityLevel severity, string message, params object[] args)
        {
            message = string.Format(message, args);

            _logger.Log(severity, message);

            if (_args.ShowConsole)
            {
                _console.WriteLine(message);

                Application.DoEvents();
            }
        }

        private static void Log(Exception ex)
        {
            _logger.Log(ex);

            if (_args.ShowConsole)
            {
                _console.WriteLine("*********************************");
                _console.WriteLine("   An error has occurred:");
                _console.WriteLine("   " + ex);
                _console.WriteLine("*********************************");

                _console.WriteLine();
                _console.WriteLine("The updater will close when you close this window.");

                Application.DoEvents();
            }
        }
    }
}
