using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.X64.Parser.Intel
{
    public class IntelCompilationUnitSyntax : CompilationUnitSyntax
    {
        public IntelCompilationUnitSyntax(InstructionSyntax instruction, SyntaxToken endOfFileToken)
        {
            Instruction = instruction;
        }

        public InstructionSyntax Instruction;
    }
}
