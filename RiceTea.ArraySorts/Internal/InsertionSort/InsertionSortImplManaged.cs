using InlineMethod;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.InsertionSort
{
    internal static class InsertionSortImplManaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                return;
            SortCore(list, startIndex, endIndex, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
            => SortCore(list, startIndex, endIndex, comparer);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(IList<T> list, int startIndex, int i, int endIndex, IComparer<T> comparer)
            => SortCore(list, startIndex, i, endIndex, comparer);

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
            => SortCore(list, startIndex, startIndex + 1, endIndex, comparer);

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(IList<T> list, int startIndex, int i , int endIndex, IComparer<T> comparer)
        {
            for (; i < endIndex; i++)
            {
                T item = list[i];
                int reverseIndex;
                for (reverseIndex = i - 1; reverseIndex >= startIndex; reverseIndex--)
                {
                    T itemComparing = list[reverseIndex];
                    int replaceIndex = reverseIndex + 1;
                    if (comparer.Compare(item, itemComparing) < 0)
                    {
                        list[replaceIndex] = itemComparing;
                        continue;
                    }
                    if (replaceIndex == i)
                        break;
                    list[replaceIndex] = item;
                    break;
                }
                if (reverseIndex < startIndex)
                {
                    list[startIndex] = item;
                }
            }
        }
    }
}
