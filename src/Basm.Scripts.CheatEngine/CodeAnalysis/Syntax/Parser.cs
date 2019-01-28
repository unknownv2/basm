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
            var script = ParseScript();
            var endOfFileToken = MatchToken(SyntaxKind.EndOfFileToken);
            return new CheatEngineCompilationUnitSyntax(script, endOfFileToken);
        }

        private CheatEngineSyntaxToken MatchToken(SyntaxKind kind)
        {
            if (Current.Kind == kind)
            {
                return NextToken();
            }

            return new CheatEngineSyntaxToken(kind, Current.Position, null, null);
        }

        private StatementSyntax ParseScript()
        {
            throw new NotImplementedException();
            
        }
    }
}
