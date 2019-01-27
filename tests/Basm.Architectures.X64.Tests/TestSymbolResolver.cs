using System;
using System.Collections.Generic;
using System.Text;
using Basm.Architectures.X64.Parser.Intel;
using Basm.Architectures.X64.Tests;
using Basm.Architectures.X86.Parser.Intel;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Scripting;
using BracketedExpressionSyntax = Basm.Architectures.X86.Parser.Intel.BracketedExpressionSyntax;

namespace Basm.Architectures.X64.Tests
{
    public class TestSymbolResolver : SymbolResolver
    {
        public override string ResolveSymbol(object symbol)
        {
            if(symbol is ExpressionStatementSyntax statement)
            {
                return ResolveExpressionStatement(statement);
            }
            if (symbol is ExpressionSyntax expression)
            {
                return ResolveExpression(expression);
            }

            return (string)symbol;
        }

        private string ResolveExpressionStatement(ExpressionStatementSyntax statement)
        {
            if (statement.IsStatementType<RegisterNameExpressionSyntax>())
            {
                return ResolveInstructionName(statement.StatementAs<RegisterNameExpressionSyntax>());
            }
            if (statement.IsStatementType<LiteralExpressionSyntax>())
            {
                return ResolveLiteral(statement.StatementAs<LiteralExpressionSyntax>());
            }
            if (statement.IsStatementType<BinaryExpressionSyntax>())
            {
                return ResolveBinaryExpression(statement.StatementAs<BinaryExpressionSyntax>());
            }
            if (statement.IsStatementType<BracketedExpressionSyntax>())
            {
                return ResolveMemoryPointerExpression(statement.StatementAs<BracketedExpressionSyntax>());
            }
            return string.Empty;
        }

        private string ResolveExpression(ExpressionSyntax expression)
        {
            if (expression is RegisterNameExpressionSyntax register)
            {
                return ResolveInstructionName(register);
            }
            if (expression is LiteralExpressionSyntax literal)
            {
                return ResolveLiteral(literal);
            }
            if (expression is BinaryExpressionSyntax binaryExpression)
            {
                return ResolveBinaryExpression(binaryExpression);
            }
            if (expression is BracketedExpressionSyntax memoryPointer)
            {
                return ResolveMemoryPointerExpression(memoryPointer);
            }
            return string.Empty;
        }

        private string ResolveInstructionName(RegisterNameExpressionSyntax register)
        {
            return register.Token();
        }

        private string ResolveLiteral(LiteralExpressionSyntax literal)
        {
            return literal.LiteralToken.Text;
        }

        private string ResolveBinaryExpression(BinaryExpressionSyntax expression)
        {
            var left = ResolveSymbol(expression.Left);
            var right = ResolveSymbol(expression.Right);

            return $"{left} {expression.OperatorToken.Text} {right}";
        }

        private string ResolveMemoryPointerExpression(BracketedExpressionSyntax expression)
        {
            var innerExpression = ResolveSymbol(expression.Expression.As<ExpressionStatementSyntax>());

            return $"{expression.PointerTypeToken.Text} ptr [{innerExpression}]";
        }
    }
}
