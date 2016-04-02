using NAppUpdate.Framework.Common;

namespace NAppUpdate.Framework.Conditions
{
	public interface IUpdateCondition : INauFieldsHolder
	{
		/// <summary>
		/// Checks if the IUpdateCondition is met. An IUpdateTask is only going to be performed if all UpdateConditions
		/// registered with it are met.
		/// </summary>
		/// <param name="task">The IUpdateTask in question, used to provide the condition logic with more info on the actual task.</param>
		/// <returns>true if the condition is fulfiled and the update task should be executed, false otherwise</returns>
		bool IsMet(Tasks.IUpdateTask task);
	}
}
