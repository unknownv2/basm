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
        DirectiveToken,
        InstructionToken,
        IdentifierToken,
        CommaToken,
        NumberToken,
        OpenBracketToken,
        CloseBracketToken,
        QuoteToken,
        ColonToken,
        SemicolonToken,
        CommentToken,
        PlusToken,
        MinusToken,
        StarToken,
        SlashToken,
        SectionToken,
        CommandToken,
    }
}
