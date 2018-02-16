using System;
using System.Collections.Generic;

namespace NAppUpdate.Framework.Conditions
{
	[Serializable]
	public sealed class BooleanCondition : IUpdateCondition
	{
		#region Condition types

		[Flags]
		public enum ConditionType : byte
		{
			AND = 1,
			OR = 2,
			NOT = 4,
		}

		public static ConditionType ConditionTypeFromString(string type)
		{
			if (!string.IsNullOrEmpty(type))
			{
				switch (type.ToLower())
				{
					case "and":
						return ConditionType.AND;
					case "or":
						return ConditionType.OR;
					case "not":
					case "and-not":
						return ConditionType.AND | ConditionType.NOT;
					case "or-not":
						return ConditionType.OR | ConditionType.NOT;
				}
			}

			// Make AND the default condition type
			return ConditionType.AND;
		}
		#endregion

		public class ConditionItem
		{
			public ConditionItem(IUpdateCondition cnd, ConditionType typ)
			{
				this.Condition = cnd;
				this.ConditionType = typ;
			}

			internal bool HasConditionType(ConditionType type)
			{
				return (ConditionType & type) > 0;
			}

			public IUpdateCondition Condition { get; private set; }
			public ConditionType ConditionType { get; private set; }
		}

		public BooleanCondition()
		{
			Attributes = new Dictionary<string, string>();
		}

		public LinkedList<ConditionItem> ChildConditions { get; private set; }
		public int ChildConditionsCount { get { if (ChildConditions != null) return ChildConditions.Count; return 0; } }

		public void AddCondition(IUpdateCondition cnd)
		{
			AddCondition(cnd, ConditionType.AND);
		}

		public void AddCondition(IUpdateCondition cnd, ConditionType type)
		{
			if (ChildConditions == null) ChildConditions = new LinkedList<ConditionItem>();
			ChildConditions.AddLast(new ConditionItem(cnd, type));
		}

		public IUpdateCondition Degrade()
		{
			if (ChildConditionsCount == 1 && (ChildConditions.First.Value.ConditionType & ConditionType.NOT) == 0)
				return ChildConditions.First.Value.Condition;

			return this;
		}

		public IDictionary<string, string> Attributes { get; private set; }

		public bool IsMet(Tasks.IUpdateTask task)
		{
			if (ChildConditions == null)
				return true;

			// perform the update if Passed == true
			// otherwise, do not perform the update
			bool passed = true, firstRun = true;
			foreach (ConditionItem item in ChildConditions)
			{
				// If after the first iteration, accept as fulfilled if we are at an OR clause and the conditions
				// before this checked OK (i.e. update needed)
				if (!firstRun)
				{
					if (passed && item.HasConditionType(ConditionType.OR))
						return true;
				}
				else { firstRun = false; }

				// Skip all ANDed conditions if some of them failed, until we consume all the conditions
				// or we hit an OR'ed one
				if (!passed)
				{
					if (item.HasConditionType(ConditionType.OR))
					{
						var checkResult = item.Condition.IsMet(task);
						passed = item.HasConditionType(ConditionType.NOT) ? !checkResult : checkResult;
					}
				}
				else
				{
					var checkResult = item.Condition.IsMet(task);
					passed = item.HasConditionType(ConditionType.NOT) ? !checkResult : checkResult;
				}
			}

			return passed;
		}
	}
}
