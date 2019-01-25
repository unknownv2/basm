using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.Parser
{
    public class IntelCompilationUnitSyntax : CompilationUnitSyntax
    {
        public IntelCompilationUnitSyntax(InstructionStatementSyntax instructionStatement, SyntaxToken endOfFileToken)
        {
            InstructionStatement = instructionStatement;
            EndOfFileToken = endOfFileToken;
        }

        public SyntaxToken EndOfFileToken { get; }
        public InstructionStatementSyntax InstructionStatement { get; }
    }
}
