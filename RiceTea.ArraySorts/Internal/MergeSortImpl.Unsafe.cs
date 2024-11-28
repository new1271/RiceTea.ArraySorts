
using InlineMethod;

using RiceTea.ArraySorts.Memory;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class MergeSortImplUnsafe<T>
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer, IMemoryAllocator allocator)
        {
            long count = ptrEnd - ptr;
            if (count <= 64L)
            {
                if (count < 2L || SortUtils.ShortCircuitSort(ptr, count, comparer))
                    return;
                BinaryInsertionSortImplUnsafe<T>.SortWithoutCheck(ptr, ptrEnd, comparer);
                return;
            }
            T* pivot = ptr + (count >> 1);
            Sort(ptr, pivot, comparer, allocator);
            Sort(pivot, ptrEnd, comparer, allocator);
            Merge(ptr, pivot, ptrEnd, comparer, allocator);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(T* ptr, T* pivot, T* ptrEnd, IComparer<T> comparer, IMemoryAllocator allocator)
        {
            T left = *(pivot - 1);
            T right = *pivot;
            if (comparer.Compare(left, right) < 0)
                return;
            ptr = SortUtils.BinarySearchForNGI(ptr, pivot - 1, right, comparer);
            ptrEnd = SortUtils.BinarySearchForNGI(pivot + 1, ptrEnd, left, comparer);
            long count = ptrEnd - ptr;
            if (count <= 64)
            {
                if (count < 2L || SortUtils.ShortCircuitSort(ptr, count, comparer))
                    return;
                BinaryInsertionSortImplUnsafe<T>.SortWithoutCheck(ptr, ptrEnd, comparer);
                return;
            }
            uint size = unchecked((uint)(count * sizeof(T)));
            T* space = (T*)allocator.AllocMemory(size);
            UnsafeHelper.CopyBlock(space, ptr, size);
            Merge(ptr, pivot, ptrEnd, space, comparer);
            allocator.FreeMemory(space);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(T* ptr, T* pivot, T* ptrEnd, T* space, IComparer<T> comparer)
        {
            T* spacePivot = space + (pivot - ptr);
            T* spaceEnd = space + (ptrEnd - ptr);

            T* iteratorLeft = space;
            T* iteratorRight = spacePivot;
            T* iterator = ptr;

            T left = *iteratorLeft, right = *iteratorRight;
            for (; iterator < ptrEnd && iteratorLeft < spacePivot && iteratorRight < spaceEnd; iterator++)
            {
                if (comparer.Compare(left, right) < 0)
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

