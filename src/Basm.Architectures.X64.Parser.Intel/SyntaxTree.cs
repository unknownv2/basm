using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X64.Parser.Intel
{
    public class SyntaxTree
    {
        public IntelCompilationUnitSyntax Root;

        private SyntaxTree(SourceText text)
        {
            var parser = new Parser(text);
            Root = parser.ParseCompilationUnit();
        }

        public static SyntaxTree Parse(string text)
        {
            return Parse(SourceText.From(text));
        }

        public static SyntaxTree Parse(SourceText text)
        {
            return new SyntaxTree(text);
        }
    }
}
