using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.HeapSort;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.IntroSort
{
    internal sealed class IntroSortImplManaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count < 2 || OptimizeSort(list, startIndex, endIndex, count, comparer))
                return;
            //預測堆疊深度
            int bestStackDeep = MathHelper.Log2(unchecked((uint)count));
            if (bestStackDeep > 32 || !TryQuickSort(list, startIndex, endIndex, comparer, bestStackDeep << 1))
                HeapSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool OptimizeSort(IList<T> list, int startIndex, int endIndex, int count, IComparer<T> comparer)
        {
            if (count > 16)
                return SortUtils.CheckPattern(list, startIndex, endIndex, comparer);
            if (SortUtils.ShortCircuitSort(list, startIndex, count, comparer) || SortUtils.CheckPattern(list, startIndex, endIndex, comparer))
                return true;
            BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
            return true;
        }

        //Code from https://stackoverflow.com/questions/55008384/can-quicksort-be-implemented-in-c-without-stack-and-recursion
        //Original from http://alienryderflex.com/quicksort/
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe bool TryQuickSort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer, int predictedStackDeep)
        {
            int* beg = stackalloc int[predictedStackDeep];
            int* end = stackalloc int[predictedStackDeep];
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

                    if (i == predictedStackDeep - 1)
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
