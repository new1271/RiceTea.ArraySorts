using InlineMethod;

using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.MergeSort;
using RiceTea.Numerics;

using System.Collections;
using System.Collections.Generic;
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
            if (count < 2L || SortUtils.OptimizeSortNC(ptr, ptrEnd, count))
                return;
            SortCore(ptr, ptrEnd - 1);
        }

        //Code from https://stackoverflow.com/questions/33884057/quick-sort-stackoverflow-error-for-large-arrays
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe void SortCore(T* ptr, T* ptrLast)
        {
            while (ptr < ptrLast)
            {
                T* ptrPivot = Partition(ptr, ptrLast);
                if (ptrPivot - ptr <= (ptrLast - ptrPivot) - 1)
                {
                    SortCore(ptr, ptrPivot);
                    ptr = ptrPivot + 1;
                }
                else
                {
                    SortCore(ptrPivot + 1, ptrLast);
                    ptrLast = ptrPivot;
                }
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T* Partition(T* ptr, T* ptrLast)
        {
            PackedPrimitive<T> pivot = *ptr;
            T* leftIterator = ptr - 1;
            T* rightIterator = ptrLast + 1;

            while (true)
            {
                while (++leftIterator < ptrLast && *leftIterator < pivot) ;
                while (--rightIterator > ptr && *rightIterator > pivot) ;

                if (leftIterator < rightIterator)
                {
                    (*leftIterator, *rightIterator) = (*rightIterator, *leftIterator);
                    continue;
                }
                return rightIterator;
            }
        }
    }
}
