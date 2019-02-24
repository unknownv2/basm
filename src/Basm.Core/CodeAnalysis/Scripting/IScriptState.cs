using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Scripting
{
    public interface IScriptState
    {
        VariableSymbols Symbols { get; }
    }
}
