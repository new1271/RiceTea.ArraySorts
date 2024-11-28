using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;
using RiceTea.ArraySorts.Internal;
using System.Collections;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.InsertionSort;
using RiceTea.ArraySorts.Internal.MergeSort;
using RiceTea.ArraySorts.Internal.QuickSort;

namespace RiceTea.ArraySorts
{
    public static class ArraySorts
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuickSort<T>(IList<T> list, IComparer<T> comparer = null)
        {
            if (list is null || list.Count <= 0)
                return;
            QuickSortImpl.Sort(list, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(IList<T> list, IComparer<T> comparer = null)
        {
            if (list is null || list.Count <= 0)
                return;
            BinaryInsertionSortImpl.Sort(list, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InsertionSort<T>(IList<T> list, IComparer<T> comparer = null)
        {
            if (list is null || list.Count <= 0)
                return;
            InsertionSortImpl.Sort(list, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void MergeSort<T>(IList<T> list, IComparer<T> comparer = null)
        {
            if (list is null || list.Count <= 0)
                return;
            MergeSortImpl.Sort(list, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list, IComparer<T> comparer = null)
        {
            if (list is null || list.Count <= 0)
                return;
            switch (list)
            {
                case T[] array:
                    Array.Sort(array, comparer);
                    return;
                case List<T> _list:
                    _list.Sort(comparer);
                    return;
                case IList _list:
                    {
                        ArrayList arrayList = ArrayList.Adapter(_list);
                        if (comparer is null)
                        {
                            arrayList.Sort();
                            return;
                        }
                        if (comparer is IComparer objectComparer)
                        {
                            arrayList.Sort(objectComparer);
                            return;
                        }
                        arrayList.Sort(new GenericComparerWrapper<T>(comparer));
                    }
                    return;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
