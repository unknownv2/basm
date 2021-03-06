using Basm.Architectures.X64.Parser.Intel;
using Basm.Architectures.X86.Parser.Intel;
using Basm.Core.CodeAnalysis.Syntax;
using Xunit;
using BracketedExpressionSyntax = Basm.Architectures.X86.Parser.Intel.BracketedExpressionSyntax;

namespace Basm.Architectures.X64.Tests
{
    public class IntelParserTests
    {
        [Fact]
        public void ShouldParseInstructionWithZeroOperands()
        {
            const string instructionText = "nop";

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
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
            // push rax
            string instructionText = $"{mnemonic} {operand1}";

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Single(instruction.Operands);

            var operand = instruction.Operand1<RegisterNameExpressionSyntax>();
            Assert.Equal(operand1, operand.Token());
        }

        [Fact]
        public void ShouldParseInstructionWithOne32BitOperand()
        {
            const string mnemonic = "pop";
            const string operand1 = "rcx";
            // pop rcx
            string instructionText = $"{mnemonic} {operand1}";

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Single(instruction.Operands);

            var operand = instruction.Operand1<RegisterNameExpressionSyntax>();
            Assert.Equal(operand1, operand.Token());
        }

        [Fact]
        public void ShouldParseInstructionWithTwoOperands()
        {
            const string mnemonic = "xor";
            const string operand1Text = "rax";
            const string operand2Text = "rcx";
            // xor rax, rcx
            string instructionText = $"{mnemonic} {operand1Text}, {operand2Text}";
            const int operandCount = 2;

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            var operand1 = instruction.Operand1<RegisterNameExpressionSyntax>();
            Assert.Equal(operand1Text, operand1.Token());

            var operand2 = instruction.Operand2<RegisterNameExpressionSyntax>();
            Assert.Equal(operand2Text, operand2.Token());
        }

        [Fact]
        public void ShouldParseInstructionWithQwordPointerSizeOperand()
        {
            const string mnemonic = "push";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            const string pointerType = "QWORD";
            // push QWORD PTR [rax]
            string instructionText = $"{mnemonic} {pointerType} PTR {operand1}";

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Single(instruction.Operands);

            var operand = instruction.Operand1<BracketedExpressionSyntax>();
            Assert.Equal(pointerType, operand.PointerTypeToken());
            Assert.Equal(operandRegister, operand.Expression.StatementAs<RegisterNameExpressionSyntax>().Token());
        }

        [Fact]
        public void ShouldParseInstructionWithNoPointerSizeOperand()
        {
            const string mnemonic = "mov";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            const string pointerType = "";
            const string sourceRegister = "ecx";
            // mov [rax], ecx
            string instructionText = $"{mnemonic} {operand1}, {sourceRegister}"; 
            const int operandCount = 2;

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            var operand = instruction.Operand1<BracketedExpressionSyntax>();
            Assert.Equal(pointerType, operand.PointerTypeToken());

            var pointerExpression = operand.Expression.As<ExpressionStatementSyntax>().Expression;
            Assert.Equal(operandRegister, pointerExpression.As<RegisterNameExpressionSyntax>().Token());
            Assert.Equal(sourceRegister, instruction.Operand2<RegisterNameExpressionSyntax>().Token());
        }

        [Fact]
        public void ShouldParseInstructionWithBytePointerSizeOperand()
        {
            const string mnemonic = "mov";
            const string operandRegister = "rax";
            string operand1 = $"[{operandRegister}]";
            const string pointerType = "BYTE";
            const string sourceRegister = "dl";
            // mov BYTE [rax], dl
            string instructionText = $"{mnemonic} {pointerType} PTR {operand1}, {sourceRegister}";
            const int operandCount = 2;

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            var operand = instruction.Operand1<BracketedExpressionSyntax>();
            Assert.Equal(pointerType, operand.PointerTypeToken());

            var pointerExpression = operand.Expression.As<ExpressionStatementSyntax>().Expression;
            Assert.Equal(operandRegister, pointerExpression.As<RegisterNameExpressionSyntax>().Token());
            Assert.Equal(sourceRegister, instruction.Operand2<RegisterNameExpressionSyntax>().Token());
        }

        [Theory]
        [InlineData("+")]
        [InlineData("-")]
        [InlineData("*")]
        public void ShouldParseInstructionBinaryExpressionWithRegisterAndSourceRegister(string expressionOperator)
        {
            const string mnemonic = "mov";
            const string leftRegister = "rax";
            const string pointerType = "BYTE";
            const int rightImmediateValue = 2;
            const string sourceRegister = "dl";

            string instructionText = $"{mnemonic} {pointerType} ptr [{leftRegister} {expressionOperator} {rightImmediateValue}], {sourceRegister}";
            const int operandCount = 2;

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            var operand = instruction.Operand1<BracketedExpressionSyntax>();
            Assert.Equal(pointerType, operand.PointerTypeToken());

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
            const string operand1Text = "2";
            const int operand1Value = 2;
            // push 2
            string instructionText = $"{mnemonic} {operand1Text}";

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Single(instruction.Operands);

            var operand1 = instruction.Operand1<LiteralExpressionSyntax>();
            Assert.Equal(operand1Text, operand1.LiteralToken.Text);
            Assert.Equal(operand1Value, operand1.Value);
        }

        [Theory]
        [InlineData("2", 2)]
        [InlineData("22H", 0x22)]
        [InlineData("100010b", 0x22)]
        [InlineData("1400o", 768)]
        public void ShouldParseInstructionWithRegisterAndLiteralOperands(string operand2Literal, int operand2Value)
        {
            const string mnemonic = "mov";
            const string operand1Register = "rax";

            string instructionText = $"{mnemonic} {operand1Register}, {operand2Literal}";
            const int operandCount = 2;

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            Assert.Equal(operand1Register, instruction.Operand1<RegisterNameExpressionSyntax>().Token());

            var operand2 = instruction.Operand2<LiteralExpressionSyntax>();
            Assert.Equal(operand2Literal, operand2.LiteralToken.Text);
            Assert.Equal(operand2Value, operand2.Value);
        }

        [Fact]
        public void ShouldParseInstructionWithArbitrarySpacing()
        {
            const string mnemonic = "mov";
            const string operand1Register = "rax";
            const string operand2Literal = "2";
            const int operand2Value = 2;
            // mov rax, 2
            string instructionText = $"  {mnemonic}       {operand1Register},       {operand2Literal}"; 
            const int operandCount = 2;

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            Assert.Equal(operand1Register, instruction.Operand1<RegisterNameExpressionSyntax>().Token());

            var operand2 = instruction.Operand2<LiteralExpressionSyntax>();
            Assert.Equal(operand2Literal, operand2.LiteralToken.Text);
            Assert.Equal(operand2Value, operand2.Value);
        }

        [Fact]
        public void ShouldParseInstructionWithComment()
        {
            const string mnemonic = "mov";
            const string operand1Register = "rax";
            const string operand2Literal = "2";
            const int operand2Value = 2;
            // mov rax, 2
            string instructionText = $"{mnemonic} {operand1Register}, {operand2Literal} ; This is a comment.";
            const int operandCount = 2;

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            Assert.Equal(operand1Register, instruction.Operand1<RegisterNameExpressionSyntax>().Token());

            var operand2 = instruction.Operand2<LiteralExpressionSyntax>();
            Assert.Equal(operand2Literal, operand2.LiteralToken.Text);
            Assert.Equal(operand2Value, operand2.Value);
        }

        [Fact]
        public void ShouldParseInstructionWithCorrectBinaryPrecedence()
        {
            const string mnemonic = "mov";
            const string operand1Register = "rax";
            const string operand2Literal = "2*4+8";
            // mov rax, 2*4+8
            string instructionText = $"{mnemonic} {operand1Register}, {operand2Literal}";
            const int operandCount = 2;

            var syntaxTree = IntelX64SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            Assert.Equal(mnemonic, instruction.Token());
            Assert.Equal(operandCount, instruction.Operands.Length);

            Assert.Equal(operand1Register, instruction.Operand1<RegisterNameExpressionSyntax>().Token());

            var operand2 = instruction.Operand2<BinaryExpressionSyntax>();
            Assert.Equal("+", operand2.OperatorToken.Text);
            Assert.Equal(2, operand2.Left.As<BinaryExpressionSyntax>().Left.As<LiteralExpressionSyntax>().Value);
            Assert.Equal(4, operand2.Left.As<BinaryExpressionSyntax>().Right.As<LiteralExpressionSyntax>().Value);
            Assert.Equal(8, operand2.Right.As<LiteralExpressionSyntax>().Value);
        }
    }
}
