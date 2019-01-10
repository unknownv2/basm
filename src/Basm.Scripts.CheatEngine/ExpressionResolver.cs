using System;
using System.Collections.Generic;
using System.Text;
using Basm.Assemblers.ScriptAssembler;

namespace Basm.Scripts.CheatEngine
{
    public class CheatEngineExpressionResolver : SymbolResolver
    {
        public override string ResolveSymbol(object symbol)
        {
            return (string) symbol;
        }
    }
}
