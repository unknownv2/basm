using System;
using System.ServiceModel;

namespace Binarysharp.MemoryManagement.Assembly.Assembler
{
    /// <summary>
    /// Interface defining an assembler that communicates using WCF technologies.
    /// </summary>
    [ServiceContract]
    public interface IHostedAssembler
    {
        /// <summary>
        /// Assemble the specified assembly code at a base address.
        /// </summary>
        /// <param name="asm">The assembly code.</param>
        /// <param name="baseAddress">The address where the code is rebased.</param>
        /// <returns>An array of bytes containing the assembly code.</returns>
        [OperationContract]
        byte[] Assemble(string asm, IntPtr baseAddress);
    }
}
