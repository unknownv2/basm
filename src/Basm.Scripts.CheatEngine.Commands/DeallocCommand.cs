using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Basm.Core.CodeAnalysis;
using Basm.Core.CodeAnalysis.Scripting;

namespace Basm.Scripts.CheatEngine.Commands
{
    public sealed class DeallocCommand : ICommand
    {
        public EvaluationResult Evaluate(ScriptState state, ImmutableList<object> arguments)
        {
            throw new NotImplementedException();
        }
    }
}
