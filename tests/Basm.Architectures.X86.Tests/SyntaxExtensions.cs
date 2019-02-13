using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Xunit;
using BracketedExpressionSyntax = Basm.Architectures.X86.Parser.Intel.BracketedExpressionSyntax;

namespace Basm.Architectures.X86.Tests
{
    internal static class SyntaxExtensions
    {
        internal static T StatementAs<T>(this SyntaxNode node) where T : ExpressionSyntax
        {
            var expression = node.As<ExpressionStatementSyntax>().Expression;
            return expression.As<T>();
        }

        internal static bool IsStatementType<T>(this SyntaxNode node) where T : ExpressionSyntax
        {
            var expression = node.AsUnchecked<ExpressionStatementSyntax>().Expression;
            return (expression as T) != null;
        }

        internal static T AsUnchecked<T>(this SyntaxNode expression) where T : ExpressionSyntax
        {
            return (T)expression;
        }

        internal static T As<T>(this SyntaxNode expression) where T : ExpressionSyntax
        {
            Assert.IsType<T>(expression);
            return (T)expression;
        }

        internal static string Token(this NameExpressionSyntax expression)
        {
            Assert.NotNull(expression.IdentifierToken);
            return expression.ToString();
        }

        internal static string Token(this InstructionStatementSyntax expression)
        {
            Assert.NotNull(expression.InstructionToken);
            return expression.InstructionToken.Text;
        }

        internal static string PointerTypeToken(this BracketedExpressionSyntax expression)
        {
            Assert.NotNull(expression.PointerTypeToken);
            return expression.PointerTypeToken.Text;
        }
    }
}
