using InlineMethod;

using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.MergeSort;
using RiceTea.Numerics;

using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.QuickSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class QuickSortImplUnmanagedNC<T>
    {
        private const int MAX_LEVELS = 128;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd)
        {
            long count = ptrEnd - ptr;
            if (count <= 16L)
            {
                if (count < 2L || SortUtils.ShortCircuitSortNC(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            if (SortCore(ptr, ptrEnd))
                return;
            MergeSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd, count);
        }

        //Code from https://stackoverflow.com/questions/55008384/can-quicksort-be-implemented-in-c-without-stack-and-recursion
        //Original from http://alienryderflex.com/quicksort/
        [MethodImpl(MethodImplOptions.NoInlining)] //因為 stackalloc 的關係, 這裡不能內聯 (避免出現 StackOverflowException )
        private static bool SortCore(T* ptr, T* ptrEnd)
        {
            int* beg = stackalloc int[MAX_LEVELS];
            int* end = stackalloc int[MAX_LEVELS];
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
                    PackedPrimitive<T> piv = new PackedPrimitive<T>(ptr[M]);
                    ptr[M] = ptr[L];

                    if (i == MAX_LEVELS - 1)
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
