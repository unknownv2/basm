using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.X86.Parser.Intel
{
    public class BracketedExpressionSyntax : Core.CodeAnalysis.Syntax.BracketedExpressionSyntax
    {
        public BracketedExpressionSyntax(SyntaxToken pointerTypeToken, SyntaxToken openBracketToken, ExpressionSyntax expression, SyntaxToken closeBracketToken)
            : base(openBracketToken, expression, closeBracketToken)
        {
            PointerTypeToken = pointerTypeToken;
        }

        public override Core.CodeAnalysis.Syntax.SyntaxKind Kind => Core.CodeAnalysis.Syntax.SyntaxKind.MemoryPointerExpression;
        public SyntaxToken PointerTypeToken { get; }
    }
}
