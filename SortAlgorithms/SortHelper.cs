using SortAlgorithms.Internal;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

namespace SortAlgorithms
{
    public static class SortHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuickSort<T>(T[] array, IComparer<T> comparer = null) where T : unmanaged
        {
            if (array is null || array.Length <= 0)
                return;
            QuickSortImpl.Sort(array, comparer ?? Comparer<T>.Default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InsertionSort<T>(T[] array, IComparer<T> comparer = null) where T : unmanaged
        {
            if (array is null || array.Length <= 0)
                return;
            InsertionSortImpl.Sort(array, comparer ?? Comparer<T>.Default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MergeSort<T>(T[] array, IComparer<T> comparer = null) where T : unmanaged
        {
            if (array is null || array.Length <= 0)
                return;
            MergeSortImpl.Sort(array, comparer ?? Comparer<T>.Default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(T[] array, IComparer<T> comparer = null) where T : unmanaged
        {
            if (array is null || array.Length <= 0)
                return;
            Array.Sort(array, comparer ?? Comparer<T>.Default);
        }
    }
}
