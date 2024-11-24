using System;
using System.Runtime.InteropServices;

namespace SortAlgorithms.Memory
{
    internal sealed class DefaultMemoryAllocator : IMemoryAllocator
    {
        public unsafe void* AllocMemory(uint size)
        {
            return (void*)Marshal.AllocHGlobal(new IntPtr(size));
        }

        public unsafe void FreeMemory(void* ptr)
        {
            Marshal.FreeHGlobal(new IntPtr(ptr));
        }
    }
}
