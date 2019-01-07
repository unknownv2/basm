﻿using System;
using System.Collections.Immutable;
using Basm.Core.CodeAnalysis;
using Basm.Core.CodeAnalysis.Scripting;

namespace Basm.Scripts.CheatEngine.Commands
{
    public sealed class UnregisterSymbolCommand : ICommand
    {
        public EvaluationResult Evaluate(ScriptState state, ImmutableList<object> arguments)
        {
            throw new NotImplementedException();
        }
    }
}
