using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.X64.Parser.Intel
{
    public sealed class InstructionSyntax : SyntaxNode
    {
        public InstructionSyntax(IntelSyntaxToken instructionToken, ImmutableArray<ExpressionSyntax> operands)
        {
            InstructionToken = instructionToken;
            Operands = operands;
        }

        public IntelSyntaxToken InstructionToken { get; }
        public ImmutableArray<ExpressionSyntax> Operands { get; set; }
    }
}
