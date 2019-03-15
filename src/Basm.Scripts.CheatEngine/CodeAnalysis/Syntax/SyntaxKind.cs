using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        BadToken,
        EndOfFileToken,
        WhitespaceToken,
        InstructionToken,
        IdentifierToken,
        CommaToken,
        NumberToken,
        OpenBracketToken,
        CloseBracketToken,
        SemicolonToken,
        CommentToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
    }
}
