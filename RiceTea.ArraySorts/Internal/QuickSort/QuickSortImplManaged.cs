using InlineMethod;

using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.MergeSort;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.QuickSort
{
    internal static class QuickSortImplManaged<T>
    {
        private const int MAX_LEVELS = 128;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count < 2 || SortUtils.OptimizeSort(list, startIndex, endIndex, count, comparer))
                return;
            SortCore(list, startIndex, endIndex - 1, comparer);
        }

        //Code from https://stackoverflow.com/questions/33884057/quick-sort-stackoverflow-error-for-large-arrays
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe void SortCore(IList<T> list, int startIndex, int lastIndex, IComparer<T> comparer)
        {
            while (startIndex < lastIndex)
            {
                int pivotIndex = Partition(list, startIndex, lastIndex, comparer);
                if (pivotIndex - startIndex <= lastIndex - (pivotIndex + 1))
                {
                    SortCore(list, startIndex, pivotIndex, comparer);
                    startIndex = pivotIndex + 1;
                }
                else
                {
                    SortCore(list, pivotIndex + 1, lastIndex, comparer);
                    lastIndex = pivotIndex;
                }
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Partition(IList<T> list, int startIndex, int lastIndex, IComparer<T> comparer)
        {
            T pivot = list[startIndex];
            int leftIndex = startIndex - 1;
            int rightIndex = lastIndex + 1;

            while (true)
            {
                while (++leftIndex < lastIndex && comparer.Compare(list[leftIndex], pivot) < 0) ;
                while (--rightIndex > startIndex && comparer.Compare(list[rightIndex], pivot) > 0) ;

                if (leftIndex < rightIndex)
                {
                    (list[rightIndex], list[leftIndex]) = (list[leftIndex], list[rightIndex]);
                    continue;
                }
                return rightIndex;
            }
        }
    }
}