using RiceTea.ArraySorts.Memory;

using System;
using System.Runtime.InteropServices;
using System.Security;

#if NET5_0_OR_GREATER
using System.Buffers;
#endif

namespace RiceTea.ArraySorts.Benchmark
{
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class Win32MemoryAllocator : IMemoryAllocator
    {
        [DllImport("kernel32", EntryPoint = nameof(GetProcessHeap), CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcessHeap();

        [DllImport("kernel32", EntryPoint = nameof(HeapAlloc), CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern void* HeapAlloc(IntPtr hHeap, int dwFlags, UIntPtr size);

        [DllImport("kernel32", EntryPoint = nameof(HeapFree), CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        private static extern void HeapFree(IntPtr hHeap, int dwFlags, void* ptr);

        private readonly IntPtr _heap = GetProcessHeap();

        public T[] AllocArray<T>(int size)
        {
#if NET5_0_OR_GREATER
            return ArrayPool<T>.Shared.Rent(size);
#else
            return new T[size];
#endif
        }

        public void* AllocMemory(uint size)
        {
            //Console.WriteLine("Alloc " + size + " bytes memory");
            return HeapAlloc(_heap, 0, new UIntPtr(size));
        }

        public void FreeArray<T>(T[] array)
        {
#if NET5_0_OR_GREATER
            ArrayPool<T>.Shared.Return(array);
#endif
        }

        public void FreeMemory(void* ptr)
        {
            HeapFree(_heap, 0, ptr);
        }
    }
}
