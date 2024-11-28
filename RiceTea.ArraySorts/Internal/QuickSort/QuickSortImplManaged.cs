using RiceTea.ArraySorts.Internal.BinaryInsertionSort;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.QuickSort
{
    internal static class QuickSortImplManaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            SortInternal(list, startIndex, endIndex - 1, comparer);
        }

        //From https://code-maze.com/csharp-quicksort-algorithm/
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(IList<T> list, int startIndex, int lastIndex, IComparer<T> comparer)
        {
            int count = lastIndex - startIndex + 1;
            if (count <= 64)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.Sort(list, startIndex, lastIndex + 1, comparer);
                return;
            }

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
                SortInternal(list, startIndex, rightIndex, comparer);

            if (leftIndex < lastIndex)
                SortInternal(list, leftIndex, lastIndex, comparer);
        }
    }
}