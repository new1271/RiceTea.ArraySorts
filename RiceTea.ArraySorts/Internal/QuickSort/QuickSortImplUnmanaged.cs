using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.InsertionSort;
using RiceTea.ArraySorts.Internal.MergeSort;

namespace RiceTea.ArraySorts.Internal.QuickSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class QuickSortImplUnmanaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;
            if (count <= 16L)
            {
                if (count < 2L || SortUtils.ShortCircuitSort(ptr, count, comparer))
                    return;
                BinaryInsertionSortImplUnmanaged<T>.SortWithoutCheck(ptr, ptrEnd, comparer);
                return;
            }
            SortCore(ptr, ptrEnd - 1, comparer, 0);
            InsertionSortImplUnmanaged<T>.SortWithoutCheck(ptr, ptrEnd, comparer);
        }

        //From https://code-maze.com/csharp-quicksort-algorithm/
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(T* ptrStart, T* ptrLast, IComparer<T> comparer, int depth)
        {
            long count = ptrLast - ptrStart + 1;
            if (count <= 16L)
            {
                if (count < 2L || SortUtils.ShortCircuitSort(ptrStart, count, comparer))
                    return;
                BinaryInsertionSortImplUnmanaged<T>.SortWithoutCheck(ptrStart, ptrLast + 1, comparer);
                return;
            }
            if (depth >= 32) //如果堆疊深度大於 32，用合併排序對子序列做排序
            {
                MergeSortImplUnmanaged<T>.SortWithoutCheck(ptrStart, ptrLast + 1, count, comparer);
                return;
            }
            SortCore(ptrStart, ptrLast, comparer, depth);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptrStart, T* ptrLast, IComparer<T> comparer, int depth)
        {
            T* leftPointer = ptrStart;
            T* rightPointer = ptrLast;

            T pivot = *leftPointer;

            while (leftPointer <= rightPointer)
            {
                while (comparer.Compare(*leftPointer, pivot) < 0)
                {
                    leftPointer++;
                }

                while (comparer.Compare(*rightPointer, pivot) > 0)
                {
                    rightPointer--;
                }

                if (leftPointer <= rightPointer)
                {
                    (*rightPointer, *leftPointer) = (*leftPointer, *rightPointer);
                    leftPointer++;
                    rightPointer--;
                }
            }

            if (ptrStart < rightPointer)
                SortInternal(ptrStart, rightPointer, comparer, depth + 1);

            if (leftPointer < ptrLast)
                SortInternal(leftPointer, ptrLast, comparer, depth + 1);
        }
    }
}
