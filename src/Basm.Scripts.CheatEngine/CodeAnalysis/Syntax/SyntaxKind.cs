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
        MnemonicToken,
        RegisterToken,
        IdentifierToken,
        CommaToken,
        NumberToken,
        SizeDirectiveToken,
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
