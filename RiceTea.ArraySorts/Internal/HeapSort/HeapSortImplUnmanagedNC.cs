using InlineMethod;

using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.Numerics;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.HeapSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class HeapSortImplUnmanagedNC<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd)
        {
            long count = ptrEnd - ptr;
            if (count < 2L || SortUtils.OptimizeSortNC(ptr, ptrEnd, count))
                return;
            SortCore(ptr, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(T* ptr, T* ptrEnd)
        {
            SortCore(ptr, ptrEnd - ptr);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, long count)
        {
            // 建立最大堆積
            long i;
            for (i = (count >> 1) - 1; i >= 0; i--)
            {
                MaxHeapify(ptr, i, count - 1);
            }
            long target = ArraySortsConfig.OptimizeTinySequenceSorting ? 0L : 16L;
            for (i = count - 1; i > target; i--)
            {
                //將堆積的頭尾交換
                T* iterator = ptr + i;
                (*ptr, *iterator) = (*iterator, *ptr);
                //重新整理堆積
                MaxHeapify(ptr, 0, i - 1);
            }
            if (i > 0)
            {
                T* ptrEnd = ptr + i + 1;
                SortUtils.Reverse(ptr, ptrEnd);
                BinaryInsertionSortImplUnmanagedNC<T>.Sort(ptr, ptrEnd);
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MaxHeapify(T* ptr, long startIndex, long endIndex)
        {
            // 建立父節點指標和子節點指標
            PackedPrimitive<T> parent = ptr[startIndex];
            for (long parentIndex = startIndex, childIndex = (parentIndex << 1) + 1; childIndex <= endIndex;
                parentIndex = childIndex, childIndex = (childIndex << 1) + 1)
            {
                PackedPrimitive<T> child = ptr[childIndex];
                long childIndex2 = childIndex + 1;
                if (childIndex2 <= endIndex)
                {
                    T child2 = ptr[childIndex2];
                    if (child < child2)
                    {
                        child = child2;
                        childIndex = childIndex2;
                    }
                }
                if (parent > child) // 如果父節點大於子節點代表調整完畢，直接跳出函數
                    return;
                ptr[parentIndex] = child.Value;
                ptr[childIndex] = parent.Value;
            }
        }
    }
}
