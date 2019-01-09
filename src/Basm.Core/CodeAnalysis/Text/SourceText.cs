using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Text
{
    public class SourceText
    {
        private readonly string _text;
        public SourceText(string text)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public static SourceText From(string text)
        {
            return new SourceText(text);
        }

        public int Length => _text.Length;

        public char this[int index] => _text[index];

        public override string ToString() => _text;

        public string ToString(int start, int length) => _text.Substring(start, length);

        public string ToString(TextSpan span) => ToString(span.Start, span.Length);
    }
}
