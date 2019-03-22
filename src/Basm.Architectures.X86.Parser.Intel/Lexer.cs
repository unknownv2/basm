using System;
using System.Collections.Generic;
using System.Globalization;
using Basm.Architectures.Parser;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X86.Parser.Intel
{
    public class Lexer : IAssemblyLexer
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

            if (IsLiteralSuffix(Current))
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
                    case 'o':
                    case 'O':
                        _value = Convert.ToInt32(text, 8);
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
            while (char.IsLetterOrDigit(Current))
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

        private bool IsLiteralSuffix(char suffix) => LiteralSuffixes.Contains(char.ToLower(suffix));

        private bool IsInstructionMnemonic(string mnemonic) => InstructionSet.Contains(mnemonic.ToLower());

        private bool IsRegister(string register) => RegisterSet.Contains(register.ToLower());

        private bool IsSizeDirective(string directive) => SizeDirectives.Contains(directive.ToLower());

        public HashSet<string> RegisterSet { get; } = new HashSet<string>
        {
            "al", "ah", "ax", "eax",
            "bl", "bh", "bx", "ebx",
            "cl", "ch", "cx", "ecx",
            "dl", "dh", "dx", "edx",
            "spl", "sp", "esp",
            "bpl", "bp", "ebp",
            "sil", "si", "esi",
            "dil", "di", "edi",
            "mm0", "mm1", "mm2", "mm3",
            "mm4", "mm5", "mm6", "mm7",
            "xmm0", "xmm1", "xmm2", "xmm3",
            "xmm4", "xmm5", "xmm6", "xmm7",
            "ymm0", "ymm1", "ymm2", "ymm3",
            "ymm4", "ymm5", "ymm6", "ymm7",
            "zmm0", "zmm1", "zmm2", "zmm3",
            "zmm4", "zmm5", "zmm6", "zmm7",
            "st0", "st1", "st2", "st3",
            "st4", "st5", "st6", "st7",
            "cr0", "cr1", "cr2", "cr3",
            "cr4", "cr5", "cr6", "cr7",
            "dr0", "dr1", "dr2", "dr3",
            "dr4", "dr5", "dr6", "dr7",
            "cs", "ds", "es", "fs", "gs",
            "ss"
        };

        public HashSet<string> InstructionSet { get; } = new HashSet<string>
        {
            // 8086/8088
            "add", "aam", "aas", "adc", "add", "and", "call",
            "cbw", "clc", "cbw", "clc", "cli", "cmc",
            "cmp", "cmpsb", "cmpsw", "cwd", "daa",
            "das", "dec", "div", "esc", "hlt", "idiv",
            "imul", "in", "inc", "int", "into", 
            "iret", "ja", "jae", "jb", "jbe", "jc", "je", "jg", "jge",
            "jl", "jle", "jna", "jnae", "jnb", "jnbe", "jnc", "jne",
            "jng", "jnge", "jnl", "jnle", "jno", "jnp", "jns",
            "jnz", "jo", "jp", "jpe", "jpo", "js", "jz",
            "jcxz", "jmp", "lahf", "lds", "lea", "les",
            "lock", "lodsb", "lodsw",
            "loop", "loope", "loopne","loopnz", "loopz",
            "mov", "movsb", "movsw", "mul",
            "neg", "nop", "not", "or", "out", "pop", "popf", "push",
            "pushf", "rcl", "rcr", "rep", "repe", "repne", "repnz",
            "repz", "ret", "retn", "retf", "rol", "ror",
            "sahf", "sal", "sar", "sbb", "scasb", "scasw",
            "shl", "shr", "stc", "std", "sti", "stosb",
            "stosw", "sub", "test", "wait", "xchg",
            "xlat", "xor",

            // 80816/80188
            "bound", "enter", "ins", "leave", "outs", "popa", "pusha",
            
            // 80286
            "arpl", "clts", "lar", "lgdt", "lidt", "lldt",
            "lmsw", "loadall", "lsl", "ltr", "sgdt",
            "sigdt", "sidt", "sldt", "smsw",
            "str", "verr", "verw",

            // 80386
            "bsf", "bsr", "bt", "btc", "btr", "bts",
            "cdq", "cmpsd", "cwde", "ibts", "insd", "iret",
            "jecxz", "lfs", "lgs", "lss", "lodsd",
            "loopw", "loopwe", "loopwne","loopwnz", "loopwz",
            "loopd", "loopde", "loopdne","loopdnz", "loopdz",
            "movsd", "movsx", "movzx", "outsd",
            "popad", "popfd", "pushad", "pushfd",
            "scasd",
            "seta", "setae", "setb", "setbe", "setc",
            "sete", "setg", "setge", "setl", "setle", "setna",
            "setnae", "setnb", "setnbe", "setnc", "setne",
            "setng", "setnge", "setnl", "setnle", "setno",
            "setnp", "setns", "setnz", "seto", "setp", "setpe",
            "setpo", "sets", "setz",
            "shld", "shrd", "stosd", "xbts",

            // 80486
            "bswap", "cmpxchg", "invd", "invlpg",
            "wbinvd", "xadd",

            // Pentium
            "cpuid", "cmpxchg8b", "rdmsr", "rdtsc",
            "wrmsr", "rsm", 

            // Pentium mmx
            "rdpmc",

            // AMD K6
            "syscall", "sysret",
            
            // Pentium Pro
            "cmova", "cmovae", "cmovb", "cmovbe",
            "cmovc", "cmove", "cmovg", "cmovge",
            "cmovl", "cmovle", "cmovna", "cmovnae",
            "cmovnb", "cmovnbe", "cmovnc", "cmovne",
            "cmovng", "cmovnge", "cmovnl", "cmovnle",
            "cmovno", "cmovnp", "cmovns", "cmovnz",
            "cmovo", "cmovp", "cmovpe", "cmovpo", "cmovs", "cmovz",
            "ud2",

            // Pentium II
            "sysenter", "sysexit"
            
        };

        public HashSet<string> SizeDirectives { get; } = new HashSet<string>
        {
            "byte", "word", "dword", "qword", "xmmword",
            "xmmword", "ymmword", "zmmword"
        };

        public HashSet<char> LiteralSuffixes = new HashSet<char>
        {
            'h', 'b', 'o'
        };
    }
}
