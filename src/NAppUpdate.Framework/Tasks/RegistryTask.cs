using System;
using System.Collections.Generic;
using System.Text;
using NAppUpdate.Framework.Common;
using Microsoft.Win32;

namespace NAppUpdate.Framework.Tasks
{
    [UpdateTaskAlias("registryUpdate")]
    public class RegistryTask : IUpdateTask
    {
        public RegistryTask()
        {
            UpdateConditions = new Conditions.BooleanCondition();
        }

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
        private object originalValue;

        #region IUpdateTask Members

        public string Description { get; set; }

        public Conditions.BooleanCondition UpdateConditions { get; set; }

        public bool Prepare(Sources.IUpdateSource source)
        {
            // No preparation required
            return true;
        }

        public bool Execute()
        {
            if (String.IsNullOrEmpty(KeyName) || String.IsNullOrEmpty(KeyValueName))
                return true;

            try
            {
                // Get the current value and store in case we need to rollback
                // This is also used to prematurely detect incorrect key and value paths
                originalValue = Registry.GetValue(KeyName, KeyValueName, null);
            }
            catch { return false; }

            try
            {
                Registry.SetValue(KeyName, KeyValueName, ValueToSet, ValueKind);
            }
            catch { return false; }

            return true;
        }

        public IEnumerator<KeyValuePair<string, object>> GetColdUpdates()
        {
            yield break;
        }

        public bool Rollback()
        {
            try
            {
                Registry.SetValue(KeyName, KeyValueName, originalValue);
            }
            catch { return false; }
            return true;
        }

        public bool MustRunPrivileged() {
            // This may be changed if required, I don't use this task...
            return false;
        }

        #endregion
    }
}
