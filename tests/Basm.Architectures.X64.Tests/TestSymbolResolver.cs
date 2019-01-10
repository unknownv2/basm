﻿using System;
using System.Collections.Generic;
using System.Text;
using Basm.Scripting;

namespace Basm.Architectures.X64.Tests
{
    public class TestSymbolResolver : SymbolResolver
    {
        public override string ResolveSymbol(object symbol)
        {
            return (string)symbol;
        }
    }
}