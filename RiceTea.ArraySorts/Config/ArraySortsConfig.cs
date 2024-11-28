using RiceTea.ArraySorts.Memory;

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
            DefaultMemoryAllocator allocator = new DefaultMemoryAllocator();
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
