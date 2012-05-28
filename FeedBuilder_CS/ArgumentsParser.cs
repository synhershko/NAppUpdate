using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
namespace FeedBuilder
{

	public class ArgumentsParser
	{
		public bool HasArgs { get; set; }
		public string FileName { get; set; }
		public bool ShowGui { get; set; }
		public bool Build { get; set; }
		public bool OpenOutputsFolder { get; set; }

		public ArgumentsParser(string[] args)
		{
            foreach (string thisArg in args)
            {
				if (thisArg.ToLower() == Application.ExecutablePath.ToLower()
                    || thisArg.ToLower().Contains(".vshost.exe"))
					continue;

				string arg = CleanArg(thisArg);
				if (arg == "build") {
					this.Build = true;
					this.HasArgs = true;
				} else if (arg == "showgui") {
					this.ShowGui = true;
					this.HasArgs = true;
				} else if (arg == "openoutputs") {
					this.OpenOutputsFolder = true;
					this.HasArgs = true;
				} else if (File.Exists(arg)) {
					this.FileName = arg;
					this.HasArgs = true;
				} else {
					Console.WriteLine("Unrecognized arg '{0}'", arg);
				}

			}
		}

		private string CleanArg(string arg)
		{
			const string pattern1 = "^(.*)([=,:](true|0))";
			arg = arg.ToLower();
			if (arg.StartsWith("-") || arg.StartsWith("/")) {
				arg = arg.Substring(1);
			}
			Regex r = new Regex(pattern1);
			arg = r.Replace(arg, "{$1}");
			return arg;
		}
	}
}
