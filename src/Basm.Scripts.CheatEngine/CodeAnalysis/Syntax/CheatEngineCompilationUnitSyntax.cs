using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public class CheatEngineCompilationUnitSyntax : CompilationUnitSyntax
    {
        public CheatEngineCompilationUnitSyntax(StatementSyntax scriptStatement, SyntaxToken endOfFileToken)
        {
            ScriptStatement = scriptStatement;
            EndOfFileToken = endOfFileToken;
        }

        public SyntaxToken EndOfFileToken { get; }
        public StatementSyntax ScriptStatement { get; }
    }
}
