using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Text
{
    public abstract class StringText : SourceText
    {
        protected StringText(string text) : base(text)
        {

        }
    }
}
