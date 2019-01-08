using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public class Lexer : ILexer
    {
        private readonly SourceText _text;

        public Lexer(SourceText text)
        {
            _text = text;
        }

        public SyntaxToken Lex()
        {
            throw new NotImplementedException();
        }
    }
}
