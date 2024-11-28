using System;
using System.Runtime.InteropServices;

#if NET5_0_OR_GREATER
using System.Buffers;
#endif

namespace RiceTea.ArraySorts.Memory
{
    internal sealed class DefaultMemoryAllocator : IMemoryAllocator
    {
        public T[] AllocArray<T>(int size)
        {
#if NET5_0_OR_GREATER
            return ArrayPool<T>.Shared.Rent(size);
#else
            return new T[size];
#endif
        }

        public unsafe void* AllocMemory(uint size)
        {
            return (void*)Marshal.AllocHGlobal(new IntPtr(size));
        }

        public void FreeArray<T>(T[] array)
        {
#if NET5_0_OR_GREATER
            ArrayPool<T>.Shared.Return(array);
#endif
        }

        public unsafe void FreeMemory(void* ptr)
        {
            Marshal.FreeHGlobal(new IntPtr(ptr));
        }
    }
}
