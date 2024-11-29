using InlineMethod;

using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.InsertionSort;
using RiceTea.ArraySorts.Internal.MergeSort;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.QuickSort
{
    internal static class QuickSortImplManaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count <= 16)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.Sort(list, startIndex, endIndex, comparer);
                return;
            }
            SortCore(list, startIndex, endIndex - 1, comparer, Intr);
            InsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(IList<T> list, int startIndex, int lastIndex, IComparer<T> comparer, int depth)
        {
            int count = lastIndex - startIndex + 1;
            if (count <= 16)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.Sort(list, startIndex, lastIndex + 1, comparer);
                return;
            }
            if (depth >= 32) //如果堆疊深度大於 32，用合併排序對子序列做排序
            {
                MergeSortImplManaged<T>.SortWithoutCheck(list, startIndex, lastIndex + 1, count, comparer);
                return;
            }
            SortCore(list, startIndex, lastIndex, comparer, depth);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(IList<T> list, int startIndex, int lastIndex, IComparer<T> comparer, int depth)
        {
            int leftIndex = startIndex;
            int rightIndex = lastIndex;

            T pivot = list[startIndex];

            while (leftIndex <= rightIndex)
            {
                while (comparer.Compare(list[leftIndex], pivot) < 0)
                {
                    leftIndex++;
                }

                while (comparer.Compare(list[rightIndex], pivot) > 0)
                {
                    rightIndex--;
                }

                if (leftIndex <= rightIndex)
                {
                    (list[rightIndex], list[leftIndex]) = (list[leftIndex], list[rightIndex]);
                    leftIndex++;
                    rightIndex--;
                }
            }

            if (startIndex < rightIndex)
                SortInternal(list, startIndex, rightIndex, comparer, depth + 1);

            if (leftIndex < lastIndex)
                SortInternal(list, leftIndex, lastIndex, comparer, depth + 1);
        }
    }
}