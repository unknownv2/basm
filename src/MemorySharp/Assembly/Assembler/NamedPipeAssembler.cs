using System;
using Binarysharp.MemoryManagement.Helpers;

namespace Binarysharp.MemoryManagement.Assembly.Assembler
{
    /// <summary>
    /// Implementation of an assembler relying on fasm proxy.
    /// </summary>
    public class NamedPipeAssembler : IAssembler
    {
        /// <summary>
        /// Assemble the specified assembly code.
        /// </summary>
        /// <param name="asm">The assembly code.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        public byte[] Assemble(string asm)
        {
            return Assemble(asm, IntPtr.Zero);
        }

        /// <summary>
        /// Assemble the specified assembly code at a base address.
        /// </summary>
        /// <param name="asm">The assembly code.</param>
        /// <param name="baseAddress">The address where the code is rebased.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        public byte[] Assemble(string asm, IntPtr baseAddress)
        {
            // Rebase the code
            asm = $"use32\norg 0x{baseAddress.ToInt64():X8}\n" + asm;

            return Singleton<NamedPipeFasmProxy>.Instance.HostedAssembler.Assemble(asm, baseAddress);
        }
    }
}
