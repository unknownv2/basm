using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Text
{
    public struct TextSpan
    {
        public int Start { get; }
        public int Length { get; }
        public int End => Start + Length;

        public TextSpan(int start, int length)
        {
            Start = start;
            Length = length;

        }

        public static TextSpan FromStringBounds(int start, int end)
        {
            return new TextSpan(start, end - start);
        }
    }
}
