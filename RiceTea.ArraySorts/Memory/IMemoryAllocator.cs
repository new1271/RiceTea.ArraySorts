namespace RiceTea.ArraySorts.Memory
{
    /// <summary>
    /// An interface used to allocate memory block or array if algorithm needed.
    /// </summary>
    public unsafe interface IMemoryAllocator
    {
        /// <summary>
        /// Allocates memory block with <paramref name="size"/> bytes
        /// </summary>
        /// <param name="size">The size of the memory block (in bytes)</param>
        /// <returns>A pointer pointed to memory block allocated</returns>
        void* AllocMemory(uint size);

        /// <summary>
        /// Frees the memory block allocated
        /// </summary>
        /// <param name="ptr">The pointer from <see cref="AllocMemory(uint)"/> call</param>
        void FreeMemory(void* ptr);

        /// <summary>
        /// Allocates array with <paramref name="size"/> bytes
        /// </summary>
        /// <typeparam name="T">The type of elements in array</typeparam>
        /// <param name="size">The size of the array</param>
        /// <returns>An array object with specific type</returns>
        T[] AllocArray<T>(int size);

        /// <summary>
        /// Frees the array object allocated
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array object from <see cref="AllocArray{T}(int)"/> call</param>
        void FreeArray<T>(T[] array);
    }
}
