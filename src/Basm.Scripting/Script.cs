using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Basm.Core.CodeAnalysis;

namespace Basm.Scripting
{
    public sealed class Script : IScript
    {
        public VariableSymbols Symbols { get; }
        public ImmutableList<IScript> Sections { get; } = ImmutableList.Create<IScript>();

        public Script()
        {
        }
    }
}
