using System;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X64.Parser.Intel
{
    public sealed class Lexer
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

        public IntelSyntaxToken Lex()
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
                default:
                    break;
            }

            var length = _position - _start;
            var text = _text.ToString(_start, length);

            return new IntelSyntaxToken(_kind, _start, text, _value);
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
            if (IsInstructionOpCode())
            {
                ReadInstructionOpCode();
            }
            else if (IsRegister())
            {
                ReadRegister();
            }
            else
            {
                ScanIdentifier();
                _kind = SyntaxKind.IdentifierToken;
            }
        }

        private void ReadInstructionOpCode()
        {
            ScanIdentifier();
            _kind = SyntaxKind.OpCodeToken;
        }

        private void ReadRegister()
        {
            ScanIdentifier();
            _kind = SyntaxKind.RegisterToken;
        }

        private void ScanIdentifier()
        {
            while (char.IsLetter(Current))
            {
                _position++;
            }
        }

        private bool IsInstructionOpCode()
        {
            return true;
        }

        private bool IsRegister()
        {
            return false;
        }


    }
}
