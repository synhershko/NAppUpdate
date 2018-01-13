using System;
using System.Collections.Generic;
using System.Text;

namespace NAppUpdate.Framework.Tasks
{
    public interface IHaveSha256Checksum
    {
        string Sha256Checksum { get; }
    }
}
