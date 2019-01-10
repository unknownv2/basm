using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X64.Parser.Intel
{
    public sealed class Parser
    {
        private readonly ImmutableArray<IntelSyntaxToken> _tokens;
        private readonly SourceText _text;

        public Parser(SourceText text)
        {
            var tokens = new List<IntelSyntaxToken>();

            var lexer = new Lexer(text);
            IntelSyntaxToken token;
            do
            {
                token = lexer.Lex();
                if (token.Kind != SyntaxKind.WhitespaceToken)
                {
                    tokens.Add(token);
                }

            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _text = text;
            _tokens = tokens.ToImmutableArray();
        }

        private IntelSyntaxToken Current => Peek(0);
        private int _position;
        
        private IntelSyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
            {
                return _tokens[_tokens.Length - 1];
            }

            return _tokens[index];
        }

        private IntelSyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        public IntelCompilationUnitSyntax ParseCompilationUnit()
        {
            var instruction = ParseInstruction();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new IntelCompilationUnitSyntax(instruction, endOfFileToken);
        }

        private InstructionStatementSyntax ParseInstruction()
        {
            var expected = SyntaxKind.MnemonicToken;
            var instruction = MatchToken(expected);
            var operands = ImmutableArray.CreateBuilder<ExpressionSyntax>();
            while (Current.Kind != SyntaxKind.EndOfFileToken)
            {
                // Parse instruction operands
                operands.Add(ParseExpressionStatement());
            }
            return new IntelInstructionStatementSyntax(instruction, operands.ToImmutable());
        }

        private RegisterNameExpressionSyntax ParseRegisterName()
        {
            var registerToken = MatchToken(SyntaxKind.RegisterToken);
            return new RegisterNameExpressionSyntax(registerToken);
        }

        private ExpressionSyntax ParseExpressionStatement()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.RegisterToken:
                    return ParseRegisterName();
                default:
                    throw new NotImplementedException();

            }
        }

        private StatementSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.MnemonicToken:
                    throw new NotImplementedException();
                default:
                    throw new NotImplementedException();
            }
        }

        private IntelSyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
            {
                return NextToken();
            }

            return new IntelSyntaxToken(kind, Current.Position, null, null);
        }
    }
}
