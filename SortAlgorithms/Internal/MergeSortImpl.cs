
using InlineMethod;

using SortAlgorithms.Config;
using SortAlgorithms.Memory;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SortAlgorithms.Internal
{
    internal static unsafe class MergeSortImpl
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(T[] array, IComparer<T> comparer) where T : unmanaged
        {
            fixed (T* ptr = array)
                MergeSortImpl<T>.Sort(ptr, ptr + array.Length, comparer);
        }
    }

    internal static unsafe class MergeSortImpl<T> where T : unmanaged
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;
            if (count <= 64L)
            {
                InsertionSortImpl<T>.Sort(ptr, ptrEnd, comparer);
                return;
            }
            T* pivot = ptr + (count >> 1);
            Sort(ptr, pivot, comparer);
            Sort(pivot, ptrEnd, comparer);
            Merge(ptr, pivot, ptrEnd, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        private static void Merge(T* ptr, T* pivot, T* ptrEnd, IComparer<T> comparer)
        {
            T left = *(pivot - 1);
            T right = *pivot;
            if (comparer.Compare(left, right) < 0)
                return;
            ptr = FindBound(ptr, pivot - 1, right, comparer);
            ptrEnd = FindBound(pivot + 1, ptrEnd, left, comparer);
            long count = ptrEnd - ptr;
            if (count <= 64L)
            {
                InsertionSortImpl<T>.Sort(ptr, ptrEnd, comparer);
                return;
            }
            uint size = unchecked((uint)(count * sizeof(T)));
            IMemoryAllocator allocator = SortAlgorithmConfig.MemoryAllocator;
            T* space = (T*)allocator.AllocMemory(size);
            Unsafe.CopyBlock(space, ptr, size);
            Merge(ptr, space, pivot, ptrEnd, comparer);
            allocator.FreeMemory(space);
        }

        [Inline(InlineBehavior.Remove)]
        private static void Merge(T* ptr, T* space, T* pivot, T* ptrEnd, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;

            T* spacePivot = space + (count >> 1);
            T* spaceEnd = space + count;

            T* iteratorLeft = space;
            T* iteratorRight = spacePivot;
            T* iterator = ptr;

            T left = *iteratorLeft, right = *iteratorRight;
            for (; iterator < ptrEnd && iteratorLeft < spacePivot && iteratorRight < spaceEnd; iterator++)
            {
                if (comparer.Compare(left, right) < 0)
                {
                    *iterator = left;
                    left = *++iteratorLeft;
                }
                else
                {
                    *iterator = right;
                    right = *++iteratorRight;
                }
            }

            count = ptrEnd - iterator;
            if (count > 0)
            {
                uint size = unchecked((uint)(count * sizeof(T)));
                if (iteratorLeft < spacePivot)
                    Unsafe.CopyBlock(iterator, iteratorLeft, size);
                else
                    Unsafe.CopyBlock(iterator, iteratorRight, size);
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static T* FindBound(T* iterator, T* ptrEnd, T reference, IComparer<T> comparer)
        {
            for (; iterator < ptrEnd; iterator++)
            {
                T item = *iterator;
                if (comparer.Compare(reference, item) < 0)
                    return iterator;
            }
            return ptrEnd;
        }
    }
}

