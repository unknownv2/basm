using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace Basm.Core.CodeAnalysis.Syntax
{
    public abstract class InstructionStatementSyntax : StatementSyntax
    {
        protected InstructionStatementSyntax(SyntaxToken instructionToken, ImmutableArray<ExpressionSyntax> operands)
        {
            InstructionToken = instructionToken;
            Operands = operands;
        }

        public SyntaxToken InstructionToken { get; }
        public ImmutableArray<ExpressionSyntax> Operands { get; set; }
    }
}
