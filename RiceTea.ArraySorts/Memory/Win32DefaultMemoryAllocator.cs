using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RiceTea.ArraySorts.Memory
{
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class Win32DefaultMemoryAllocator : DefaultMemoryAllocator
    {
        [DllImport("kernel32", EntryPoint = nameof(GetProcessHeap), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr GetProcessHeap();

        [DllImport("kernel32", EntryPoint = nameof(HeapAlloc), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void* HeapAlloc(IntPtr hHeap, int dwFlags, IntPtr size);

        [DllImport("kernel32", EntryPoint = nameof(HeapFree), CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void HeapFree(IntPtr hHeap, int dwFlags, void* ptr);

        private readonly IntPtr _heap = GetProcessHeap();

        public override void* AllocMemory(IntPtr size)
        {
            return HeapAlloc(_heap, 0, size);
        }

        public override void FreeMemory(void* ptr)
        {
            HeapFree(_heap, 0, ptr);
        }
    }
}
