using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NAppUpdate.Framework;
using NAppUpdate.Framework.Tasks;
using NAppUpdate.Framework.Conditions;

namespace NAppUpdate.SampleApp
{
    public class UpdateTaskHelper
    {
        private UpdateManager _manager;

        public IList<UpdateTaskInfo> TaskListInfo { get; private set; }
        public string CurrentVersion { get; private set; }
        public string UpdateDescription { get; private set; }
        public string UpdateFileName { get; private set; }
        public string UpdateVersion { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public long UpdateFileSize { get; private set; }

        public UpdateTaskHelper()
        {
            _manager = UpdateManager.Instance;
            this.GetUpdateTaskInfo();
            this.CurrentVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public IList<UpdateTaskInfo> GetUpdateTaskInfo()
        {
            var taskListInfo = new List<UpdateTaskInfo>();
            foreach (IUpdateTask task in _manager.UpdatesToApply)
            {
                var fileTask = task as FileUpdateTask;
                if (fileTask == null) continue;

                var taskInfo = new UpdateTaskInfo();
                taskInfo.FileName = fileTask.LocalPath;
                taskInfo.FileDescription = fileTask.Description;

                string appExe = System.Reflection.Assembly.GetExecutingAssembly().Location;
                bool isAppExe = fileTask.LocalPath == null ? true : appExe == new FileInfo(fileTask.LocalPath).FullName;
                if (isAppExe)
                {
                    this.UpdateFileName = taskInfo.FileName;
                    this.UpdateDescription = taskInfo.FileDescription;
                }

                foreach (BooleanCondition.ConditionItem condition in task.UpdateConditions.ChildConditions)
                {
                    var fileSize = condition._Condition as FileSizeCondition;
                    if (fileSize != null)
                    {
                        taskInfo.FileSize = FormatCondition(condition, fileSize.FileSize);
                        this.UpdateFileSize += fileSize.FileSize;
                        continue;
                    }
                    var fileVersion = condition._Condition as FileVersionCondition;
                    if (fileVersion != null)
                    {
                        taskInfo.FileVersion = FormatCondition(condition, fileVersion.Version);
                        if (isAppExe) this.UpdateVersion = fileVersion.Version.ToString();
                        continue;
                    }
                    var fileDate = condition._Condition as FileDateCondition;
                    if (fileDate != null)
                    {
                        taskInfo.FileDate = FormatCondition(condition, fileDate.Timestamp);
                        if (isAppExe) this.UpdateDate = fileDate.Timestamp == DateTime.MinValue ? (DateTime?)null : fileDate.Timestamp;
                        continue;
                    }

                    var fileChecksum = condition._Condition as FileChecksumCondition;
                    if (fileChecksum != null)
                    {
                        taskInfo.FileChecksum = FormatCondition(condition, fileChecksum.Checksum);
                    }
                }
                taskListInfo.Add(taskInfo);
            }
            this.TaskListInfo = taskListInfo;
            return taskListInfo;
        }

        private string FormatCondition(BooleanCondition.ConditionItem condition, object value)
        {
            // Helps with debugging conditiions
            //return string.Format("{0} {1}", condition, value);

            // Better for production
            return string.Format("{0}", value);
        }

    }
}
