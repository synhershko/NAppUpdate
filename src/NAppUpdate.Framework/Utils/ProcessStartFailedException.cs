using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NAppUpdate.Framework.Utils
{
	/// <summary>
	/// Thrown if the Process.Start() call returns null, e.g. the updater process fails to start at all.
	/// </summary>
	internal class ProcessStartFailedException : Exception
	{
		public ProcessStartFailedException(string message) : base(message) { }
	}

}
