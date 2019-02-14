using Basm.Core.Memory;

namespace Basm.Architectures.X64.Tests
{
    internal class TestMemory : IMemory
    {
        public ulong Address { get; set; }
    }
}
