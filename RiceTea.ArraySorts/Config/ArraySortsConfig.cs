using RiceTea.ArraySorts.Memory;

namespace RiceTea.ArraySorts.Config
{
    public static class ArraySortsConfig
    {
        static ArraySortsConfig()
        {
            MemoryAllocator = DefaultMemoryAllocator = new DefaultMemoryAllocator();
        }

        public static IMemoryAllocator DefaultMemoryAllocator { get; }

        public static IMemoryAllocator MemoryAllocator { get; set; }
    }
}
