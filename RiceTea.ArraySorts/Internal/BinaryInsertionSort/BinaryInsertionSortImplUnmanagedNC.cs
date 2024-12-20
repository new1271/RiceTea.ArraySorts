﻿using InlineMethod;

using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.BinaryInsertionSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class BinaryInsertionSortImplUnmanagedNC<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd)
        {
            long count = ptrEnd - ptr;
            if (count < 2 || SortUtils.ShortCircuitSortNC(ptr, count))
                return;
            SortCore(ptr, ptrEnd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(T* ptr, T* ptrEnd)
        {
            SortCore(ptr, ptrEnd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(T* ptr, T* iterator, T* ptrEnd)
        {
            SortCore(ptr, iterator, ptrEnd);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd)
        {
            SortCore(ptr, ptr + 1, ptrEnd);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* iterator, T* ptrEnd)
        {
            for (; iterator < ptrEnd; iterator++)
            {
                T item = *iterator;
                T* ptrPlace = SortUtils.BinarySearchForNGI(ptr, iterator, item);
                if (ptrPlace == iterator)
                    continue;
                for (T* iteratorReverse = iterator; iteratorReverse > ptrPlace; iteratorReverse--)
                {
                    *iteratorReverse = *(iteratorReverse - 1);
                }
                *ptrPlace = item;
            }
        }
    }
}
