using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.HeapSort;
using RiceTea.Numerics;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.IntroSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal sealed unsafe class IntroSortImplUnmanagedNC<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd)
        {
            long count = ptrEnd - ptr;
            if (count < 2L || OptimizeSort(ptr, ptrEnd, count))
                return;
            //預測堆疊深度
            int bestStackDeep = MathHelper.Log2(unchecked((ulong)count));
            if (bestStackDeep > 32 || !TryQuickSort(ptr, ptrEnd, bestStackDeep << 1))
                HeapSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool OptimizeSort(T* ptr, T* ptrEnd, long count)
        {
            if (count > 16L)
                return SortUtils.CheckPatternNC(ptr, ptrEnd);
            if (SortUtils.ShortCircuitSortNC(ptr, count) || SortUtils.CheckPatternNC(ptr, ptrEnd))
                return true;
            BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
            return true;
        }

        //Code from https://stackoverflow.com/questions/55008384/can-quicksort-be-implemented-in-c-without-stack-and-recursion
        //Original from http://alienryderflex.com/quicksort/
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe bool TryQuickSort(T* ptr, T* ptrEnd, int predictedStackDeep)
        {
            int* beg = stackalloc int[predictedStackDeep];
            int* end = stackalloc int[predictedStackDeep];
            int L, R;
            int i = 0;

            beg[0] = 0;
            end[0] = unchecked((int)(ptrEnd - ptr));
            while (i >= 0)
            {
                L = beg[i];
                R = end[i];
                if (R - L > 1)
                {
                    int M = L + ((R - L) >> 1);
                    PackedPrimitive<T> piv = ptr[M];
                    ptr[M] = ptr[L];

                    if (i == predictedStackDeep - 1)
                        return false;
                    R--;
                    while (L < R)
                    {
                        while (ptr[R] >= piv && L < R)
                            R--;
                        if (L < R)
                            ptr[L++] = ptr[R];
                        while (ptr[L] <= piv && L < R)
                            L++;
                        if (L < R)
                            ptr[R--] = ptr[L];
                    }
                    ptr[L] = piv.Value;
                    M = L + 1;
                    while (L > beg[i] && ptr[L - 1] == piv)
                        L--;
                    while (M < end[i] && ptr[M] == piv)
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
