using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Basm.Core.CodeAnalysis.Scripting;

namespace Basm.Scripting
{
    public interface IScript : IScriptState
    {
        ImmutableList<IScript> Sections { get; }
    }
}
