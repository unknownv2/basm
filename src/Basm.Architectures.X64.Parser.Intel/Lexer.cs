using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X64.Parser.Intel
{
    public class Lexer : X86.Parser.Intel.Lexer
    {
        public const char InvalidCharacter = char.MaxValue;
        private readonly SourceText _text;
        private int _position;
        private int _start;
        private int _offset;
        private SyntaxKind _kind;
        private object _value;

        public Lexer()
        {
            Extend();
        }

        public Lexer(SourceText text) : this()
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
                    _kind = SyntaxKind.SlashToken;
                    _position++;
                    break;
                case ',':
                    _kind = SyntaxKind.CommaToken;
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

            return new IntelSyntaxToken(_kind, _start, text, _value);
        }

        private void ReadStatement()
        {

        }

        private void ReadNumericLiteral()
        {
            while (char.IsDigit(Current))
            {
                _position++;
            }
            var length = _position - _start;
            var text = _text.ToString(_start, length);

            if (IsLiteralSuffix(Current.ToString()))
            {
                switch (Current)
                {
                    case 'h':
                    case 'H':
                        if (!int.TryParse(text, NumberStyles.HexNumber,
                            CultureInfo.CurrentCulture, out var value))
                        {
                            throw new InvalidOperationException("Bad numeric hex literal");
                        }
                        _value = value;
                        break;
                    case 'b':
                    case 'B':
                        _value = Convert.ToInt32(text, 2);
                        break;
                }
                _position++;
            }
            else
            {
                if (!int.TryParse(text, out var value))
                {
                    throw new InvalidOperationException("Bad numeric literal");
                }
                _value = value;
            }
   
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
            else if (IsSizeDirective(text))
            {
                _kind = SyntaxKind.SizeDirectiveToken;
            }
            else
            {
                _kind = SyntaxKind.IdentifierToken;
            }
        }

        private string ScanIdentifier()
        {
            while (char.IsLetter(Current) || char.IsDigit(Current))
            {
                _position++;
            }

            var length = _position - _start;
            return _text.ToString(_start, length);
        }

        private void ScanComment()
        {
            while (Current != InvalidCharacter && Current != '\0')
            {
                _position++;
            }
        }

        private bool IsLiteralSuffix(string suffix) => LiteralSuffixes.Contains(suffix.ToLower());

        private bool IsInstructionMnemonic(string mnemonic) => InstructionSet.Contains(mnemonic.ToLower());

        private bool IsRegister(string register) =>  RegisterSet.Contains(register.ToLower());

        private bool IsSizeDirective(string directive) => SizeDirective.Contains(directive.ToLower());

        /// <summary>
        /// Extend the base x86 architecture.
        /// </summary>
        private void Extend()
        {
            // Extend the base X86 register set.
            RegisterSet.UnionWith(
                new HashSet<string>
                {
                    "rax", "rbx", "rcx", "rdx",
                    "rsp", "rbp", "rsi", "rdi",
                    "r8b", "r8w", "r8d", "r8",
                    "r9b", "r9w", "r9d", "r9",
                    "r10b", "r10w", "r10d", "r10",
                    "r11b", "r11w", "r11d", "r11",
                    "r12b", "r12w", "r12d", "r12",
                    "r13b", "r13w", "r13d", "r13",
                    "r14b", "r14w", "r14d", "r14",
                    "r15b", "r15w", "r15d", "r15",
                    "mm8", "mm9", "mm10", "mm11",
                    "mm12", "mm13", "mm14", "mm15",
                    "xmm8", "xmm9", "xmm10", "xmm11",
                    "xmm12", "xmm13", "xmm14", "xmm15",
                    "xmm16", "xmm17", "xmm18", "xmm19",
                    "xmm20", "xmm21", "xmm22", "xmm23",
                    "xmm24", "xmm25", "xmm26", "xmm27",
                    "xmm28", "xmm29", "xmm30", "xmm31",
                    "ymm8", "ymm9", "ymm10", "ymm11",
                    "ymm12", "ymm13", "ymm14", "ymm15",
                    "ymm16", "ymm17", "ymm18", "ymm19",
                    "ymm20", "ymm21", "ymm22", "ymm23",
                    "ymm24", "ymm25", "ymm26", "ymm27",
                    "ymm28", "ymm29", "ymm30", "ymm31",
                    "zmm8", "zmm9", "zmm10", "zmm11",
                    "zmm12", "zmm13", "zmm14", "zmm15",
                    "zmm16", "zmm17", "zmm18", "zmm19",
                    "zmm20", "zmm21", "zmm22", "zmm23",
                    "zmm24", "zmm25", "zmm26", "zmm27",
                    "zmm28", "zmm29", "zmm30", "zmm31",
                    "st8", "st9", "st10", "st11",
                    "st12", "st13", "st14", "st15",
                    "cr8", "cr9", "cr10", "cr11",
                    "cr12", "cr13", "cr14", "cr15",
                }
            );

            // Extend the base x86 instruction set.
            InstructionSet.UnionWith(new HashSet<string>
            {
               
            });
        }
    }
}
