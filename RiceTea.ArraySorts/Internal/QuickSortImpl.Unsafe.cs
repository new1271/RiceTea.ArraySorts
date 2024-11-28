using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class QuickSortImplUnsafe<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            SortInternal(ptr, ptrEnd - 1, comparer);
        }

        //From https://code-maze.com/csharp-quicksort-algorithm/
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(T* ptrStart, T* ptrLast, IComparer<T> comparer)
        {
            long count = ptrStart - ptrLast + 1;
            if (count <= 64L)
            {
                if (count < 2L || SortUtils.ShortCircuitSort(ptrStart, count, comparer))
                    return;
                BinaryInsertionSortImplUnsafe<T>.SortWithoutCheck(ptrStart, ptrLast + 1, comparer);
                return;
            }

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
                SortInternal(ptrStart, rightPointer, comparer);

            if (leftPointer < ptrLast)
                SortInternal(leftPointer, ptrLast, comparer);
        }
    }
}
