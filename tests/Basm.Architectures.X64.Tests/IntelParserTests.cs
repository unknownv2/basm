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

            Assert.Equal(instructionText, instruction.Token());
            Assert.Empty(instruction.Operands);
        }

        [Fact]
        public void ShouldParseInstructionWithOneOperand()
        {
            const string mnemonic = "push";
            const string operand1 = "rax";
            string instructionText = $"{mnemonic} {operand1}"; // push rax
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand = instruction.Operand1<RegisterNameExpressionSyntax>();
            Assert.Equal(operand1, operand.Token());
        }

        [Fact]
        public void ShouldParseInstructionWithOne32BitOperand()
        {
            const string mnemonic = "pop";
            const string operand1 = "rcx";
            string instructionText = $"{mnemonic} {operand1}"; // pop rcx
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand = instruction.Operand1<RegisterNameExpressionSyntax>();
            Assert.Equal(operand1, operand.Token());
        }

        [Fact]
        public void ShouldParseInstructionWithTwoOperands()
        {
            const string mnemonic = "xor";
            const string operand1Text = "rax";
            const string operand2Text = "rcx";

            string instructionText = $"{mnemonic} {operand1Text}, {operand2Text}"; // xor rax, rcx
            const int operandCount = 2;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand1 = instruction.Operand1<RegisterNameExpressionSyntax>();
            var operand2 = instruction.Operand2<RegisterNameExpressionSyntax>();
            Assert.Equal(operand1Text, operand1.Token());
            Assert.Equal(operand2Text, operand2.Token());
        }

        [Fact]
        public void ShouldParseInstructionWithNoPointerSizeOperand()
        {
            const string mnemonic = "push";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            const string pointerType = "QWORD";
            string instructionText = $"{mnemonic} {pointerType} PTR {operand1}"; // push QWORD PTR [rax]
            const int operandCount = 1;


            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand = instruction.Operand1<MemoryPointerExpressionSyntax>();
            Assert.Equal(pointerType, operand.Token());
            Assert.Equal(operandRegister, operand.Expression.StatementAs<RegisterNameExpressionSyntax>().Token());
        }

        [Fact]
        public void ShouldParseInstructionWithDwordPointerSizeOperand()
        {
            const string mnemonic = "push";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            const string pointerType = "DWORD";
            string instructionText = $"{mnemonic} {pointerType} PTR {operand1}"; // push DWORD [rax]
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand = instruction.Operand1<MemoryPointerExpressionSyntax>();
            Assert.Equal(pointerType, operand.Token());
            Assert.Equal(operandRegister, operand.Expression.StatementAs<RegisterNameExpressionSyntax>().Token());
        }

        [Fact]
        public void ShouldParseInstructionWithBytePointerSizeOperand()
        {
            const string mnemonic = "push";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            const string pointerType = "BYTE";
            string instructionText = $"{mnemonic} {pointerType} {operand1}"; // push BYTE [rax]
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand = instruction.Operand1<MemoryPointerExpressionSyntax>();
            Assert.Equal(pointerType, operand.Token());
            var pointerExpression = operand.Expression.As<ExpressionStatementSyntax>().Expression;
            Assert.Equal(operandRegister, pointerExpression.As<RegisterNameExpressionSyntax>().Token());
        }


        [Theory]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        [InlineData("/")]
        public void ShouldParseInstructionBinaryExpressionWithRegisterAndSourceRegister(string expressionOperator)
        {
            const string mnemonic = "mov";
            const string leftRegister = "rax";
            const string pointerType = "BYTE";
            const int rightImmediateValue = 2;
            const string sourceRegister = "dl";

            // mov BYTE [rax + 2], dl
            string instructionText = $"{mnemonic} {pointerType} [{leftRegister} {expressionOperator} {rightImmediateValue}], {sourceRegister}";
            const int operandCount = 2;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand = instruction.Operand1<MemoryPointerExpressionSyntax>();
            Assert.Equal(pointerType, operand.Token());
            var pointerExpression = operand.Expression.As<ExpressionStatementSyntax>().Expression;
            var binaryExpression = pointerExpression.As<BinaryExpressionSyntax>();
            var left = binaryExpression.Left.As<RegisterNameExpressionSyntax>();
            Assert.Equal(leftRegister, left.Token());
            Assert.Equal(expressionOperator, binaryExpression.OperatorToken.Text);
            var right = binaryExpression.Right.As<LiteralExpressionSyntax>();
            Assert.Equal(rightImmediateValue, right.Value);
            var operand2 = instruction.Operand2<RegisterNameExpressionSyntax>();
            Assert.Equal(sourceRegister, operand2.Token());
        }

        [Fact]
        public void ShouldParseInstructionWithLiteralOperand()
        {
            const string mnemonic = "push";
            const string operand1 = "2";
            const int operand1Value = 2;
            string instructionText = $"{mnemonic} {operand1}"; // push 2
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand = instruction.Operand1<LiteralExpressionSyntax>();
            Assert.Equal(operand1, operand.LiteralToken.Text);
            Assert.Equal(operand1Value, operand.Value);
        }

        [Fact]
        public void ShouldParseInstructionWithRegisterAndLiteralOperands()
        {
            const string mnemonic = "mov";
            const string operand1Register = "rax";
            const string operand2Literal = "2";
            const int operand2Value = 2;

            string instructionText = $"{mnemonic} {operand1Register}, {operand2Literal}"; // mov rax, 2
            const int operandCount = 2;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);
            var operand2 = instruction.Operand2<LiteralExpressionSyntax>();
            Assert.Equal(operand1Register, instruction.Operand1<RegisterNameExpressionSyntax>().Token());
            Assert.Equal(operand2Literal, operand2.LiteralToken.Text);
            Assert.Equal(operand2Value, operand2.Value);
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

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            var left = binaryExpression.Left.As<RegisterNameExpressionSyntax>();
            Assert.Equal(leftRegister, left.Token());
            Assert.Equal(expressionOperator, binaryExpression.OperatorToken.Text);
            var right = binaryExpression.Right.As<LiteralExpressionSyntax>();
            Assert.Equal(rightImmediateValue.ToString(), right.LiteralToken.Text);
            Assert.Equal(rightImmediateValue, right.Value);
        }

        [Fact]
        public void ShouldParseInstructionWithArbitrarySpacing()
        {
            const string mnemonic = "push";
            const string leftRegister = "rax";
            const string expressionOperator = "+";
            const int rightImmediateValue = 2;

            // Instruction: 'push   rax   +   2'
            string instructionText = $"{mnemonic}   {leftRegister}  {expressionOperator}   {rightImmediateValue}";
            const int operandCount = 1;

            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            var binaryExpression = instruction.Operand1<BinaryExpressionSyntax>();

            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            var left = binaryExpression.Left.As<RegisterNameExpressionSyntax>();
            Assert.Equal(leftRegister, left.Token());
            Assert.Equal(expressionOperator, binaryExpression.OperatorToken.Text);
            var right = binaryExpression.Right.As<LiteralExpressionSyntax>();
            Assert.Equal(rightImmediateValue.ToString(), right.LiteralToken.Text);
            Assert.Equal(rightImmediateValue, right.Value);
        }
    }
}
