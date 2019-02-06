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
            const string instructionText = "nop";
            const byte nopOpcode = 0x90;
            var memory = new TestMemory { Address = 0 };
            var builder = new Sequence<byte>();

            var instruction = IntelX86SyntaxTree.Parse(instructionText).Root.InstructionStatement;

            Assert.Equal(instructionText, instruction.InstructionToken.Text);
            Assert.Empty(instruction.Operands);

            new KeystoneAssembler(memory).Emit(builder, instruction, new TestSymbolResolver());
            var instructionBuffer = builder.AsReadOnlySequence.ToArray();

            Assert.Single(instructionBuffer);
            Assert.Equal(nopOpcode, instructionBuffer[0]);
        }

        [Theory]
        [InlineData("push 2", new byte[] { 0x6a, 0x02 })]
        [InlineData("push eax", new byte[] { 0x50 })]
        [InlineData("mov eax, 1", new byte[] { 0xb8, 0x01, 0x00, 0x00, 0x00 })]
        [InlineData("mov dword ptr [eax], 2", new byte[] { 0xc7, 0x00, 0x02, 0x00, 0x00, 0x00 })]
        [InlineData("mov byte ptr [eax+4],8", new byte[] { 0xc6, 0x40, 0x04, 0x08 })]
        [InlineData("mov dword ptr [eax+4],8", new byte[] { 0xc7, 0x40, 0x04, 0x08, 0x00, 0x00, 0x00 })]
        [InlineData("mov byte ptr [eax-4],8", new byte[] { 0xc6, 0x40, 0xfc, 0x08 })]
        [InlineData("mov byte ptr [eax+ebx*4],8", new byte[] { 0xc6, 0x04, 0x98, 0x08 })]
        [InlineData("mov byte ptr [eax+ebx*4+9],8", new byte[] { 0xc6, 0x44, 0x98, 0x09, 0x08 })]
        [InlineData("mov byte ptr [eax+ebx*4+9],cl", new byte[] { 0x88, 0x4c, 0x98, 0x09 })]
        [InlineData("mov byte ptr [edx+ecx*4+9],bl", new byte[] {  0x88, 0x5c, 0x8a, 0x09 })]

        public void ShouldAssembleInstructionToBuffer(string inputText, byte[] expectedBytes)
        {
            var memory = new TestMemory { Address = 0 };
            var builder = new Sequence<byte>();

            var instruction = IntelX86SyntaxTree.Parse(inputText).Root.InstructionStatement;

            new KeystoneAssembler(memory).Emit(builder, instruction, new TestSymbolResolver());
            var instructionBuffer = builder.AsReadOnlySequence.ToArray();

            Assert.Equal(expectedBytes.Length, instructionBuffer.Length);
            Assert.Equal(expectedBytes, instructionBuffer);
        }
    }
}
