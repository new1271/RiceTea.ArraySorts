using InlineMethod;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
    internal static class QuickSortImpl
    {
        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(IList<T> list, IComparer<T> comparer)
        {
            if (list is T[] array)
            {
                Type type = typeof(T);
                if (type.IsPrimitive || type.IsValueType)
                {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
                    //Do unsafe sort
                    unsafe
                    {
                        fixed (T* ptr = array)
                        {
                            QuickSortImplUnsafe<T>.Sort(ptr, ptr + array.Length, comparer);
                        }
                    }
                    return;
#pragma warning restore CS8500
                }
            }
            QuickSortImpl<T>.Sort(list, 0, list.Count, comparer);
        }
    }

    internal static class QuickSortImpl<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            SortInternal(list, startIndex, endIndex - 1, comparer);
        }

        //From https://code-maze.com/csharp-quicksort-algorithm/
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(IList<T> list, int startIndex, int lastIndex, IComparer<T> comparer)
        {
            int count = lastIndex - startIndex + 1;
            if (count <= 64)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImpl<T>.Sort(list, startIndex, lastIndex + 1, comparer);
                return;
            }

            int leftIndex = startIndex;
            int rightIndex = lastIndex;

            T pivot = list[startIndex];

            while (leftIndex <= rightIndex)
            {
                while (comparer.Compare(list[leftIndex], pivot) < 0)
                {
                    leftIndex++;
                }

                while (comparer.Compare(list[rightIndex], pivot) > 0)
                {
                    rightIndex--;
                }

                if (leftIndex <= rightIndex)
                {
                    (list[rightIndex], list[leftIndex]) = (list[leftIndex], list[rightIndex]);
                    leftIndex++;
                    rightIndex--;
                }
            }

            if (startIndex < rightIndex)
                SortInternal(list, startIndex, rightIndex, comparer);

            if (leftIndex < lastIndex)
                SortInternal(list, leftIndex, lastIndex, comparer);
        }
    }
}