using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Assemblers.ScriptAssembler
{
    public abstract class SymbolResolver
    {
        public abstract string ResolveSymbol(object symbol);
    }
}
