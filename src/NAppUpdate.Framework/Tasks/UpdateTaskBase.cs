using System;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Conditions;
using NAppUpdate.Framework.Sources;

namespace NAppUpdate.Framework.Tasks
{
	[Serializable]
	public abstract class UpdateTaskBase : IUpdateTask
	{
		public string Description { get; set; }
		public TaskExecutionStatus ExecutionStatus { get; set; }

		[NonSerialized]
		private BooleanCondition _updateConditions;
		public BooleanCondition UpdateConditions
		{
			get { return _updateConditions ?? (_updateConditions = new BooleanCondition()); }
			set { _updateConditions = value; }
		}

		[field: NonSerialized]
		public event ReportProgressDelegate ProgressDelegate;

		public virtual void OnProgress(UpdateProgressInfo pi)
		{
			if (ProgressDelegate != null)
				ProgressDelegate(pi);
		}

		public abstract void Prepare(IUpdateSource source);
		public abstract TaskExecutionStatus Execute(bool coldRun);
		public abstract bool Rollback();

		public override string ToString()
		{
			return Description;
		}
	}
}
