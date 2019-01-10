using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.X64.Parser.Intel
{
    public class MemoryPointerExpressionSyntax : BracketedExpressionSyntax
    {
        public MemoryPointerExpressionSyntax(SyntaxToken pointerTypeToken, SyntaxToken openBracketToken, ExpressionSyntax expression, SyntaxToken closeBracketToken)
            : base(openBracketToken, expression, closeBracketToken)
        {
            PointerTypeToken = pointerTypeToken;
        }

        public override Basm.Core.CodeAnalysis.Syntax.SyntaxKind Kind => Basm.Core.CodeAnalysis.Syntax.SyntaxKind.MemoryPointerExpression;
        public SyntaxToken PointerTypeToken { get; }
    }
}
