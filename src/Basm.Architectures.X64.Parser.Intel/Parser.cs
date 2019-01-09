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

        private InstructionSyntax ParseInstruction()
        {
            var expected = SyntaxKind.OpCodeToken;
            var instruction = MatchToken(expected);
            var operands = ImmutableArray.CreateBuilder<ExpressionSyntax>();
            if (Current.Kind != SyntaxKind.EndOfFileToken)
            {
                // Parse instruction operands;
            }
            return new InstructionSyntax(instruction, operands.ToImmutable());
        }

        private StatementSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.OpCodeToken:
                    break;
                default:
                    break;
            }
            throw new NotImplementedException();

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
