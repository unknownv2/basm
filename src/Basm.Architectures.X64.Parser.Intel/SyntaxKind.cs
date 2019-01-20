﻿namespace Basm.Architectures.X64.Parser.Intel
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
