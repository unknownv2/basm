using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Scripts.CheatEngine.CodeAnalysis.Syntax
{
    public class Lexer : ILexer
    {
        public const char InvalidCharacter = char.MaxValue;
        private readonly SourceText _text;
        private int _position;
        private int _start;
        private int _offset;
        private SyntaxKind _kind;
        private object _value;

        public Lexer(SourceText text)
        {
            _text = text;
        }

        private char Current => PeekChar();

        private char PeekChar()
        {
            char c = Peek(0);
            if (c != InvalidCharacter)
            {
                AdvanceChar();
            }
            return c;
        }

        private void AdvanceChar()
        {
            _offset++;
        }

        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
            {
                return InvalidCharacter;
            }

            return _text[index];
        }

        public CheatEngineSyntaxToken Lex()
        {
            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;

            switch (Current)
            {
                case '\0':
                case InvalidCharacter:
                    _kind = SyntaxKind.EndOfFileToken;
                    break;
                case '+':
                    _kind = SyntaxKind.PlusToken;
                    _position++;
                    break;
                case '-':
                    _kind = SyntaxKind.MinusToken;
                    _position++;
                    break;
                case '*':
                    _kind = SyntaxKind.StarToken;
                    _position++;
                    break;
                case '/':
                    ScanSlash();
                    break;
                case ',':
                    _kind = SyntaxKind.CommaToken;
                    _position++;
                    break;
                case ':':
                    _kind = SyntaxKind.ColonToken;
                    _position++;
                    break;
                case ';':
                    _kind = SyntaxKind.CommentToken;
                    ScanComment();
                    break;
                case '[':
                    _kind = SyntaxKind.OpenBracketToken;
                    _position++;
                    break;
                case '"':
                    _kind = SyntaxKind.QuoteToken;
                    _position++;
                    break;
                case ']':
                    _kind = SyntaxKind.CloseBracketToken;
                    _position++;
                    break;
                case 'a':
                case 'b':
                case 'c':
                case 'd':
                case 'e':
                case 'f':
                case 'g':
                case 'h':
                case 'i':
                case 'j':
                case 'k':
                case 'l':
                case 'm':
                case 'n':
                case 'o':
                case 'p':
                case 'q':
                case 'r':
                case 's':
                case 't':
                case 'u':
                case 'v':
                case 'w':
                case 'x':
                case 'y':
                case 'z':
                case 'A':
                case 'B':
                case 'C':
                case 'D':
                case 'E':
                case 'F':
                case 'G':
                case 'H':
                case 'I':
                case 'J':
                case 'K':
                case 'L':
                case 'M':
                case 'N':
                case 'O':
                case 'P':
                case 'Q':
                case 'R':
                case 'S':
                case 'T':
                case 'U':
                case 'V':
                case 'W':
                case 'X':
                case 'Y':
                case 'Z':
                    ReadIdentifierOrKeyword();
                    break;
                case ' ':
                case '\t':
                case '\n':
                case '\r':
                    ReadWhiteSpace();
                    break;
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    ReadNumericLiteral();
                    break;
                default:
                    if (char.IsLetter(Current))
                    {
                        ReadIdentifierOrKeyword();
                    }
                    break;
            }

            var length = _position - _start;
            var text = _text.ToString(_start, length);

            return new CheatEngineSyntaxToken(_kind, _start, text, _value);
        }

        private void ReadNumericLiteral()
        {
            while (char.IsDigit(Current))
            {
                _position++;
            }
            var length = _position - _start;
            var text = _text.ToString(_start, length);

            if (!int.TryParse(text, out var value))
            {
                throw new InvalidOperationException("Bad numeric literal");
            }
            _value = value;

            _kind = SyntaxKind.NumberToken;
        }

        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
            {
                _position++;
            }

            _kind = SyntaxKind.WhitespaceToken;
        }

        private void ReadIdentifierOrKeyword()
        {
            var text = ScanIdentifier();
            if (IsDirective(text))
            {
                _kind = SyntaxKind.DirectiveToken;
            }
            else
            {
                _kind = SyntaxKind.IdentifierToken;
            }
        }

        private string ScanIdentifier()
        {
            while (char.IsLetterOrDigit(Current) || Current == '.')
            {
                _position++;
            }

            var length = _position - _start;
            return _text.ToString(_start, length);
        }

        private void ScanSlash()
        {
            if (Peek(1) == '/')
            {
                ScanComment();
            }
            else
            {
                _kind = SyntaxKind.SlashToken;
                _position++;
            }
        }

        private void ScanComment()
        {
            while (Current != InvalidCharacter &&
                   Current != '\0' &&
                   Current != '\r' &&
                   Current != '\n')
            {
                _position++;
            }

            _kind = SyntaxKind.CommentToken;
        }

        private bool IsDirective(string token) => Directives.Contains(token);

        public HashSet<string> Directives { get; } = new HashSet<string>
        {
            "alloc",
            "aobscanmodule",
            "dealloc",
            "loadlibrary",
            "label",
            "readmem",
            "registersymbol",
            "unregistersymbol"
        };
    }
}
