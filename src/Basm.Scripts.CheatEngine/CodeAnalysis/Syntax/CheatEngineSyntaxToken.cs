using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public sealed class CheatEngineSyntaxToken : SyntaxToken
    {
        public SyntaxKind Kind { get; }

        public CheatEngineSyntaxToken(SyntaxKind kind, int position, string text, object value) : base(position, text, value)
        {
            Kind = kind;
        }
    }
}
