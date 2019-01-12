using Basm.Architectures.X64.Parser.Intel;
using Basm.Architectures.X64.Tests;
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

            Assert.Equal(operand1, ((RegisterNameExpressionSyntax)((ExpressionStatementSyntax)instruction.Operands[0]).Expression).IdentifierToken.Text);
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
        public void ShouldParseInstructionWithNoPointerSizeOperand()
        {
            const string mnemonic = "push";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            string instructionText = $"{mnemonic} {operand1}";
            const int operandCount = 1;
            const string pointerType = "DWORD";

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            
            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);
            Assert.Equal(pointerType, ((MemoryPointerExpressionSyntax) instruction.Operands[0]).PointerTypeToken.Text);
            Assert.Equal(operandRegister, ((RegisterNameExpressionSyntax)((MemoryPointerExpressionSyntax)instruction.Operands[0]).Expression).IdentifierToken.Text);
        }

        [Fact]
        public void ShouldParseInstructionWithDwordPointerSizeOperand()
        {
            const string mnemonic = "push";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            const string pointerType = "DWORD";
            string instructionText = $"{mnemonic} {pointerType} {operand1}";
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);
            Assert.Equal(pointerType, ((MemoryPointerExpressionSyntax)instruction.Operands[0]).PointerTypeToken.Text);
            Assert.Equal(operandRegister, ((RegisterNameExpressionSyntax)((MemoryPointerExpressionSyntax)instruction.Operands[0]).Expression).IdentifierToken.Text);
        }

        [Fact]
        public void ShouldParseInstructionWithBytePointerSizeOperand()
        {
            const string mnemonic = "push";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            const string pointerType = "BYTE";
            string instructionText = $"{mnemonic} {pointerType} {operand1}";
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand = instruction.Operand1<MemoryPointerExpressionSyntax>();
            Assert.Equal(pointerType, operand.PointerTypeToken.Text);
            var pointerExpression = operand.Expression.As<ExpressionStatementSyntax>().Expression;
            Assert.Equal(operandRegister, pointerExpression.As<RegisterNameExpressionSyntax>().IdentifierToken.Text);
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
            var operand = instruction.Operand1<LiteralExpressionSyntax>();
            Assert.Equal(operand1, operand.LiteralToken.Text);
            Assert.Equal(operand1Value, operand.Value);
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
            Assert.Equal(operand1, (instruction.Operand1<RegisterNameExpressionSyntax>()).IdentifierToken.Text);
            Assert.Equal(operand2, ((LiteralExpressionSyntax)instruction.Operands[1]).LiteralToken.Text);
            Assert.Equal(operand2Value, ((LiteralExpressionSyntax)instruction.Operands[1]).Value);
        }

        [Fact]
        public void ShouldParseInstructionBinaryExpressionWithRegisterAndImmediate()
        {
            const string mnemonic = "push";
            const string leftRegister = "rax";
            const string expressionOperator = "+";
            const int rightImmediateValue = 2;
          
            // Instruction: 'push rax+2'
            string instructionText = $"{mnemonic} {leftRegister}{expressionOperator}{rightImmediateValue}";
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            var binaryExpression = instruction.Operand1<BinaryExpressionSyntax>();

            Assert.Equal(mnemonic, instruction.InstructionToken.Text);
            Assert.Equal(operandCount, instruction.Operands.Length);

            Assert.Equal(leftRegister, (binaryExpression.Left as RegisterNameExpressionSyntax).IdentifierToken.Text);
            Assert.Equal(expressionOperator, binaryExpression.OperatorToken.Text);
            Assert.Equal(rightImmediateValue, (binaryExpression.Right as LiteralExpressionSyntax).Value);
            Assert.Equal(rightImmediateValue, (binaryExpression.Right as LiteralExpressionSyntax).Value);
        }
    }
}
