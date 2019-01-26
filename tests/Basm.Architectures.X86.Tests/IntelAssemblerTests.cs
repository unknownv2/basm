using System;
using System.Buffers;
using Basm.Architectures.X86.Assembler;
using Basm.Architectures.X86.Parser.Intel;
using Nerdbank.Streams;
using Xunit;

namespace Basm.Architectures.X86.Tests
{
    public class InterlAssemblerTests
    {
        [Fact]
        public void ShouldAssembleNopInstructionToBuffer()
        {
            const string instructionText = "NOP";
            const byte nopOpcode = 0x90;
            const int nopOpCodeLength = 1;
            var memory = new TestMemory { Address = 0 };
            var syntaxTree = IntelX86SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            var builder = new Sequence<byte>();

            Assert.Equal(instructionText, instruction.InstructionToken.Text);
            Assert.Empty(instruction.Operands);

            new KeystoneAssembler(memory).Emit(builder, instruction, new TestSymbolResolver());
            var instructionBuffer = builder.AsReadOnlySequence.ToArray();

            Assert.Equal(nopOpCodeLength, instructionBuffer.Length);
            Assert.Equal(nopOpcode, instructionBuffer[0]);
        }
    }
}
