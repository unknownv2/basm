﻿using System;
using System.Collections.Immutable;
using Basm.Core.CodeAnalysis;
using Basm.Core.CodeAnalysis.Scripting;

namespace Basm.Scripts.CheatEngine.Commands
{
    public sealed class AllocCommand : ICommand
    {
        public EvaluationResult Evaluate(ScriptState state, ImmutableArray<object> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
