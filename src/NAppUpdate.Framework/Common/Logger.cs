using System;
using System.Collections.Generic;
using System.IO;

namespace NAppUpdate.Framework.Common
{
	public class Logger
	{
		[Serializable]
		public enum SeverityLevel
		{
			Debug,
			Warning,
			Error
		}

		[Serializable]
		public class LogItem
		{
			public DateTime Timestamp { get; set; }
			public string Message { get; set; }
			public Exception Exception { get; set; }
			public SeverityLevel Severity { get; set; }

			public override string ToString()
			{
				if (Exception == null)
					return string.Format("{0,-25}\t{1}\t{2}",
										 Timestamp.ToShortDateString() + " " + Timestamp.ToString("HH:mm:ss.fff"),
										 Severity,
										 Message);

				return string.Format("{0,-25}\t{1}\t{2}{3}{4}",
									 Timestamp.ToShortDateString() + " " + Timestamp.ToString("HH:mm:ss.fff"),
									 Severity,
									 Message, Environment.NewLine, Exception);
			}
		}

		public List<LogItem> LogItems { get; private set; }

		public Logger()
		{
			LogItems = new List<LogItem>();
		}

		public Logger(List<LogItem> logItems)
		{
			LogItems = logItems ?? new List<LogItem>();
		}

		public void Log(string message, params object[] args)
		{
			Log(SeverityLevel.Debug, message, args);
		}

		public void Log(SeverityLevel severity, string message, params object[] args)
		{
			lock (LogItems)
				LogItems.Add(new LogItem
								{
									Message = string.Format(message, args),
									Severity = severity,
									Timestamp = DateTime.Now,
								});
		}

		public void Log(Exception exception)
		{
			Log(exception, string.Empty);
		}

		public void Log(Exception exception, string message)
		{
			lock (LogItems)
				LogItems.Add(new LogItem
								{
									Message = message,
									Severity = SeverityLevel.Error,
									Timestamp = DateTime.Now,
									Exception = exception,
								});
		}

		public void Dump()
		{
			Dump(null);
		}

		public void Dump(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				var workingDir = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
				filePath = Path.Combine(workingDir ?? string.Empty, @"NauUpdate.log");
			}

			if (File.Exists(filePath))
			{
				var oldBackupFile = SelectBackupLogFileName(filePath);
				if (oldBackupFile != null)
					File.Move(filePath, oldBackupFile);
			}

			lock (LogItems)
			{
				using (StreamWriter w = File.CreateText(filePath))
					foreach (var logItem in LogItems)
					{
						w.WriteLine(logItem.ToString());
					}
			}
		}

		private string SelectBackupLogFileName(string filePath)
		{
			var directory = Path.GetDirectoryName(filePath);
			if (directory == null)
				directory = ".";
			for (var i = 0; i < 10; i++)
			{
				filePath = Path.Combine(directory, "NauUpdate." + i + ".log");
				if(File.Exists(filePath))
					continue;
				return filePath;
			}
			return null;
		}
	}
}
