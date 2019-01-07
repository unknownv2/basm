using System;
using System.Collections.Immutable;
using Basm.Core.CodeAnalysis;
using Basm.Core.CodeAnalysis.Scripting;

namespace Basm.Scripts.CheatEngine.Commands
{
    /// <summary>
    /// Interface for a Cheat Engine AutoAssembler command,
    /// such as alloc or dealloc.
    /// </summary>
    public interface ICommand : IDirective
    {

    }
}
