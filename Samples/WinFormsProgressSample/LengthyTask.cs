using System.Threading;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Sources;
using NAppUpdate.Framework.Tasks;

namespace WinFormsProgressSample
{
	public class LengthyTask : UpdateTaskBase
	{
		public override void Prepare(IUpdateSource source)
		{
			for (int i = 0; i < 50; i++)
			{
				Thread.Sleep(100);
				OnProgress(new UpdateProgressInfo
								{
									Message = "Doing some work, cycle " + i,
									Percentage = i * 2,
									StillWorking = true
								});
			}

			OnProgress(new UpdateProgressInfo
							{
								Message = "Finished preperations",
								Percentage = 100,
								StillWorking = false,
							});
		}

		public override TaskExecutionStatus Execute(bool coldRun)
		{
			return coldRun ? TaskExecutionStatus.Successful : TaskExecutionStatus.RequiresAppRestart;
		}

		public override bool Rollback()
		{
			return true;
		}
	}
}
