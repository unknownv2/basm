using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.X64.Parser.Intel
{
    public sealed class IntelSyntaxToken : SyntaxToken
    {
        public SyntaxKind Kind { get; }

        public IntelSyntaxToken(SyntaxKind kind, int position, string text, object value) : base(position, text, value)
        {
            Kind = kind;
        }
    }
}
