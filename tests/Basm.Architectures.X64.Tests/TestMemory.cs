﻿using System;
using System.Collections.Generic;
using System.Text;
using Basm.Architectures.X64.Assembler;

namespace Basm.Architectures.X64.Tests
{
    internal class TestMemory : IMemory
    {
        public ulong Address { get; set; }
    }
}
