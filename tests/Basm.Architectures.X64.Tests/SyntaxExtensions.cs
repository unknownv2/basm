using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Xunit;

namespace Basm.Architectures.X64.Tests
{
    internal static class SyntaxExtensions
    {
        internal static T As<T>(this SyntaxNode expression) where T : ExpressionSyntax
        {
            Assert.IsType<T>(expression);
            return (T)expression;
        }
    }
}
