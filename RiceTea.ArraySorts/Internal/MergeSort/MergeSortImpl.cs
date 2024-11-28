
using InlineMethod;
using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Memory;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.MergeSort
{
    internal static unsafe class MergeSortImpl
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
                            MergeSortImplUnmanagedNC<T>.Sort(ptr, ptr + array.Length);
                            return;
                        }
                        MergeSortImplUnmanaged<T>.Sort(ptr, ptr + array.Length, comparer);
                    }
                    return;
                }
                if (type.IsValueType)
                {
                    fixed (T* ptr = array)
                        MergeSortImplUnmanaged<T>.Sort(ptr, ptr + array.Length, comparer ?? Comparer<T>.Default);
                    return;
                }
            }
            MergeSortImplManaged<T>.Sort(list, 0, list.Count, comparer ?? Comparer<T>.Default);
        }
    }
}

