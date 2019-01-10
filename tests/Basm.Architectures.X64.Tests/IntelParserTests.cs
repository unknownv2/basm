using Basm.Architectures.X64.Parser.Intel;
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

        [Fact]
        public void ShouldParseInstructionWithOneOperand()
        {
            const string mnemonic = "push";
            const string operand1 = "rax";
            string instructionText = $"{mnemonic} {operand1}";
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);
            Assert.Equal(operand1, ((RegisterNameExpressionSyntax)instruction.Operands[0]).IdentifierToken.Text);
        }

        [Fact]
        public void ShouldParseInstructionWithOne32BitOperand()
        {
            const string mnemonic = "pop";
            const string operand1 = "ecx";
            string instructionText = $"{mnemonic} {operand1}";
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);
            Assert.Equal(operand1, ((RegisterNameExpressionSyntax)instruction.Operands[0]).IdentifierToken.Text);
        }
    }
}
