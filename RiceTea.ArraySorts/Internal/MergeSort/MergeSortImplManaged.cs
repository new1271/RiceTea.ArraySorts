
using InlineMethod;

using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Memory;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.MergeSort
{
    internal static unsafe class MergeSortImplManaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count <= 16)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
                return;
            }
            IMemoryAllocator allocator = ArraySortsConfig.MemoryAllocator;
            T[] space = allocator.AllocArray<T>(count);
            SortCore(list, space, startIndex, endIndex, count, comparer);
            allocator.FreeArray(space);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(IList<T> list, T[] space, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count <= 16)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
                return;
            }
            SortCore(list, space, startIndex, endIndex, count, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(IList<T> list, T[] space, int startIndex, int endIndex, int count, IComparer<T> comparer)
        {
            int pivotIndex = startIndex + (count >> 1);
            SortInternal(list, space, startIndex, pivotIndex, comparer);
            SortInternal(list, space, pivotIndex, endIndex, comparer);
            Merge(list, space, startIndex, pivotIndex, endIndex, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        private static void Merge(IList<T> list, T[] space, int startIndex, int pivotIndex, int endIndex, IComparer<T> comparer)
        {
            T left = list[pivotIndex - 1];
            T right = list[pivotIndex];
            if (comparer.Compare(left, right) < 0)
                return;
            int count = endIndex - startIndex;
            if (count <= 16)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
                return;
            }
            MemoryUtils.CopyToArray(space, list, startIndex, 0, count);
            MergeCore(list, space, startIndex, pivotIndex, endIndex, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        private static void MergeCore(IList<T> list, T[] space, int startIndex, int pivotIndex, int endIndex, IComparer<T> comparer)
        {
            int spacePivotIndex = pivotIndex - startIndex;
            int spaceEndIndex = endIndex - startIndex;

            int leftIndex = 0;
            int rightIndex = pivotIndex - startIndex;
            int sourceIndex = startIndex;

            T left = space[leftIndex], right = space[rightIndex];
            for (; sourceIndex < endIndex && leftIndex < spacePivotIndex && rightIndex < spaceEndIndex; sourceIndex++)
            {
                if (comparer.Compare(left, right) < 0)
                {
                    list[sourceIndex] = left;
                    if (++leftIndex >= spacePivotIndex)
                    {
                        sourceIndex++;
                        break;
                    }
                    left = space[leftIndex];
                }
                else
                {
                    list[sourceIndex] = right;
                    if (++rightIndex >= spaceEndIndex)
                    {
                        sourceIndex++;
                        break;
                    }
                    right = space[rightIndex];
                }
            }

            int count = endIndex - sourceIndex;
            if (count > 0)
            {
                if (leftIndex < spacePivotIndex)
                    MemoryUtils.CopyToList(list, space, leftIndex, sourceIndex, count);
                else
                    MemoryUtils.CopyToList(list, space, rightIndex, sourceIndex, count);
            }
        }
    }
}

