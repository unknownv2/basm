using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X64.Parser.Intel
{
    public class IntelX64SyntaxTree : SyntaxTree
    {
        public IntelCompilationUnitSyntax Root;

        private IntelX64SyntaxTree(SourceText text)
        {
            var parser = new Parser(text);
            Root = parser.ParseCompilationUnit();
        }

        public static IntelX64SyntaxTree Parse(string text)
        {
            return Parse(SourceText.From(text));
        }

        public static IntelX64SyntaxTree Parse(SourceText text)
        {
            return new IntelX64SyntaxTree(text);
        }
    }
}
