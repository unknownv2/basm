using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Syntax
{
    public class BracketedExpressionSyntax : ExpressionSyntax
    {
        public BracketedExpressionSyntax(SyntaxToken openBracketToken, ExpressionSyntax expression, SyntaxToken closeBracketToken)
        {
            OpenBracketToken = openBracketToken;
            Expression = expression;
            CloseBracketToken = closeBracketToken;
        }

        public override SyntaxKind Kind => SyntaxKind.BracketedExpression;
        public SyntaxToken OpenBracketToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken CloseBracketToken { get; }
    }
}
