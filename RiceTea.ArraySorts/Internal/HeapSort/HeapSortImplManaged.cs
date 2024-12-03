using InlineMethod;

using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RiceTea.ArraySorts.Internal.HeapSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class HeapSortImplManaged<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count < 2 || SortUtils.OptimizeSort(list, startIndex, endIndex, count, comparer))
                return;
            SortCore(list, startIndex, count, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortCore(IList<T> list, int startIndex, int count, IComparer<T> comparer)
        {
            // 建立最大堆積
            int i;
            for (i = (count >> 1) - 1; i >= 0; i--)
            {
                MaxHeapify(list, i, startIndex, count - 1, comparer);
            }
            int target = ArraySortsConfig.OptimizeTinySequenceSorting ? 0 : 16;
            for (i = count - 1; i > target; i--)
            {
                //將堆積的頭尾交換
                int offsetedIterator = startIndex + i;
                (list[startIndex], list[offsetedIterator]) = (list[offsetedIterator], list[startIndex]);
                //重新整理堆積
                MaxHeapify(list, 0, startIndex, i - 1, comparer);
            }
            if (i > 0)
            {
                int endIndex = i + 1;
                SortUtils.Reverse(list, startIndex, endIndex);
                BinaryInsertionSortImplManaged<T>.Sort(list, startIndex, endIndex, comparer);
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void MaxHeapify(IList<T> list, int startIndex, int offset, int endIndex, IComparer<T> comparer)
        {
            // 建立父節點指標和子節點指標
            T parent = list[startIndex + offset];
            for (int parentIndex = startIndex, childIndex = (parentIndex << 1) + 1; childIndex <= endIndex;
                parentIndex = childIndex, childIndex = (childIndex << 1) + 1)
            {
                T child = list[childIndex + offset];
                int childIndex2 = childIndex + 1;
                if (childIndex2 <= endIndex)
                {
                    T child2 = list[childIndex2 + offset];
                    if (comparer.Compare(child, child2) < 0)
                    {
                        child = child2;
                        childIndex = childIndex2;
                    }
                }
                if (comparer.Compare(parent, child) > 0) // 如果父節點大於子節點代表調整完畢，直接跳出函數
                    return;
                list[parentIndex + offset] = child;
                list[childIndex + offset] = parent;
            }
        }
    }
}
