using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public class SectionStatementSyntax : StatementSyntax
    {
        public SectionStatementSyntax(SyntaxToken sectionName, ImmutableArray<ExpressionSyntax> script)
        {
            SectionName = sectionName;
            Script = script;
        }

        public SyntaxToken SectionName { get; }
        public ImmutableArray<ExpressionSyntax> Script { get; }
    }
}
