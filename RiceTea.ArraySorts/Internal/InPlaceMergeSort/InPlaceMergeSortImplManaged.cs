
using InlineMethod;

using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.ShellSort;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.InPlaceMergeSort
{
    internal static unsafe class InPlaceMergeSortImplManaged<T>
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
            SoreCore(list, startIndex, endIndex, count, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(IList<T> list, int startIndex, int endIndex, int count, IComparer<T> comparer)
        {
            SoreCore(list, startIndex, endIndex, count, comparer);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count <= 16)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
                return;
            }
            SoreCore(list, startIndex, endIndex, count, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SoreCore(IList<T> list, int startIndex, int endIndex, int count, IComparer<T> comparer)
        {
            int pivotIndex = startIndex + (count >> 1);
            SortInternal(list, startIndex, pivotIndex, comparer);
            SortInternal(list, pivotIndex, endIndex, comparer);
            Merge(list, startIndex, pivotIndex, endIndex, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        private static void Merge(IList<T> list, int startIndex, int pivotIndex, int endIndex, IComparer<T> comparer)
        {
            T left = list[pivotIndex - 1];
            T right = list[pivotIndex];
            if (comparer.Compare(left, right) < 0)
                return;
            int count = endIndex - startIndex;
            if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                return;
            do
            {
                bool isOdd = (count & 0b01) == 0b01;
                count >>= 1;
                if (isOdd)
                    count++;
                ShellSortImplManaged<T>.SortOnce(list, startIndex, endIndex, comparer, count);
            } while (count > 1);
        }
    }
}

