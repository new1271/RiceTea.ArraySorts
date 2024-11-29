using InlineMethod;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.ShellSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class ShellSortImplUnmanaged<T>
    {
        public static void Sort(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            long count = ptrEnd - ptr;
            if (count < 2 || SortUtils.ShortCircuitSort(ptr, count, comparer))
                return;
            SortInternal(ptr, ptrEnd, comparer);
        }

        public static void SortWithoutCheck(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            SortInternal(ptr, ptrEnd, comparer);
        }

        public static void SortOnce(T* ptr, T* ptrEnd, IComparer<T> comparer, int step)
        {
            SortCore(ptr, ptrEnd, comparer, step);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortInternal(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            int step = unchecked((int)(ptrEnd - ptr));
            do
            {
                step >>= 1;
                SortCore(ptr, ptrEnd, comparer, step);
            } while (step > 1);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, IComparer<T> comparer, int step)
        {
            for (int i = 0; i < step; i++)
            {
                T* ptrStart = ptr + i;
                for (T* iterator = ptrStart; iterator < ptrEnd; iterator += step)
                {
                    T item = *iterator;
                    T* iteratorReverse;
                    for (iteratorReverse = iterator - step; iteratorReverse >= ptrStart; iteratorReverse -= step)
                    {
                        T itemComparing = *iteratorReverse;
                        T* replaceIterator = iteratorReverse + step;
                        if (comparer.Compare(item, itemComparing) < 0)
                        {
                            *replaceIterator = itemComparing;
                            continue;
                        }
                        if (replaceIterator == iterator)
                            break;
                        *replaceIterator = item;
                        break;
                    }
                    if (iteratorReverse < ptrStart && ptrStart < iterator)
                    {
                        *ptrStart = item;
                    }
                }
            }
        }
    }
}
