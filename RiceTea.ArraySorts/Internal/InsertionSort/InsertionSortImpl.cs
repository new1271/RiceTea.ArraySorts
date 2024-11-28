using InlineMethod;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.InsertionSort
{
    internal static class InsertionSortImpl
    {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Sort<T>(IList<T> list, int index, int count, IComparer<T> comparer)
        {
            if (list is T[] array)
            {
                Type type = typeof(T);
                if (type.IsPrimitive)
                {
                    fixed (T* ptr = array)
                    {
                        T* ptrStart = ptr + index;
                        T* ptrEnd = ptr + index + count;
                        if (comparer is null || comparer == Comparer<T>.Default)
                        {
                            InsertionSortImplUnmanagedNC<T>.Sort(ptrStart, ptrEnd);
                            return;
                        }
                        InsertionSortImplUnmanaged<T>.Sort(ptrStart, ptrEnd, comparer);
                    }
                    return;
                }
                if (type.IsValueType)
                {
                    fixed (T* ptr = array)
                    {
                        T* ptrStart = ptr + index;
                        T* ptrEnd = ptr + index + count;
                        InsertionSortImplUnmanaged<T>.Sort(ptrStart, ptrEnd, comparer ?? Comparer<T>.Default);
                    }
                    return;
                }
            }
            InsertionSortImplManaged<T>.Sort(list, 0, list.Count, comparer ?? Comparer<T>.Default);
        }
    }
}
