using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using InlineMethod;

using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.InsertionSort;
using RiceTea.ArraySorts.Internal.MergeSort;
using RiceTea.Numerics;

namespace RiceTea.ArraySorts.Internal.QuickSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class QuickSortImplUnmanagedNC<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd)
        {
            long count = ptrEnd - ptr;
            if (count <= 64L)
            {
                if (count < 2L || SortUtils.ShortCircuitSortNC(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            SortCore(ptr, ptrEnd - 1, 0);
            InsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
        }

        //From https://code-maze.com/csharp-quicksort-algorithm/
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(T* ptrStart, T* ptrLast, int depth)
        {
            if (depth >= 32) //如果堆疊深度大於 32，用合併排序對子序列做排序
            {
                MergeSortImplUnmanagedNC<T>.Sort(ptrStart, ptrLast + 1);
                return;
            }
            long count = ptrLast - ptrStart + 1;
            if (count <= 64L)
            {
                if (count < 2L || SortUtils.ShortCircuitSortNC(ptrStart, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptrStart, ptrLast + 1);
                return;
            }
            SortCore(ptrStart, ptrLast, depth);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptrStart, T* ptrLast, int depth)
        {
            T* leftPointer = ptrStart;
            T* rightPointer = ptrLast;

            T pivot = *leftPointer;

            PackedPrimitive<T> packedPivot = new PackedPrimitive<T>(pivot);

            while (leftPointer <= rightPointer)
            {
                while (new PackedPrimitive<T>(leftPointer) < packedPivot)
                {
                    leftPointer++;
                }

                while (new PackedPrimitive<T>(rightPointer) > packedPivot)
                {
                    rightPointer--;
                }

                if (leftPointer <= rightPointer)
                {
                    (*rightPointer, *leftPointer) = (*leftPointer, *rightPointer);
                    leftPointer++;
                    rightPointer--;
                }
            }

            if (ptrStart < rightPointer)
                SortInternal(ptrStart, rightPointer, depth + 1);
            if (leftPointer < ptrLast)
                SortInternal(leftPointer, ptrLast, depth + 1);
        }
    }
}
