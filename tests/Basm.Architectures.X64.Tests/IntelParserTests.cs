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

        [Fact]
        public void ShouldParseInstructionWithTwoOperands()
        {
            const string mnemonic = "xor";
            const string operand1 = "rax";
            const string operand2 = "rcx";

            string instructionText = $"{mnemonic} {operand1}, {operand2}";
            const int operandCount = 2;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);
            Assert.Equal(operand1, ((RegisterNameExpressionSyntax)instruction.Operands[0]).IdentifierToken.Text);
            Assert.Equal(operand2, ((RegisterNameExpressionSyntax)instruction.Operands[1]).IdentifierToken.Text);
        }

        [Fact]
        public void ShouldParseInstructionWithPointerOperand()
        {
            const string mnemonic = "push";
            const string operand1 = "[rax]";
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
        public void ShouldParseInstructionWithLiteralOperand()
        {
            const string mnemonic = "push";
            const string operand1 = "2";
            const int operand1Value = 2;
            string instructionText = $"{mnemonic} {operand1}";
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);
            Assert.Equal(operand1, ((LiteralExpressionSyntax)instruction.Operands[0]).LiteralToken.Text);
            Assert.Equal(operand1Value, ((LiteralExpressionSyntax)instruction.Operands[0]).Value);
        }

        [Fact]
        public void ShouldParseInstructionWithRegisterAndLiteralOperands()
        {
            const string mnemonic = "mov";
            const string operand1 = "rax";
            const string operand2 = "2";
            const int operand2Value = 2;

            string instructionText = $"{mnemonic} {operand1}, {operand2}";
            const int operandCount = 2;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);
            Assert.Equal(operand1, ((RegisterNameExpressionSyntax)instruction.Operands[0]).IdentifierToken.Text);
            Assert.Equal(operand2, ((LiteralExpressionSyntax)instruction.Operands[1]).LiteralToken.Text);
            Assert.Equal(operand2Value, ((LiteralExpressionSyntax)instruction.Operands[1]).Value);

        }
    }
}
