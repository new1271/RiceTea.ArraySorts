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
            if (count <= 16)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImplManaged<T>.Sort(list, startIndex, endIndex, comparer);
                return;
            }
            if (SortCore(list, startIndex, endIndex, comparer))
                return;
            MergeSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, count, comparer);
        }

        //Code from https://stackoverflow.com/questions/55008384/can-quicksort-be-implemented-in-c-without-stack-and-recursion
        //Original from http://alienryderflex.com/quicksort/
        [MethodImpl(MethodImplOptions.NoInlining)] //因為 stackalloc 的關係, 這裡不能內聯 (避免出現 StackOverflowException )
        private static unsafe bool SortCore(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int* beg = stackalloc int[MAX_LEVELS];
            int* end = stackalloc int[MAX_LEVELS];
            int L, R;
            int i = 0;

            beg[0] = startIndex;
            end[0] = endIndex;
            while (i >= 0)
            {
                L = beg[i];
                R = end[i];
                if (R - L > 1)
                {
                    int M = L + ((R - L) >> 1);
                    T piv = list[M];
                    list[M] = list[L];

                    if (i == MAX_LEVELS - 1)
                        return false;
                    R--;
                    while (L < R)
                    {
                        while (comparer.Compare(list[R], piv) >= 0 && L < R)
                            R--;
                        if (L < R)
                            list[L++] = list[R];
                        while (comparer.Compare(list[L], piv) <= 0 && L < R)
                            L++;
                        if (L < R)
                            list[R--] = list[L];
                    }
                    list[L] = piv;
                    M = L + 1;
                    while (L > beg[i] && comparer.Compare(list[L - 1], piv) == 0)
                        L--;
                    while (M < end[i] && comparer.Compare(list[M], piv) == 0)
                        M++;
                    if (L - beg[i] > end[i] - M)
                    {
                        beg[i + 1] = M;
                        end[i + 1] = end[i];
                        end[i++] = L;
                    }
                    else
                    {
                        beg[i + 1] = beg[i];
                        end[i + 1] = L;
                        beg[i++] = M;
                    }
                }
                else
                {
                    i--;
                }
            }
            return true;
        }
    }
}