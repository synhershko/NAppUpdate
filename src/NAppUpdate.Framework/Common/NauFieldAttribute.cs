using System;

namespace NAppUpdate.Framework.Common
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class NauFieldAttribute : Attribute
	{
		private readonly string _alias;
		private readonly string _description;
		private readonly bool _isRequired;

		public NauFieldAttribute(string alias, string description, bool isRequired)
		{
			this._alias = alias;
			this._description = description;
			this._isRequired = isRequired;
		}

		public string Alias
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return this._alias; }
		}

		public string Description
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return this._description; }
		}

		public bool IsRequired
		{
			[System.Diagnostics.DebuggerStepThrough]
			get { return this._isRequired; }
		}
	}
}
