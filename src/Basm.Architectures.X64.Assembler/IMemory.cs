using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Architectures.X64.Assembler
{
    public interface IMemory
    {
        ulong Address { get; }
    }
}
