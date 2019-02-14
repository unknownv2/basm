using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.Memory;

namespace Basm.Architectures.X86.Tests
{
    internal class TestMemory : IMemory
    {
        public ulong Address { get; set; }
    }
}
