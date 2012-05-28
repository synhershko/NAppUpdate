using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
namespace NAppUpdate.Updater
{

	public class ArgumentsParser
	{
		public bool HasArgs { get; private set; }
        public string ProcessName { get; private set; }
		public bool ShowConsole { get; private set; }
		public bool Log { get; private set; }

        private static ArgumentsParser _instance;
        protected ArgumentsParser()
        {
        }

        public static ArgumentsParser Get()
        {
            if (_instance == null) _instance = new ArgumentsParser();
            return _instance;
        }

		public ArgumentsParser(string[] args)
		{
            Parse(args);
        }

        public void ParseCommandLineArgs()
        {
            Parse(Environment.GetCommandLineArgs());
        }

        public void Parse(string[] args)
        {
            for (int i=0; i < args.Length; i++)
            {
                string arg = args[i];

                // Skip any args that are our own executable (first arg should be this).
                // In Visual Studio, the arg will be the VS host starter instead of
                // actually ourself.
                if (arg.ToLower() == System.Reflection.Assembly.GetEntryAssembly().Location.ToLower()
                    || arg.ToLower().Contains(".vshost.exe"))
					continue;

				arg = CleanArg(arg);
				if (arg == "log") {
					this.Log = true;
					this.HasArgs = true;
				} else if (arg == "showconsole") {
					this.ShowConsole = true;
					this.HasArgs = true;
				} else if (this.ProcessName == null) {
                    // if we don't already have the processname set, assume this is it
                    this.ProcessName = args[i];
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
