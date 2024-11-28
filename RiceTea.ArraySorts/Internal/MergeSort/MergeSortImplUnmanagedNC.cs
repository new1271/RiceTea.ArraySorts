
using InlineMethod;
using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Memory;
using RiceTea.Numerics;

using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.MergeSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class MergeSortImplUnmanagedNC<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd)
        {
            SortCore(ptr, ptrEnd, ArraySortsConfig.MemoryAllocator);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, IMemoryAllocator allocator)
        {
            long count = ptrEnd - ptr;
            if (count <= 64)
            {
                if (count < 2L || SortUtils.ShortCircuitSort(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            T* pivot = ptr + (count >> 1);
            SortCore(ptr, pivot, allocator);
            SortCore(pivot, ptrEnd, allocator);
            Merge(ptr, pivot, ptrEnd, allocator);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(T* ptr, T* pivot, T* ptrEnd, IMemoryAllocator allocator)
        {
            T left = *(pivot - 1);
            T right = *pivot;
            if (new PackedPrimitive<T>(left) < right)
                return;
            long count = ptrEnd - ptr;
            if (count <= 64)
            {
                if (count < 2L || SortUtils.ShortCircuitSort(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            uint size = unchecked((uint)(count * sizeof(T)));
            T* space = (T*)allocator.AllocMemory(size);
            UnsafeHelper.CopyBlock(space, ptr, size);
            Merge(ptr, pivot, ptrEnd, space);
            allocator.FreeMemory(space);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(T* ptr, T* pivot, T* ptrEnd, T* space)
        {
            T* spacePivot = space + (pivot - ptr);
            T* spaceEnd = space + (ptrEnd - ptr);

            T* iteratorLeft = space;
            T* iteratorRight = spacePivot;
            T* iterator = ptr;

            T left = *iteratorLeft, right = *iteratorRight;
            for (; iterator < ptrEnd && iteratorLeft < spacePivot && iteratorRight < spaceEnd; iterator++)
            {
                if (new PackedPrimitive<T>(left) < right)
                {
                    *iterator = left;
                    if (++iteratorLeft >= spacePivot)
                    {
                        iterator++;
                        break;
                    }
                    left = *iteratorLeft;
                }
                else
                {
                    *iterator = right;
                    if (++iteratorRight >= spaceEnd)
                    {
                        iterator++;
                        break;
                    }
                    right = *iteratorRight;
                }
            }

            long count = ptrEnd - iterator;
            if (count > 0)
            {
                uint size = unchecked((uint)(count * sizeof(T)));
                if (iteratorLeft < spacePivot)
                    UnsafeHelper.CopyBlock(iterator, iteratorLeft, size);
                else
                    UnsafeHelper.CopyBlock(iterator, iteratorRight, size);
            }
        }
    }
}

