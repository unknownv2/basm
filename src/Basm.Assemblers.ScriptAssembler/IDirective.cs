using System;
using System.Collections.Immutable;
using Basm.Core.CodeAnalysis;
using Basm.Core.CodeAnalysis.Scripting;

namespace Basm.Assemblers.ScriptAssembler
{
    /// <summary>
    /// A message defined by the assembler that executes a pre-defined function.
    /// </summary>
    public interface IDirective
    {
        /// <summary>
        /// Evaluate the command and return the result.
        /// For example, the alloc command would take a size argument
        /// and return a memory address.
        /// </summary>
        /// <param name="state">The global script state, providing access to properties
        /// such as script symbols or variables.</param>
        /// <param name="arguments">The arguments given to the command being evaluated.</param>
        /// <returns>The result of executing the script command.</returns>
        EvaluationResult Evaluate(ScriptState state, ImmutableList<object> arguments);
    }
}
