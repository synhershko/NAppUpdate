using System;
using System.Collections.Generic;
using System.Text;

namespace NAppUpdate.Framework.Logger
{
    public interface ILogger
    {
        void Debug(object message);
        void Error(object message);
        void Info(object message);
        void Warn(object message);
    }
}
