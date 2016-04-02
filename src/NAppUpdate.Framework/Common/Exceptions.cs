using System;
using System.Runtime.Serialization;

namespace NAppUpdate.Framework
{
	[Serializable]
	public class NAppUpdateException : Exception
	{
		public NAppUpdateException() : base() { }
		public NAppUpdateException(string message) : base(message) { }
		public NAppUpdateException(string message, Exception ex) : base(message, ex) { }
		public NAppUpdateException(SerializationInfo info, StreamingContext context) : base(info, context) { }

		public override string ToString()
		{
			var ret = Message;
			if (!string.IsNullOrEmpty(ret)) ret += Environment.NewLine;
			if (InnerException != null)
				ret += InnerException;
			else
				ret += StackTrace;
			return ret;
		}
	}

	[Serializable]
	public class UpdateProcessFailedException : NAppUpdateException
	{
		public UpdateProcessFailedException() : base() { }
		public UpdateProcessFailedException(string message) : base(message) { }
		public UpdateProcessFailedException(string message, Exception ex) : base(message, ex) { }
		public UpdateProcessFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class FeedReaderException : NAppUpdateException
	{
		public FeedReaderException() : base() { }
		public FeedReaderException(string message) : base(message) { }
		public FeedReaderException(string message, Exception ex) : base(message, ex) { }
		public FeedReaderException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}

	[Serializable]
	public class UserAbortException : NAppUpdateException
	{
		public UserAbortException()
			: base("User abort")
		{
		}
	}
}
