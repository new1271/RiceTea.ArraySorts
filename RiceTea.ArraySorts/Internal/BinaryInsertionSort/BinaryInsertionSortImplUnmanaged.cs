using InlineMethod;

using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;


#if !DEBUG
using InlineIL;
#endif

namespace RiceTea.ArraySorts.Internal.BinaryInsertionSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class BinaryInsertionSortImplUnmanaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;
            if (count < 2 || SortUtils.ShortCircuitSort(ptr, count, comparer))
                return;
            SortCore(ptr, ptrEnd, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            SortCore(ptr, ptrEnd, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            for (T* iterator = ptr + 1; iterator < ptrEnd; iterator++)
            {
                T item = *iterator;
                T* ptrPlace = SortUtils.BinarySearchForNGI(ptr, iterator, item, comparer);
                if (ptrPlace == iterator)
                    continue;
                for (T* iteratorReverse = iterator; iteratorReverse > ptrPlace; iteratorReverse--)
                {
                    *iteratorReverse = *(iteratorReverse - 1);
                }
                *ptrPlace = item;
            }
        }
    }
}
