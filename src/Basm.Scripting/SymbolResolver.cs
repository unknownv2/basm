using System;

namespace Basm.Scripting
{
    public abstract class SymbolResolver
    {
        public abstract string ResolveSymbol(object symbol);
    }
}
