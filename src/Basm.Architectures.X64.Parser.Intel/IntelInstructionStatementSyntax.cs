using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.X64.Parser.Intel
{
    public sealed class IntelInstructionStatementSyntax : InstructionStatementSyntax
    {
        public IntelInstructionStatementSyntax(IntelSyntaxToken instructionToken, ImmutableArray<ExpressionSyntax> operands) 
            : base(instructionToken, operands)
        {

        }
    }
}
