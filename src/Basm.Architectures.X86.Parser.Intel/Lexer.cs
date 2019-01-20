using System;
using System.Collections.Generic;
using Basm.Architectures.Parser;
using Basm.Core.CodeAnalysis.Syntax;

namespace Basm.Architectures.X86.Parser.Intel
{
    public class Lexer : IAssemblyLexer
    {
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
            "ss",
        };

        public HashSet<string> InstructionSet { get; } = new HashSet<string>
        {
            "add", "aam", "aas", "adc",
            "mov", "nop", "push", "pop", "xor"
        };

        public HashSet<string> SizeDirective { get; } = new HashSet<string>
        {
            "byte", "word", "dword", "qword", "xmmword",
            "xmmword", "ymmword", "zmmword"
        };

        public HashSet<string> LiteralSuffixes = new HashSet<string>
        {
            "h", "b", "o"
        };
    }
}
