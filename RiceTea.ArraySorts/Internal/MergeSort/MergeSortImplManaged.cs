
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
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            SortCore(list, startIndex, endIndex, comparer, ArraySortsConfig.DefaultMemoryAllocator);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void SortCore(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer, IMemoryAllocator allocator)
        {
            int count = endIndex - startIndex;
            if (count <= 64)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
                return;
            }
            int pivotIndex = startIndex + (count >> 1);
            SortCore(list, startIndex, pivotIndex, comparer, allocator);
            SortCore(list, pivotIndex, endIndex, comparer, allocator);
            Merge(list, startIndex, pivotIndex, endIndex, comparer, allocator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(IList<T> list, int startIndex, int pivotIndex, int endIndex, IComparer<T> comparer, IMemoryAllocator allocator)
        {
            T left = list[pivotIndex - 1];
            T right = list[pivotIndex];
            if (comparer.Compare(left, right) < 0)
                return;
            int count = endIndex - startIndex;
            if (count <= 64)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
                return;
            }
            T[] space = allocator.AllocArray<T>(count);
            MemoryUtils.CopyToArray(space, list, startIndex, 0, count);
            Merge(list, space, startIndex, pivotIndex, endIndex, comparer);
            allocator.FreeArray(space);
        }

        [Inline(InlineBehavior.Remove)]
        private static void Merge(IList<T> list, T[] space, int startIndex, int pivotIndex, int endIndex, IComparer<T> comparer)
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

