using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Syntax
{
    public class SyntaxToken : SyntaxNode
    {
        public override SyntaxKind Kind { get; }

        public SyntaxToken(SyntaxKind king, int position, string text, object value)
        {

        }
        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

    }
}
