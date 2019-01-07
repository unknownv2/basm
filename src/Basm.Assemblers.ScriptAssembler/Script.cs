using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Basm.Core.CodeAnalysis;

namespace Basm.Assemblers.ScriptAssembler
{
    public sealed class Script : IScript
    {
        public VariableSymbols Variables { get; }
        public ImmutableList<IScript> Sections { get; } = ImmutableList.Create<IScript>();

        public Script()
        {
            Sections.AddRange(new List<IScript>());
        }
    }
}
