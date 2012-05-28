using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

// Used for the named pipes implementation
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace NAppUpdate.Framework
{
    /// <summary>
    /// Starts the cold update process by extracting the updater app from the library's resources,
    /// passing it all the data it needs and terminating the current application
    /// </summary>
    internal class UpdateStarter
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateNamedPipe(
           String pipeName,
           uint dwOpenMode,
           uint dwPipeMode,
           uint nMaxInstances,
           uint nOutBufferSize,
           uint nInBufferSize,
           uint nDefaultTimeOut,
           IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern int ConnectNamedPipe(
           SafeFileHandle hNamedPipe,
           IntPtr lpOverlapped);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern SafeFileHandle CreateFile(
           String pipeName,
           uint dwDesiredAccess,
           uint dwShareMode,
           IntPtr lpSecurityAttributes,
           uint dwCreationDisposition,
           uint dwFlagsAndAttributes,
           IntPtr hTemplate);

        //private const uint DUPLEX = (0x00000003);
        private const uint WRITE_ONLY = (0x00000002);
        private const uint FILE_FLAG_OVERLAPPED = (0x40000000);

        internal string PIPE_NAME { get { return string.Format("\\\\.\\pipe\\{0}", _syncProcessName); } }
        internal uint BUFFER_SIZE = 4096;

        private readonly string _updaterPath;
        private readonly Dictionary<string, object> _updateData;
        private readonly string _syncProcessName;

        private readonly bool _runPrivileged;
        private bool _updaterDoLogging;
        private bool _updaterShowConsole;

        public UpdateStarter(string pathWhereUpdateExeShouldBeCreated,
            Dictionary<string, object> updateData, string syncProcessName, bool runPrivileged)
        {
            _updaterPath = pathWhereUpdateExeShouldBeCreated;
            _updateData = updateData;
            _syncProcessName = syncProcessName;
            _runPrivileged = runPrivileged;
        }

        public void SetOptions(bool updaterDoLogging, bool updaterShowConsole)
        {
            _updaterDoLogging = updaterDoLogging;
            _updaterShowConsole = updaterShowConsole;
        }

        public bool Start()
        {
            ExtractUpdaterFromResource(); //take the update executable and extract it to the path where it should be created

            using (var clientPipeHandle = CreateNamedPipe(
                   PIPE_NAME,
                   WRITE_ONLY | FILE_FLAG_OVERLAPPED,
                   0,
                   1, // 1 max instance (only the updater utility is expected to connect)
                   BUFFER_SIZE,
                   BUFFER_SIZE,
                   0,
                   IntPtr.Zero))
            {
                //failed to create named pipe
                if (clientPipeHandle.IsInvalid)
                    return false;

                var info = new ProcessStartInfo
                           	{
                           		UseShellExecute = true,
                           		WorkingDirectory = Environment.CurrentDirectory,
                           		FileName = _updaterPath,
                           		Arguments = string.Format(@"""{0}"" {1} {2}", _syncProcessName,
                                _updaterShowConsole ? "-showConsole" : "",
                                _updaterDoLogging ? "-log" : ""),
                           	};
                if (!_updaterShowConsole)
                {
                    info.WindowStyle = ProcessWindowStyle.Hidden;
                    info.CreateNoWindow = true;
                }

				//If we can't write to the destination folder, then lets try elevating priviledges.
				if (!Utils.PermissionsCheck.HaveWritePermissionsForFolder(Environment.CurrentDirectory) || _runPrivileged) { info.Verb = "runas"; }
            	
				try
                {
                    Process.Start(info);
                }
                catch (Win32Exception)
                {
                    // Person denied UAC escallation
                    return false;
                }

                while (true)
                {
                    var success = 0;
                    try
                    {
                        success = ConnectNamedPipe(
                           clientPipeHandle,
                           IntPtr.Zero);
                    }
                    catch { }

                    //failed to connect client pipe
                    if (success == 0)
                        break;

                    //client connection successfull
                    using (var fStream = new FileStream(clientPipeHandle, FileAccess.Write, (int)BUFFER_SIZE, true))
                    {
                        new BinaryFormatter().Serialize(fStream, _updateData);
                        fStream.Close();
                    }
                }
            }

            return true;
        }

        private void ExtractUpdaterFromResource()
        {
            //store the updater temporarily in the designated folder            
            using (var writer = new BinaryWriter(File.Open(_updaterPath, FileMode.Create)))
                writer.Write(Resources.updater);
        }
    }
}