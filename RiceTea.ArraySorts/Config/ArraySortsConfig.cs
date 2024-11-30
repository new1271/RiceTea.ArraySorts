using RiceTea.ArraySorts.Memory;

using System;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Config
{
    /// <summary>
    /// The configuation for sorting algorithms in <see cref="ArraySorts"/>
    /// </summary>
    public static class ArraySortsConfig
    {
        private static readonly IMemoryAllocator _defaultAllocator;

        private static IMemoryAllocator _allocator;

        static ArraySortsConfig()
        {
            IMemoryAllocator allocator;
#if NET5_0_OR_GREATER
            if (OperatingSystem.IsWindows())
                allocator = new Win32DefaultMemoryAllocator();
            else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
                allocator = new UnixDefaultMemoryAllocator();
            else
                allocator = new DefaultMemoryAllocator();
#else
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                    allocator = new Win32DefaultMemoryAllocator();
                    break;
                case PlatformID.Unix:
                case PlatformID.MacOSX:
                    allocator = new UnixDefaultMemoryAllocator();
                    break;
                default:
                    allocator = new DefaultMemoryAllocator();
                    break;
            }
#endif
            _defaultAllocator = allocator;
            _allocator = allocator;
        }

        /// <summary>
        /// Default <see cref="IMemoryAllocator"/> implementation.
        /// </summary>
        public static IMemoryAllocator DefaultMemoryAllocator
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _defaultAllocator;
        }

        /// <summary>
        /// The <see cref="IMemoryAllocator"/> implementation used in sorting algorithms.
        /// </summary>
        public static IMemoryAllocator MemoryAllocator
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _allocator;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _allocator = value ?? _defaultAllocator;
        }
    }
}
