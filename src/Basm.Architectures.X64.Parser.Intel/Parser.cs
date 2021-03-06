﻿using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X64.Parser.Intel
{
    public class Parser : X86.Parser.Intel.Parser
    {
        public Parser(SourceText text) : base(text, new Lexer(text))
        {
        }
    }
}
