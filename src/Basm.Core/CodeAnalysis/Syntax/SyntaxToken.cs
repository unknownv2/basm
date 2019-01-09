using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Syntax
{
    public abstract class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(int position, string text, object value)
        {
            Position = position;
            Text = text;
            Value = value;
        }
        public int Position { get;}
        public string Text { get; }
        public object Value { get; }
    }
}
