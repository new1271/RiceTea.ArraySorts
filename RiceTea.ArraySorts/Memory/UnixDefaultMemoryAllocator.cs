using System;
using System.Runtime.InteropServices;
using System.Security;

namespace RiceTea.ArraySorts.Memory
{
    [SuppressUnmanagedCodeSecurity]
    internal sealed unsafe class UnixDefaultMemoryAllocator : DefaultMemoryAllocator
    {
        [DllImport("libc", EntryPoint = nameof(malloc), CallingConvention = CallingConvention.Cdecl)]
        private static extern void* malloc(IntPtr size);

        [DllImport("libc", EntryPoint = nameof(free), CallingConvention = CallingConvention.Cdecl)]
        private static extern void free(void* ptr);

        public override void* AllocMemory(IntPtr size)
        {
            return malloc(size);
        }

        public override void FreeMemory(void* ptr)
        {
            free(ptr);
        }
    }
}
