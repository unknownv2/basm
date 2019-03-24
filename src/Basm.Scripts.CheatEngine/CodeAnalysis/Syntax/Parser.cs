using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public class Parser
    {
        private ImmutableArray<CheatEngineSyntaxToken> _tokens;
        private SourceText _text;

        public Parser(SourceText text, Lexer lexer = null)
        {
            _text = text;
            ParseText(lexer ?? new Lexer(text));
        }

        public void ParseText(Lexer lexer)
        {
            var tokens = new List<CheatEngineSyntaxToken>();

            CheatEngineSyntaxToken token;
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

        private CheatEngineSyntaxToken Current => Peek(0);
        private int _position;

        private CheatEngineSyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
            {
                return _tokens[_tokens.Length - 1];
            }

            return _tokens[index];
        }

        private CheatEngineSyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        public CheatEngineCompilationUnitSyntax ParseCompilationUnit()
        {
            var section = ParseScriptSection();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new CheatEngineCompilationUnitSyntax(section, endOfFileToken);
        }

        private CheatEngineSyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
            {
                return NextToken();
            }
            return new CheatEngineSyntaxToken(kind, Current.Position, null, null);
        }

        private ExpressionStatementSyntax ParseExpressionStatement()
        {
            var expression = ParseExpression();
            return new ExpressionStatementSyntax(expression);
        }

        private ExpressionSyntax ParseExpression()
        {
            return ParsePrimaryExpression();
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            switch (Current.Kind)
            {
                default:
                    return ParseExpressionStatement();
            }
        }

        private SectionStatementSyntax ParseScriptSection()
        {
            MatchToken(SyntaxKind.OpenBracketToken);
            var section = MatchToken(SyntaxKind.IdentifierToken);
            var scriptCode = ImmutableArray.CreateBuilder<ExpressionSyntax>();
            while (Current.Kind != SyntaxKind.OpenBracketToken && 
                   Current.Kind != SyntaxKind.EndOfFileToken)
            {
                // Keep parsing tokens until end of section or end of file.
                NextToken();
            }
            return new SectionStatementSyntax(section, scriptCode.ToImmutable());
        }
    }
}
