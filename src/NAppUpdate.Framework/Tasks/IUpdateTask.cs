using System.Collections.Generic;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Conditions;

namespace NAppUpdate.Framework.Tasks
{
    public interface IUpdateTask : INauFieldsHolder
    {
        string Description { get; set; }
        BooleanCondition UpdateConditions { get; set; }

        /// <summary>
        /// Do all work, especially if it is lengthy, required to prepare the update task, except from
        /// the final trivial operations required to actually perform the update.
        /// </summary>
        /// <param name="source">An update source object, in case more data is required</param>
        /// <returns>True if successful, false otherwise</returns>
        bool Prepare(Sources.IUpdateSource source);
        
        /// <summary>
        /// Execute the update. After all preparation is done, this call should be quite a short one
        /// to perform.
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        bool Execute();
    	IEnumerator<KeyValuePair<string, object>> GetColdUpdates();
        bool MustRunPrivileged();

        /// <summary>
        /// Rollback the update performed by this task.
        /// </summary>
        /// <returns>True if successful, false otherwise</returns>
        bool Rollback();
    }
}
