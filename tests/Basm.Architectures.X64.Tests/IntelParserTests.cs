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
            var syntaxTree = SyntaxTree.Parse("NOP");
            var root = syntaxTree.Root;
            var instruction = root.Instruction;
            Assert.Empty(instruction.Operands);
        }

    }
}
