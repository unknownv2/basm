﻿using System;
using System.Collections.Generic;
using System.Text;
using Basm.Scripting;

namespace Basm.Scripts.CheatEngine
{
    public class CheatEngineSymbolResolver : SymbolResolver
    {
        public override string ResolveSymbol(object symbol)
        {
            return (string) symbol;
        }
    }
}
