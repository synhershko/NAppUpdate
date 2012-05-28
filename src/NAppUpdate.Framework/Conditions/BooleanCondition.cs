using System;
using System.Collections.Generic;
using System.Text;

namespace NAppUpdate.Framework.Conditions
{
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
                this._Condition = cnd;
                this._ConditionType = typ;
            }

            public IUpdateCondition _Condition;
            public ConditionType _ConditionType;
        }

        public BooleanCondition()
        {
            Attributes = new Dictionary<string, string>();
        }

        public LinkedList<ConditionItem> ChildConditions { get; set; }
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
            if (ChildConditionsCount == 1 && (ChildConditions.First.Value._ConditionType & ConditionType.NOT) == 0)
                return ChildConditions.First.Value._Condition;

            return this;
        }

        #region IUpdateCondition Members

        public IDictionary<string, string> Attributes { get; private set; }

        public bool IsMet(NAppUpdate.Framework.Tasks.IUpdateTask task)
        {
            if (ChildConditions == null)
                return true;

            // perform the update if Passed == true
            // otherwise, do not perform the update
            bool Passed = true, firstRun = true;
            foreach (ConditionItem item in ChildConditions)
            {
                // If after the first iteration, accept as fulfilled if we are at an OR clause and the conditions
                // before this checked OK (i.e. update needed)
                if (!firstRun)
                {
                    if (Passed && (item._ConditionType & ConditionType.OR) > 0)
                        return true;
                }
                else { firstRun = false; }

                // Skip all ANDed conditions if some of them failed, until we consume all the conditions
                // or we hit an OR'ed one
                if (!Passed)
                {
                    if ((item._ConditionType & ConditionType.OR) > 0)
                    {
                        bool checkResult = item._Condition.IsMet(task);
                        Passed = (item._ConditionType & ConditionType.NOT) > 0 ? !checkResult : checkResult;
                    }
                }
                else
                {
                    bool checkResult = item._Condition.IsMet(task);
                    Passed = (item._ConditionType & ConditionType.NOT) > 0 ? !checkResult : checkResult;
                }
            }

            return Passed;
        }

        #endregion
    }
}
