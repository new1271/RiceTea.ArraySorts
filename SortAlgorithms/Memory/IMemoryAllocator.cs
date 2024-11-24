﻿namespace SortAlgorithms.Memory
{
    public unsafe interface IMemoryAllocator
    {
        void* AllocMemory(uint size);

        void FreeMemory(void* ptr);
    }
}
