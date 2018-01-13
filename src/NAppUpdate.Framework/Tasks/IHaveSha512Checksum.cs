using System;
using System.Collections.Generic;
using System.Text;

namespace NAppUpdate.Framework.Tasks
{
    public interface IHaveSha512Checksum
    {
        string Sha512Checksum { get; }
    }
}
