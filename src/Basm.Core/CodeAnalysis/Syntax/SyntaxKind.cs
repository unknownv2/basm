using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Syntax
{
    // The types of tokens possible during text parsing.
    public enum SyntaxKind
    {
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        BinaryExpression,
        NameExpression,
        LiteralExpression,
        BracketedExpression,
        ExpressionStatement
    }
}
