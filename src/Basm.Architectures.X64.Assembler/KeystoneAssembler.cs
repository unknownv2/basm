using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Text;
using Basm.Scripting;
using Basm.Core.CodeAnalysis.Binding;
using Basm.Core.CodeAnalysis.Syntax;
using Keystone;

namespace Basm.Architectures.X64.Assembler
{
    public class KeystoneAssembler
    {
        private readonly IMemory _memory;

        public KeystoneAssembler(IMemory memory)
        {
            _memory = memory;
        }

        public object Emit(IBufferWriter<byte> stream, InstructionStatementSyntax instruction, SymbolResolver resolver)
        {
            var data = Assemble(instruction, resolver);
            stream.Write(data);
            return null;
        }

        private string ConvertInstructionToString(InstructionStatementSyntax instruction, SymbolResolver resolver)
        {
            StringBuilder instructionBuilder = new StringBuilder();
            instructionBuilder.Append(instruction.InstructionToken.Text);
            if (instruction.Operands.Any())
            {
                instructionBuilder.Append(" ");
                int operandIndex = 0;
                foreach (var operand in instruction.Operands)
                {
                    instructionBuilder.Append(resolver.ResolveSymbol(operand));
                    if (++operandIndex != instruction.Operands.Count())
                    {
                        instructionBuilder.Append(",");
                    }
                }
            }
            return instructionBuilder.ToString();
        }

        /// <summary>
        /// Assemble an instruction statement into a binary buffer.
        /// </summary>
        /// <param name="instruction">The instruction statement to assemble.</param>
        /// <param name="resolver">Resolves any symbols found in the instruction statement.</param>
        /// <returns>The encoded instruction to binary.</returns>
        private byte[] Assemble(InstructionStatementSyntax instruction, SymbolResolver resolver)
        {
            using (var assembler = new Engine(Architecture.X86, Mode.X64))
            {
                var instructionString = ConvertInstructionToString(instruction, resolver);

                var encoded = assembler.Assemble(instructionString, _memory.Address);

                return encoded.Buffer;
            }
        }
    }
}
