﻿using System.Buffers;
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
            const string instructionText = "nop";
            const byte nopOpcode = 0x90;
            var memory = new TestMemory { Address = 0 };
            var builder = new Sequence<byte>();

            var instruction = IntelX64SyntaxTree.Parse(instructionText).Root.InstructionStatement;

            Assert.Equal(instructionText, instruction.InstructionToken.Text);
            Assert.Empty(instruction.Operands);

            new KeystoneAssembler(memory).Emit(builder, instruction, new TestSymbolResolver());
            var instructionBuffer = builder.AsReadOnlySequence.ToArray();

            Assert.Single(instructionBuffer);
            Assert.Equal(nopOpcode, instructionBuffer[0]);
        }

        [Theory]
        [InlineData("push 2", new byte[] { 0x6a, 0x02 })]
        [InlineData("push rax", new byte[] { 0x50 })]
        [InlineData("mov rax, 1", new byte[] { 0x48, 0xc7, 0xc0, 0x01, 0x00, 0x00, 0x00 })]
        [InlineData("mov dword ptr [rax], 2", new byte[] { 0xc7, 0x00, 0x02, 0x00, 0x00, 0x00 })]
        [InlineData("mov byte ptr [rax+4],8", new byte[] { 0xc6, 0x40, 0x04, 0x08})]
        [InlineData("mov word ptr [rax+4],8", new byte[] { 0x66, 0xc7, 0x40, 0x04, 0x08, 0x00 })]
        [InlineData("mov dword ptr [rax+4],8", new byte[] { 0xc7, 0x40, 0x04, 0x08, 0x00, 0x00, 0x00 })]
        [InlineData("mov qword ptr [rax+4],8", new byte[] { 0x48, 0xc7, 0x40, 0x04, 0x08, 0x00, 0x00, 0x00 })]
        [InlineData("mov byte ptr [rax-4],8", new byte[] { 0xc6, 0x40, 0xfc, 0x08 })]
        [InlineData("mov byte ptr [rax+rbx*4],8", new byte[] { 0xc6, 0x04, 0x98, 0x08 })]
        [InlineData("mov byte ptr [rax+rbx*4+9],8", new byte[] { 0xc6, 0x44, 0x98, 0x09, 0x08 })]
        [InlineData("mov byte ptr [rax+rbx*4+9],r9b", new byte[] { 0x44, 0x88, 0x4c, 0x98, 0x09 })]
        [InlineData("mov byte ptr [r8+r10*4+9],r13b", new byte[] { 0x47, 0x88, 0x6c, 0x90, 0x09 })]

        public void ShouldAssembleInstructionToBuffer(string inputText, byte[] expectedBytes)
        {
            var memory = new TestMemory { Address = 0 };
            var builder = new Sequence<byte>();

            var instruction = IntelX64SyntaxTree.Parse(inputText).Root.InstructionStatement;

            new KeystoneAssembler(memory).Emit(builder, instruction, new TestSymbolResolver());
            var instructionBuffer = builder.AsReadOnlySequence.ToArray();

            Assert.Equal(expectedBytes.Length, instructionBuffer.Length);
            Assert.Equal(expectedBytes, instructionBuffer);
        }
    }
}
