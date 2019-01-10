using System;
using System.Collections.Generic;
using System.IO;
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
                case ',':
                    _kind = SyntaxKind.CommaToken;
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

            return new IntelSyntaxToken(_kind, _start, text, _value);
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
                throw new InvalidDataException("Bad numeric literal");
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
            string text = ScanIdentifier();
            if (IsInstructionMnemonic(text))
            {
                _kind = SyntaxKind.MnemonicToken;
            }
            else if (IsRegister(text))
            {
                _kind = SyntaxKind.RegisterToken;
            }
            else
            {
                
                _kind = SyntaxKind.IdentifierToken;
            }
        }

        private string ScanIdentifier()
        {
            while (char.IsLetter(Current))
            {
                _position++;
            }
            var length = _position - _start;
            return _text.ToString(_start, length);
        }

        private bool IsInstructionMnemonic(string mnemonic) => InstructionSet.Contains(mnemonic.ToLower());

        private static readonly HashSet<string> InstructionSet = new HashSet<string>
        {
            "add", "aam", "aas", "adc", "add",
            "mov", "nop", "push", "pop", "xor"
        };

        private bool IsRegister(string register) =>  RegisterSet.Contains(register.ToLower());

        private static readonly HashSet<string> RegisterSet = new HashSet<string>
        {
            "al", "ah", "ax", "eax", "rax",
            "bl", "bh", "bx", "ebx", "rbx",
            "cl", "ch", "cx", "ecx", "rcx",
            "dl", "dh", "dx", "edx", "rdx",
            "spl", "sp", "esp", "rsp",
            "bpl", "bp", "ebp", "rbp",
            "sil", "si", "esi", "rsi",
            "dil", "di", "edi", "rdi",
            "r8b", "r8w", "r8d", "r8",
            "r9b", "r9w", "r9d", "r9",
            "r10b", "r10w", "r10d", "r10",
            "r11b", "r11w", "r11d", "r11",
            "r12b", "r12w", "r12d", "r12",
            "r13b", "r13w", "r13d", "r13",
            "r14b", "r14w", "r14d", "r14",
            "r15b", "r15w", "r15d", "r15",
        };
    }
}
