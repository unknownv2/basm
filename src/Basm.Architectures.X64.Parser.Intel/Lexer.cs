using System.Collections.Generic;
using Basm.Core.CodeAnalysis.Text;

namespace Basm.Architectures.X64.Parser.Intel
{
    public class Lexer : X86.Parser.Intel.Lexer
    {
        public Lexer(SourceText text) : base(text)
        {
            Extend();
        }

        /// <summary>
        /// Extend the base x86 architecture.
        /// </summary>
        private void Extend()
        {
            // Extend the base x86 register set.
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
                "cdqe", "cqo", "cmpsq", "cmpxchg16b",
                "iretq", "jrcxz", "lodsq", "movsxd",
                "popfq", "pushfq", "rdtscp", "scasq",
                "stosq", "swapgs"
            });
        }
    }
}
