using System;
using System.Collections.Generic;
using System.Text;
using Basm.Architectures.Parser;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X86.Parser.Intel
{
    public class Parser
    {
        private readonly SourceText _text;

        public Parser(SourceText text)
        {

            _text = text;
   
        }
        public IntelCompilationUnitSyntax ParseCompilationUnit()
        {
            throw new NotImplementedException();
        }
    }
}
