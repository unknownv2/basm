using System;
using System.Collections.Generic;
using System.Text;

namespace Basm.Core.CodeAnalysis.Syntax
{
    public abstract class SyntaxNode
    {
        public abstract SyntaxKind Kind { get; }
    }
}
