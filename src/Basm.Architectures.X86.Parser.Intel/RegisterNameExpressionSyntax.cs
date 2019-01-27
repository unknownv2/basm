using System;
using System.Collections.Generic;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.X86.Parser.Intel
{
    public class RegisterNameExpressionSyntax : NameExpressionSyntax
    {
        public RegisterNameExpressionSyntax(SyntaxToken registerNameToken) : base(registerNameToken)
        {

        }

        public override string ToString() => base.ToString();
    }
}
