using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public class CheatEngineCompilationUnitSyntax : CompilationUnitSyntax
    {
        public CheatEngineCompilationUnitSyntax(SectionStatementSyntax sectionStatement, SyntaxToken endOfFileToken)
        {
            SectionStatement = sectionStatement;
            EndOfFileToken = endOfFileToken;
        }

        public SyntaxToken EndOfFileToken { get; }
        public SectionStatementSyntax SectionStatement { get; }
    }
}
