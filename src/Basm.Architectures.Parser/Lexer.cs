using System;
using System.Collections.Generic;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.Parser
{
    public interface IAssemblyLexer : ILexer
    {
        HashSet<string> InstructionSet { get; }
        HashSet<string> RegisterSet { get; }
    }
}
