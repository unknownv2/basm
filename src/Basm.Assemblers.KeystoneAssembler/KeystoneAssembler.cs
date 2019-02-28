using System.Buffers;
using System.Linq;
using System.Text;
using Basm.Core.CodeAnalysis.Syntax;
using Basm.Core.Memory;
using Basm.Scripting;
using Keystone;

namespace Basm.Assemblers.KeystoneAssembler
{
    public class KeystoneAssembler
    {
        private readonly IMemory _memory;
        private readonly Mode _mode;
        private readonly Architecture _architecture;

        public KeystoneAssembler(Architecture architecture, Mode mode, IMemory memory)
        {
            _architecture = architecture;
            _mode = mode;
            _memory = memory;
        }

        public IBufferWriter<byte> Emit(IBufferWriter<byte> stream, InstructionStatementSyntax instruction, SymbolResolver resolver)
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
            using (var assembler = new Engine(_architecture, _mode))
            {
                var instructionText = ConvertInstructionToString(instruction, resolver);
                var encoded = assembler.Assemble(instructionText, _memory.Address);
                return encoded.Buffer;
            }
        }
    }
}
