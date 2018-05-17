using System;
using NAppUpdate.Framework.Common;
using NAppUpdate.Framework.Sources;

namespace NAppUpdate.Framework.Tasks
{
    [Serializable]
    [UpdateTaskAlias("fileUpdateEx")]
    public class FileUpdateExTask : FileUpdateTask
    {

        [NauField("index", "Task Index", true)]
        public int Index { get; set; }
        [NauField("count", "Task Count", true)]
        public int Count { get; set; }
        public override void Prepare(IUpdateSource source)
        {
            //This was an assumed int, which meant we never reached 100% with an odd number of tasks
            var info = new UpdateProgressInfo
            {
                Message = "Preparing",
                Percentage = Convert.ToInt32((this.Index + 1) / Count * 100f)
            };
            OnProgress(info);
            base.Prepare(source);
        }
        public override TaskExecutionStatus Execute(bool coldRun)
        {
           var result =  base.Execute(coldRun);
            var info = new UpdateProgressInfo
            {
                Message = "Applied with result " + result.ToString(),
                Percentage = Convert.ToInt32((this.Index + 1) / Count * 100)
            };
            OnProgress(info);
            return result;
        }
    }

}
