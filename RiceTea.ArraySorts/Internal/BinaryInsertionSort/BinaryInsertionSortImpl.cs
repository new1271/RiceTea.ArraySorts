using InlineMethod;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.BinaryInsertionSort
{
    internal static unsafe class BinaryInsertionSortImpl
    {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(IList<T> list, IComparer<T> comparer)
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
                            BinaryInsertionSortImplUnmanagedNC<T>.Sort(ptr, ptr + array.Length);
                            return;
                        }
                        BinaryInsertionSortImplUnmanaged<T>.Sort(ptr, ptr + array.Length, comparer);
                    }
                    return;
                }
                if (type.IsValueType)
                {
                    fixed (T* ptr = array)
                        BinaryInsertionSortImplUnmanaged<T>.Sort(ptr, ptr + array.Length, comparer ?? Comparer<T>.Default);
                    return;
                }
            }
            BinaryInsertionSortImplManaged<T>.Sort(list, 0, list.Count, comparer ?? Comparer<T>.Default);
        }
    }
}
