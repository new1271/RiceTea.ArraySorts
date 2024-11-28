
using InlineMethod;

using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Memory;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
    internal static unsafe class MergeSortImpl
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
                            MergeSortImplUnsafe<T>.Sort(ptr, ptr + array.Length, comparer, null);
                        }
                    }
                    return;
#pragma warning restore CS8500
                }
            }
            MergeSortImpl<T>.Sort(list, 0, list.Count, comparer, null);
        }
    }

    internal static unsafe class MergeSortImpl<T>
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer, IMemoryAllocator allocator)
        {
            int count = endIndex - startIndex;
            if (count <= 64)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImpl<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
                return;
            }
            if (allocator is null)
                allocator = ArraySortsConfig.MemoryAllocator;
            int pivotIndex = startIndex + (count >> 1);
            Sort(list, startIndex, pivotIndex, comparer, allocator);
            Sort(list, pivotIndex, endIndex, comparer, allocator);
            Merge(list, startIndex, pivotIndex, endIndex, comparer, allocator);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(IList<T> list, int startIndex, int pivotIndex, int endIndex, IComparer<T> comparer, IMemoryAllocator allocator)
        {
            T left = list[pivotIndex - 1];
            T right = list[pivotIndex];
            if (comparer.Compare(left, right) < 0)
                return;
            int count = endIndex - startIndex;
            if (count <= 64)
            {
                if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                    return;
                BinaryInsertionSortImpl<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
                return;
            }
            T[] space = allocator.AllocArray<T>(count);
            SortUtils.CopyToArray(space, list, startIndex, 0, count);
            Merge(list, space, startIndex, pivotIndex, endIndex, comparer);
            allocator.FreeArray(space);
        }

        [Inline(InlineBehavior.Remove)]
        private static void Merge(IList<T> list, T[] space, int startIndex, int pivotIndex, int endIndex, IComparer<T> comparer)
        {
            int spacePivotIndex = pivotIndex - startIndex;
            int spaceEndIndex = endIndex - startIndex;

            int leftIndex = 0;
            int rightIndex = pivotIndex - startIndex;
            int sourceIndex = startIndex;

            T left = space[leftIndex], right = space[rightIndex];
            for (; sourceIndex < endIndex && leftIndex < spacePivotIndex && rightIndex < spaceEndIndex; sourceIndex++)
            {
                if (comparer.Compare(left, right) < 0)
                {
                    list[sourceIndex] = left;
                    if (++leftIndex >= spacePivotIndex)
                    {
                        sourceIndex++;
                        break;
                    }
                    left = space[leftIndex];
                }
                else
                {
                    list[sourceIndex] = right;
                    if (++rightIndex >= spaceEndIndex)
                    {
                        sourceIndex++;
                        break;
                    }
                    right = space[rightIndex];
                }
            }

            int count = endIndex - sourceIndex;
            if (count > 0)
            {
                if (leftIndex < spacePivotIndex)
                    SortUtils.CopyToList(list, space, leftIndex, sourceIndex, count);
                else
                    SortUtils.CopyToList(list, space, rightIndex, sourceIndex, count);
            }
        }
    }
}

