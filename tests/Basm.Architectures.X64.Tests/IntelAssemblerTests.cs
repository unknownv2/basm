using System.Buffers;
using Basm.Architectures.X64.Assembler;
using Basm.Architectures.X64.Parser.Intel;
using Nerdbank.Streams;
using Xunit;

namespace Basm.Architectures.X64.Tests
{
    public class IntelAssemblerTests
    {
        [Fact]
        public void ShouldAssembleNopInstructionToBuffer()
        {
            const string instructionText = "NOP";
            const byte nopOpcode = 0x90;
            const int nopOpCodeLength = 1;
            var memory = new TestMemory {Address = 0};
            var syntaxTree = SyntaxTree.Parse(instructionText);
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
