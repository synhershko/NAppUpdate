using System;
using NAppUpdate.Framework.Common;
using Microsoft.Win32;

namespace NAppUpdate.Framework.Tasks
{
	[Serializable]
	[UpdateTaskAlias("registryUpdate")]
	public class RegistryTask : UpdateTaskBase
	{
		[NauField("keyName", "The full path to the registry key", true)]
		public string KeyName { get; set; }

		[NauField("keyValue", "The value name to set", true)]
		public string KeyValueName { get; set; }

		[NauField("valueKind",
			"Value type; choose one and then set only one value field (leave all blank to remove the key)"
			, true)]
		public RegistryValueKind ValueKind { get; set; }

		[NauField("value", "A String value to set", false)]
		public string StringValue { get; set; }

		[NauField("value", "A DWord value to set", false)]
		public Int32? DWordValue { get; set; }

		[NauField("value", "A QWord value to set", false)]
		public Int64? QWordValue { get; set; }

		// Get the first non-null value
		protected object ValueToSet
		{
			get
			{
				if (StringValue != null)
					return StringValue;
				if (DWordValue != null)
					return DWordValue;
				if (QWordValue != null)
					return QWordValue;
				return null;
			}
		}
		private object _originalValue;

		public override void Prepare(Sources.IUpdateSource source)
		{
			// No preparation required
		}

		public override TaskExecutionStatus Execute(bool coldRun /* unused */)
		{
			if (String.IsNullOrEmpty(KeyName) || String.IsNullOrEmpty(KeyValueName))
				return ExecutionStatus = TaskExecutionStatus.Successful;


			// Get the current value and store in case we need to rollback
			// This is also used to prematurely detect incorrect key and value paths
			// Any exception thrown in this stage would just keep this task in Pending state
			_originalValue = Registry.GetValue(KeyName, KeyValueName, null);

			try
			{
				Registry.SetValue(KeyName, KeyValueName, ValueToSet, ValueKind);
			}
			catch (Exception ex)
			{
				ExecutionStatus = TaskExecutionStatus.Failed;
				throw new UpdateProcessFailedException("Error while trying to set the new registry key value", ex);
			}

			return ExecutionStatus = TaskExecutionStatus.Successful;
		}

		public override bool Rollback()
		{
			Registry.SetValue(KeyName, KeyValueName, _originalValue);
			return true;
		}

		public override string ToString()
		{
			var ret = base.ToString();
			return string.IsNullOrWhiteSpace(ret) ? KeyName + @"\" + KeyValueName : ret;
		}
	}
}
