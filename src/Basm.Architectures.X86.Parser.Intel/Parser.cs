using System;
using System.Collections.Generic;
using Basm.Architectures.Parser;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;
using System.Collections.Immutable;

namespace Basm.Architectures.X86.Parser.Intel
{
    public class Parser
    {
        private ImmutableArray<IntelSyntaxToken> _tokens;
        private SourceText _text;

        public Parser(SourceText text, Lexer lexer = null)
        {
            _text = text;
            ParseText(lexer ?? new Lexer(text));
        }

        public void ParseText(Lexer lexer)
        {
            var tokens = new List<IntelSyntaxToken>();

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
                // Skip commas since they separate the operands.
                if (Current.Kind == SyntaxKind.CommaToken)
                {
                    MatchToken(SyntaxKind.CommaToken);
                    continue;
                }
                // Parse instruction operands.
                operands.Add(ParseStatement());
            }
            return new IntelInstructionStatementSyntax(instruction, operands.ToImmutable());
        }

        private ExpressionSyntax ParseStatement()
        {
            switch (Current.Kind)
            {
                default:
                    return ParseExpressionStatement();
            }
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
                    return ParseBracketExpression();
                default:
                    return ParseNameExpression();
            }
        }

        private RegisterNameExpressionSyntax ParseRegisterName()
        {
            var registerToken = MatchToken(SyntaxKind.RegisterToken);
            return new RegisterNameExpressionSyntax(registerToken);
        }

        private ExpressionSyntax ParseNumberLiteral()
        {
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }

        private ExpressionSyntax ParseMemoryPointerExpression()
        {
            if (Current.Kind == SyntaxKind.SizeDirectiveToken)
            {
                var sizeDirective = MatchToken(SyntaxKind.SizeDirectiveToken);
                // Parse 'PTR', as in DWORD PTR.
                if (Current.Kind == SyntaxKind.IdentifierToken)
                {
                    MatchToken(SyntaxKind.IdentifierToken);
                }

                var openBracket = MatchToken(SyntaxKind.OpenBracketToken);
                var expression = ParseStatement();
                var closeBracket = MatchToken(SyntaxKind.CloseBracketToken);
                return new BracketedExpressionSyntax(sizeDirective, openBracket, expression, closeBracket);
            }

            throw new InvalidOperationException("Attempting to read an invalid expression");
        }

        private ExpressionSyntax ParseBracketExpression()
        {
            var sizeDirective = new IntelSyntaxToken(SyntaxKind.SizeDirectiveToken, 0, string.Empty, null);
            var openBracket = MatchToken(SyntaxKind.OpenBracketToken);
            var expression = ParseStatement();
            var closeBracket = MatchToken(SyntaxKind.CloseBracketToken);
            return new BracketedExpressionSyntax(sizeDirective, openBracket, expression, closeBracket);
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
