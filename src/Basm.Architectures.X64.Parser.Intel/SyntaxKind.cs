using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Architectures.X64.Parser.Intel
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
    }
}
