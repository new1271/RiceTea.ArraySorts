using InlineMethod;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.MergeSort;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.QuickSort
{
    internal static class QuickSortImpl
    {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Sort<T>(IList<T> list, int index, int count, IComparer<T> comparer)
        {
            if (list is T[] array)
            {
                Type type = typeof(T);
                if (type.IsPrimitive || type.IsValueType)
                {
                    fixed (T* ptr = array)
                    {
                        T* ptrStart = ptr + index;
                        T* ptrEnd = ptr + index + count;
                        QuickSortImplUnmanaged<T>.Sort(ptrStart, ptrEnd, comparer);
                    }
                    return;
                }
            }
            QuickSortImplManaged<T>.Sort(list, 0, list.Count, comparer ?? Comparer<T>.Default);
        }
    }
}