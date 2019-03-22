using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public sealed class CheatEngineSyntaxTree : SyntaxTree
    {
        public CheatEngineCompilationUnitSyntax Root;

        private CheatEngineSyntaxTree(SourceText text)
        {
            var parser = new Parser(text);
            Root = parser.ParseCompilationUnit();
        }

        public static CheatEngineSyntaxTree Parse(string text)
        {
            return Parse(SourceText.From(text));
        }

        public static CheatEngineSyntaxTree Parse(SourceText text)
        {
            return new CheatEngineSyntaxTree(text);
        }
    }
}
