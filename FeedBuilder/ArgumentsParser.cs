using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace FeedBuilder
{
	public class ArgumentsParser
	{
		public bool HasArgs { get; set; }
		public string FileName { get; set; }
		public bool ShowGui { get; set; }
		public bool Build { get; set; }
		public bool OpenOutputsFolder { get; set; }

		public ArgumentsParser(IEnumerable<string> args)
		{
			foreach (string thisArg in args) {
				if (thisArg.ToLower() == Application.ExecutablePath.ToLower() || thisArg.ToLower().Contains(".vshost.exe")) continue;

				string arg = CleanArg(thisArg);
				if (arg == "build") {
					Build = true;
					HasArgs = true;
				} else if (arg == "showgui") {
					ShowGui = true;
					HasArgs = true;
				} else if (arg == "openoutputs") {
					OpenOutputsFolder = true;
					HasArgs = true;
				} else if (IsValidFileName(thisArg)) {
					// keep the same character casing as we were originally provided
					FileName = thisArg;
					HasArgs = true;
				} else Console.WriteLine("Unrecognized arg '{0}'", arg);
			}
		}

		// this merely checks whether the parent folder exists and if it does, 
		// we say the filename is valid
		private bool IsValidFileName(string filename)
		{
			if (File.Exists(filename)) return true;
			try {
				// the URI test... filter out things that aren't even trying to look like filenames
// ReSharper disable UnusedVariable
				Uri u = new Uri(filename);
// ReSharper restore UnusedVariable
				// see if the arg's parent folder exists
				var d = Directory.GetParent(filename);
				if (d.Exists) return true;
			} catch {}
			return false;
		}

		private string CleanArg(string arg)
		{
			const string pattern1 = "^(.*)([=,:](true|0))";
			arg = arg.ToLower();
			if (arg.StartsWith("-") || arg.StartsWith("/")) arg = arg.Substring(1);
			Regex r = new Regex(pattern1);
			arg = r.Replace(arg, "{$1}");
			return arg;
		}
	}
}