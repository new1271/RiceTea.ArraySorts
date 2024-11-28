using InlineMethod;

using RiceTea.ArraySorts.Memory;

using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Config
{
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

        public static IMemoryAllocator DefaultMemoryAllocator
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _defaultAllocator;
        }

        public static IMemoryAllocator MemoryAllocator
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _allocator;
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _allocator = value ?? _defaultAllocator;
        }
    }
}
