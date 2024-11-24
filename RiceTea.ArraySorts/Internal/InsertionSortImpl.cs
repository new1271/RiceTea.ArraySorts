using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
    internal static unsafe class InsertionSortImpl
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(T[] array, IComparer<T> comparer) where T : unmanaged
        {
            fixed (T* ptr = array)
                InsertionSortImpl<T>.Sort(ptr, ptr + array.Length, comparer);
        }
    }

    internal static unsafe class InsertionSortImpl<T> where T : unmanaged
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;
            if (count < 2)
                return;
            if (count == 2)
            {
                T* ptr1 = ptr + 1;
                T a = *ptr;
                T b = *ptr1;
                if (comparer.Compare(a, b) < 0)
                {
                    *ptr = b;
                    *ptr1 = a;
                }
                return;
            }
            for (T* iterator = ptr; iterator < ptrEnd; iterator++)
            {
                T a = *iterator;
                T* iteratorReverse;
                for (iteratorReverse = iterator - 1; iteratorReverse >= ptr; iteratorReverse--)
                {
                    T b = *iteratorReverse;
                    T* replaceIterator = iteratorReverse + 1;
                    if (comparer.Compare(a, b) < 0)
                    {
                        *replaceIterator = b;
                        continue;
                    }
                    if (replaceIterator == iterator)
                        break;
                    *replaceIterator = a;
                    break;
                }
                if (iteratorReverse < ptr)
                {
                    if (ptr == iterator)
                        continue;
                    *ptr = a;
                }
            }
        }
    }
}
