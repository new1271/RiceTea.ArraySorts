
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
            long count = ptrEnd - ptr;
            if (count <= 16)
            {
                if (count < 2L || SortUtils.ShortCircuitSortNC(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            SortCore(ptr, ptrEnd, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(T* ptr, T* ptrEnd, long count)
        {
            SortCore(ptr, ptrEnd, count);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(T* ptr, T* ptrEnd, T* space)
        {
            long count = ptrEnd - ptr;
            if (count <= 16)
            {
                if (count < 2L || SortUtils.ShortCircuitSortNC(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            SortCore(ptr, ptrEnd, space, count);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, long count)
        {
            IMemoryAllocator allocator = ArraySortsConfig.MemoryAllocator;
            T* space = (T*)allocator.AllocMemory(unchecked((uint)(count * sizeof(T))));
            SortCore(ptr, ptrEnd, space, count);
            allocator.FreeMemory(space);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, T* space, long count)
        {
            T* pivot = ptr + (count >> 1);
            SortInternal(ptr, pivot, space);
            SortInternal(pivot, ptrEnd, space);
            Merge(ptr, pivot, ptrEnd, space);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(T* ptr, T* pivot, T* ptrEnd, T* space)
        {
            T left = *(pivot - 1);
            T right = *pivot;
            if (new PackedPrimitive<T>(left) < right)
                return;
            long count = ptrEnd - ptr;
            if (count <= 16)
            {
                if (count < 2L || SortUtils.ShortCircuitSortNC(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            UnsafeHelper.CopyBlock(space, ptr, unchecked((uint)(count * sizeof(T))));
            MergeCore(ptr, pivot, ptrEnd, space);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MergeCore(T* ptr, T* pivot, T* ptrEnd, T* space)
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

