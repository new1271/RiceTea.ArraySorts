
using InlineMethod;

using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.ShellSort;
using RiceTea.ArraySorts.Memory;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.MergeSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class MergeSortImplUnmanaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;
            if (count < 2L || SortUtils.OptimizeSort(ptr, ptrEnd, count, comparer))
                return;
            SortCore(ptr, ptrEnd, count, comparer);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(T* ptr, T* ptrEnd, T* space, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;
            if (count < 2L || SortUtils.OptimizeSort(ptr, ptrEnd, count, comparer))
                return;
            SortCore(ptr, ptrEnd, space, count, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, long count, IComparer<T> comparer)
        {
            IMemoryAllocator allocator = ArraySortsConfig.MemoryAllocator;
            T* space = (T*)allocator.AllocMemory(new IntPtr(count * sizeof(T)));
            SortCore(ptr, ptrEnd, space, count, comparer);
            allocator.FreeMemory(space);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, T* space, long count, IComparer<T> comparer)
        {
            T* pivot = ptr + (count >> 1);
            SortInternal(ptr, pivot, space, comparer);
            SortInternal(pivot, ptrEnd, space, comparer);
            Merge(ptr, pivot, ptrEnd, space, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(T* ptr, T* pivot, T* ptrEnd, T* space, IComparer<T> comparer)
        {
            T left = *(pivot - 1);
            T right = *pivot;
            if (comparer.Compare(left, right) < 0)
                return;
            long count = ptrEnd - ptr;
            if (count < 2 || SortUtils.ShortCircuitSort(ptr, count, comparer))
                return;
            UnsafeHelper.CopyBlock(space, ptr, unchecked((uint)(count * sizeof(T))));
            MergeCore(ptr, pivot, ptrEnd, space, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MergeCore(T* ptr, T* pivot, T* ptrEnd, T* space, IComparer<T> comparer)
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

