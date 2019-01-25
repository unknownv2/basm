using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Scripting;

namespace Basm.Architectures.X86.Tests
{
    public class TestSymbolResolver : SymbolResolver
    {
        public override string ResolveSymbol(object symbol)
        {
            return (string) symbol;
        }
    }
}
