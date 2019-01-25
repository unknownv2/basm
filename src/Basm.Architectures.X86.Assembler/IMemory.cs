using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Architectures.X86.Assembler
{
    public interface IMemory
    {
        ulong Address { get; }
    }
}
