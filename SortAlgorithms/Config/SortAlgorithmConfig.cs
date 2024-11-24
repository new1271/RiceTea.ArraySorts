using SortAlgorithms.Memory;

namespace SortAlgorithms.Config
{
    public static class SortAlgorithmConfig
    {
        static SortAlgorithmConfig()
        {
            MemoryAllocator = DefaultMemoryAllocator = new DefaultMemoryAllocator();
        }

        public static IMemoryAllocator DefaultMemoryAllocator { get; }

        public static IMemoryAllocator MemoryAllocator { get; set; }
    }
}
