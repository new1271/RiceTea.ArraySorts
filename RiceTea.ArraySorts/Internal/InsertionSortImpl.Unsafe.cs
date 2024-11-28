﻿using InlineMethod;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class InsertionSortImplUnsafe<T>
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
                T* iteratorReverse;
                for (iteratorReverse = iterator - 1; iteratorReverse >= ptr; iteratorReverse--)
                {
                    T itemComparing = *iteratorReverse;
                    T* replaceIterator = iteratorReverse + 1;
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
                if (iteratorReverse < ptr)
                {
                    *ptr = item;
                }
            }
        }
    }
}
