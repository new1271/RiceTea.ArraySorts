using InlineMethod;
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
        public static unsafe void Sort<T>(IList<T> list, IComparer<T> comparer)
        {
            if (list is T[] array)
            {
                Type type = typeof(T);
                if (type.IsPrimitive)
                {
                    fixed (T* ptr = array)
                    {
                        if (comparer is null || comparer == Comparer<T>.Default)
                        {
                            InsertionSortImplUnmanagedNC<T>.Sort(ptr, ptr + array.Length);
                            return;
                        }
                        InsertionSortImplUnmanaged<T>.Sort(ptr, ptr + array.Length, comparer);
                    }
                    return;
                }
                if (type.IsValueType)
                {
                    fixed (T* ptr = array)
                        InsertionSortImplUnmanaged<T>.Sort(ptr, ptr + array.Length, comparer ?? Comparer<T>.Default);
                    return;
                }
            }
            InsertionSortImplManaged<T>.Sort(list, 0, list.Count, comparer ?? Comparer<T>.Default);
        }
    }
}
