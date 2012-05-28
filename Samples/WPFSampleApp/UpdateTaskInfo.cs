using System;
using System.Collections.Generic;

namespace NAppUpdate.SampleApp
{
    public class UpdateTaskInfo
    {
        // TaskDescription?
        public string FileDescription { get; set; }
        public string FileName { get; set; }
        public string FileVersion { get; set; }
        public string FileSize { get; set; }
        public string FileDate { get; set; }
        public string FileChecksum { get; set; }
        //public long? FileSize { get; set; }
        //public DateTime? FileDate { get; set; }
    }
}
