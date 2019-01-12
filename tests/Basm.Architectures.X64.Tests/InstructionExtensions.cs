using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Xunit;

namespace Basm.Architectures.X64.Tests
{
    internal static class InstructionExtensions
    {
        internal static T Operand1<T>(this InstructionStatementSyntax instruction) where T : ExpressionSyntax
        {
            return Operand<T>(instruction, index: 0);
        }

        internal static T Operand2<T>(this InstructionStatementSyntax instruction) where T : ExpressionSyntax
        {
            return Operand<T>(instruction, index: 1);
        }

        internal static T Operand3<T>(this InstructionStatementSyntax instruction) where T : ExpressionSyntax
        {
            return Operand<T>(instruction, index: 2);
        }

        internal static T Operand<T>(this InstructionStatementSyntax instruction, int index) where T : ExpressionSyntax
        {
            var operand = instruction.Operands.ElementAt(index);
            var expression = GetStatementExpression(operand);
            Assert.IsType<T>(expression);
            return (T) (expression);
        }

        internal static ExpressionSyntax GetStatementExpression(ExpressionSyntax statement)
        {
            Assert.IsType<ExpressionStatementSyntax>(statement);
            return ((ExpressionStatementSyntax)statement).Expression;
        }
    }
}
