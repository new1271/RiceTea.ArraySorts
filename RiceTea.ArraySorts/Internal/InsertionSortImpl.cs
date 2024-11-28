﻿using InlineMethod;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
    internal static class InsertionSortImpl
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
                            InsertionSortImplUnsafe<T>.Sort(ptr, ptr + array.Length, comparer);
                        }
                    }
                    return;
#pragma warning restore CS8500
                }
            }
            InsertionSortImpl<T>.Sort(list, 0, list.Count, comparer);
        }
    }

    internal static class InsertionSortImpl<T>
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
        {
            SortCore(list, startIndex, endIndex, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            for (int i = startIndex + 1; i < endIndex; i++)
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
