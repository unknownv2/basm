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

        [Theory]
        [InlineData("push 2", new byte[] { 0x6a, 0x02 })]
        [InlineData("push rax", new byte[] { 0x50 })]
        [InlineData("mov rax, 1", new byte[] { 0x48, 0xc7, 0xc0, 0x01, 0x00, 0x00, 0x00 })]
        [InlineData("mov dword ptr [rax], 2", new byte[] { 0xc7, 0x00, 0x02, 0x00, 0x00, 0x00 })]
        [InlineData("mov byte ptr [rax+4],8", new byte[] { 0xc6, 0x40, 0x04, 0x08})]
        [InlineData("mov byte [rax+4],8", new byte[] { 0xc6, 0x40, 0x04, 0x08 })]
        [InlineData("mov word ptr [rax+4],8", new byte[] { 0x66, 0xc7, 0x40, 0x04, 0x08, 0x00 })]
        [InlineData("mov word [rax+4],8", new byte[] { 0x66, 0xc7, 0x40, 0x04, 0x08, 0x00 })]
        [InlineData("mov dword ptr [rax+4],8", new byte[] { 0xc7, 0x40, 0x04, 0x08, 0x00, 0x00, 0x00 })]
        [InlineData("mov dword [rax+4],8", new byte[] { 0xc7, 0x40, 0x04, 0x08, 0x00, 0x00, 0x00 })]
        [InlineData("mov qword ptr [rax+4],8", new byte[] { 0x48, 0xc7, 0x40, 0x04, 0x08, 0x00, 0x00, 0x00 })]
        [InlineData("mov qword [rax+4],8", new byte[] { 0x48, 0xc7, 0x40, 0x04, 0x08, 0x00, 0x00, 0x00 })]

        public void ShouldAssembleInstructionToBuffer(string instructionText, byte[] opcodeBytes)
        {
            var memory = new TestMemory { Address = 0 };
            var syntaxTree = SyntaxTree.Parse(instructionText);
            var root = syntaxTree.Root;
            var instruction = root.InstructionStatement;
            var builder = new Sequence<byte>();

            new KeystoneAssembler(memory).Emit(builder, instruction, new TestSymbolResolver());
            var instructionBuffer = builder.AsReadOnlySequence.ToArray();

            Assert.Equal(opcodeBytes.Length, instructionBuffer.Length);
            Assert.Equal(opcodeBytes, instructionBuffer);
        }
    }
}
