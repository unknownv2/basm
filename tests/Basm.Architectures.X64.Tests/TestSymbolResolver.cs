using System;
using System.Collections.Generic;
using System.Text;
using Basm.Architectures.X64.Parser.Intel;
using Basm.Architectures.X64.Tests;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Scripting;

namespace Basm.Architectures.X64.Tests
{
    public class TestSymbolResolver : SymbolResolver
    {
        public override string ResolveSymbol(object symbol)
        {
            if(symbol is ExpressionStatementSyntax statement)
            {
                if (statement.IsStatementType<RegisterNameExpressionSyntax>())
                {
                    return ResolveInstructionName(statement.StatementAs<RegisterNameExpressionSyntax>());
                }

                if (statement.IsStatementType<LiteralExpressionSyntax>())
                {
                    return statement.StatementAs<LiteralExpressionSyntax>().LiteralToken.Text;
                }
            }
            return (string)symbol;
        }

        private string ResolveInstructionName(RegisterNameExpressionSyntax register)
        {
            return register.Token();
        }
    }
}
