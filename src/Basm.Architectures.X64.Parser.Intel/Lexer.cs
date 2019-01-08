using System;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X64.Parser.Intel
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
