using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Syntax
{
    public class NameExpressionSyntax : ExpressionSyntax
    {
        public NameExpressionSyntax(SyntaxToken identifierToken)
        {
            IdentifierToken = identifierToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NameExpression;
        public SyntaxToken IdentifierToken { get; }
    }
}
