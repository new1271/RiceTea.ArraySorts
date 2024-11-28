namespace RiceTea.ArraySorts.Memory
{
    public unsafe interface IMemoryAllocator
    {
        void* AllocMemory(uint size);

        void FreeMemory(void* ptr);

        T[] AllocArray<T>(int size);

        void FreeArray<T>(T[] array);
    }
}
