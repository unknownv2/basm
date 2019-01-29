using System.Buffers;
using System.Linq;
using System.Text;
using Basm.Scripting;
using Basm.Core.CodeAnalysis.Syntax;
using Keystone;

namespace Basm.Architectures.X86.Assembler
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
            stream.Write(Assemble(instruction, resolver));
            return stream;
        }

        private string ConvertInstructionToString(InstructionStatementSyntax instruction, SymbolResolver resolver)
        {
            StringBuilder instructionBuilder = new StringBuilder();
            instructionBuilder.Append(instruction.InstructionToken.Text);
            if (instruction.Operands.Any())
            {
                int operandIndex = 0;
                instructionBuilder.Append(" ");
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
            using (var assembler = new Engine(Architecture.X86, Mode.X32))
            {
                var instructionText = ConvertInstructionToString(instruction, resolver);

                var encoded = assembler.Assemble(instructionText, _memory.Address);

                return encoded.Buffer;
            }
        }
    }
}
