using System;
using Basm.Architectures.X64.Parser.Intel;
using Basm.Core.CodeAnalysis.Syntax;
using Xunit;

namespace Basm.Architectures.X64.Tests
{
    public class IntelParserTests
    {
        [Fact]
        public void ShouldParseInstructionWithZeroOperands()
        {
            const string instructionText = "NOP";

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(instructionText, instruction.InstructionToken.Text);
            Assert.Empty(instruction.Operands);
        }

    }
}
