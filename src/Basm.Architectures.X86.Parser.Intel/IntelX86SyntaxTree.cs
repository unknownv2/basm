using System;
using System.Collections.Generic;
using System.Text;
using Basm.Architectures.Parser;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X86.Parser.Intel
{
    public class IntelX86SyntaxTree : SyntaxTree
    {
        public IntelCompilationUnitSyntax Root;

        private IntelX86SyntaxTree(SourceText text)
        {
            var parser = new Parser(text);
            Root = parser.ParseCompilationUnit();
        }

        public static IntelX86SyntaxTree Parse(string text)
        {
            return Parse(SourceText.From(text));
        }

        public static IntelX86SyntaxTree Parse(SourceText text)
        {
            return new IntelX86SyntaxTree(text);
        }
    }
}
