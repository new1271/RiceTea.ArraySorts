using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace SortAlgorithms.Internal
{
    internal static unsafe class QuickSortImpl
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(T[] array, IComparer<T> comparer) where T : unmanaged
        {
            fixed (T* ptr = array)
                QuickSortImpl<T>.Sort(ptr, ptr + array.Length, comparer);
        }
    }

    internal static unsafe class QuickSortImpl<T> where T : unmanaged
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;
            if (count <= 64L)
            {
                InsertionSortImpl<T>.Sort(ptr, ptrEnd, comparer);
                return;
            }
            SortInternal(ptr, ptrEnd - 1, comparer);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(T* ptrFirst, T* ptrLast, IComparer<T> comparer)
        {
            long count = ptrLast - ptrFirst - 1;
            if (count <= 64L)
            {
                InsertionSortImpl<T>.Sort(ptrFirst, ptrLast + 1, comparer);
                return;
            }
            if (ptrFirst >= ptrLast)
                return;
            T pivot = *(ptrFirst + (ptrLast - ptrFirst) / 2);
            T* leftPointer = ptrFirst - 1;
            T* rightPointer = ptrLast + 1;
            while (true)
            {
                while (comparer.Compare(*(++leftPointer), pivot) < 0) ;
                while (comparer.Compare(*(--rightPointer), pivot) > 0) ;
                if (leftPointer >= rightPointer)
                    break;
                T temp = *leftPointer;
                *leftPointer = *rightPointer;
                *rightPointer = temp;
            }
            SortInternal(ptrFirst, rightPointer, comparer);
            SortInternal(rightPointer + 1, ptrLast, comparer);
        }
    }
}
