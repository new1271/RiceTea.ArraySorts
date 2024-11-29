
using InlineMethod;

using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.ShellSort;
using RiceTea.ArraySorts.Memory;
using RiceTea.Numerics;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.InPlaceMergeSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class InPlaceMergeSortImplUnmanagedNC<T>
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort(T* ptr, T* ptrEnd)
        {
            long count = ptrEnd - ptr;
            if (count <= 16)
            {
                if (count < 2L || SortUtils.ShortCircuitSortNC(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            SortCore(ptr, ptrEnd, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SortWithoutCheck(T* ptr, T* ptrEnd, long count)
        {
            SortCore(ptr, ptrEnd, count);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void SortInternal(T* ptr, T* ptrEnd)
        {
            long count = ptrEnd - ptr;
            if (count <= 16)
            {
                if (count < 2L || SortUtils.ShortCircuitSortNC(ptr, count))
                    return;
                BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
                return;
            }
            SortCore(ptr, ptrEnd, count);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, long count)
        {
            T* pivot = ptr + (count >> 1);
            SortInternal(ptr, pivot);
            SortInternal(pivot, ptrEnd);
            Merge(ptr, pivot, ptrEnd);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Merge(T* ptr, T* pivot, T* ptrEnd)
        {
            T left = *(pivot - 1);
            T right = *pivot;
            if (new PackedPrimitive<T>(left) < right)
                return;
            long count = ptrEnd - ptr;
            if (count < 2L || SortUtils.ShortCircuitSortNC(ptr, count))
                return;
            int step = unchecked((int)count);
            do
            {
                bool isOdd = (step & 0b01) == 0b01;
                step >>= 1;
                if (isOdd)
                    step++;
                ShellSortImplUnmanagedNC<T>.SortOnce(ptr, ptrEnd, step);
            } while (step > 1);
        }
    }
}

