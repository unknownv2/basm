using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Architectures.X86.Parser.Intel
{
    public static class SyntaxFacts
    {
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.SlashToken:
                case SyntaxKind.StarToken:
                    return 5;
                case SyntaxKind.MinusToken:
                case SyntaxKind.PlusToken:
                    return 4;
                default:
                    return 0;
            }
        }
    }
}
