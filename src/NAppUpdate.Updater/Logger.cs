using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NAppUpdate.Updater
{
    public class Logger
    {
        private string _filename;
        public Logger(string filename)
        {
            _filename = filename;
        }

        public void Log(string message)
        {
            using (StreamWriter w = File.AppendText(_filename)) 
            {
                w.WriteLine("{0,-25}: {1}", 
                    DateTime.Now.ToShortDateString() + " " + 
                    DateTime.Now.ToString("HH:mm:ss.fff"), 
                    message);
                w.Flush();
            }
        }

        public void Log(string message, params object[] args)
        {
            Log(string.Format(message, args));
        }

    }
}
