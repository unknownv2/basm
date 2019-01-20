using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
                if (token.Kind != SyntaxKind.WhitespaceToken
                    && token.Kind != SyntaxKind.CommentToken)
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
                // Skip commas since they separate the operands
                if (Current.Kind == SyntaxKind.CommaToken)
                {
                    MatchToken(SyntaxKind.CommaToken);
                    continue;
                }
                // Parse instruction operands
                operands.Add(ParseStatement());
            }
            return new IntelInstructionStatementSyntax(instruction, operands.ToImmutable());
        }

        private RegisterNameExpressionSyntax ParseRegisterName()
        {
            var registerToken = MatchToken(SyntaxKind.RegisterToken);
            return new RegisterNameExpressionSyntax(registerToken);
        }

        private ExpressionSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                default:
                    return ParseExpressionStatement();
            }
        }


        private ExpressionSyntax ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }

        private ExpressionSyntax ParseMemoryPointerExpression()
        {
            if (Peek(0).Kind == SyntaxKind.SizeDirectiveToken)
            {
                var sizeDirective = MatchToken(SyntaxKind.SizeDirectiveToken);
                if (Current.Kind == SyntaxKind.IdentifierToken) // Parse 'PTR', as in DWORD PTR
                {
                    MatchToken(SyntaxKind.IdentifierToken);
                }
                
                /*
                if (Current.Kind == SyntaxKind.SizeDirectiveToken)
                {
                    MatchToken(SyntaxKind.SizeDirectiveToken);
                }*/

                var openBracket = MatchToken(SyntaxKind.OpenBracketToken);
                var expression = ParseStatement();
                var closeBracket = MatchToken(SyntaxKind.CloseBracketToken);
                return new MemoryPointerExpressionSyntax(sizeDirective, openBracket, expression, closeBracket);
            }

            throw new InvalidOperationException("Attempting to read an invalid expression");
        }

        private ExpressionSyntax ParseBracketStatement()
        {
            var sizeDirective = new IntelSyntaxToken(SyntaxKind.SizeDirectiveToken, 0, "", null);
            var openBracket = MatchToken(SyntaxKind.OpenBracketToken);
            var expression = ParseStatement();
            var closeBracket = MatchToken(SyntaxKind.CloseBracketToken);
            return new MemoryPointerExpressionSyntax(sizeDirective, openBracket, expression, closeBracket);
        }

        private ExpressionStatementSyntax ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementSyntax(expression);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left = ParsePrimaryExpression();

            while (true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if (precedence == 0 || precedence <= parentPrecedence)
                {
                    break;
                }

                var operatorToken = NextToken();
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                case SyntaxKind.RegisterToken:
                    return ParseRegisterName();
                case SyntaxKind.NumberToken:
                    return ParseNumberLiteral();
                case SyntaxKind.SizeDirectiveToken:
                    return ParseMemoryPointerExpression();
                case SyntaxKind.OpenBracketToken:
                    return ParseBracketStatement();
                default:
                    return ParseNameExpression();
            }
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var identifierToken = MatchToken(SyntaxKind.IdentifierToken);
            return new NameExpressionSyntax(identifierToken);
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
