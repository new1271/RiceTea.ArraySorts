using InlineMethod;

using RiceTea.Numerics;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RiceTea.ArraySorts.Config;
using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using System.Collections;




#if !DEBUG
using InlineIL;
#endif

namespace RiceTea.ArraySorts.Internal
{
    internal static partial class SortUtils
    {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T>(IList<T> list) => list is null || list.Count <= 0;

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ShortCircuitSort<T>(IList<T> list, int startIndex, int count, IComparer<T> comparer)
        {
            switch (count)
            {
                case 2:
                    {
                        int startIndex1 = startIndex + 1;
                        T a = list[startIndex];
                        T b = list[startIndex1];
                        if (comparer.Compare(a, b) > 0)
                        {
                            list[startIndex] = b;
                            list[startIndex1] = a;
                        }
                    }
                    return true;
                case 3:
                    {
                        int startIndex1 = startIndex + 1;
                        int startIndex2 = startIndex + 2;
                        T a = list[startIndex];
                        T b = list[startIndex1];
                        T c = list[startIndex2];
                        if (Sort3(ref a, ref b, ref c, comparer))
                        {
                            list[startIndex] = a;
                            list[startIndex1] = b;
                            list[startIndex2] = c;
                        }
                    }
                    return true;
                default:
                    return false;
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool ShortCircuitSort<T>(T* ptr, long count, IComparer<T> comparer)
        {
            switch (count)
            {
                case 2:
                    {
                        T* ptr1 = ptr + 1;
                        T a = *ptr;
                        T b = *ptr1;
                        if (comparer.Compare(a, b) > 0)
                        {
                            *ptr = b;
                            *ptr1 = a;
                        }
                    }
                    return true;
                case 3:
                    {
                        T* ptr1 = ptr + 1;
                        T* ptr2 = ptr + 2;
                        Sort3(ref *ptr, ref *ptr1, ref *ptr2, comparer);
                    }
                    return true;
                default:
                    return false;
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool ShortCircuitSortNC<T>(T* ptr, long count)
        {
            switch (count)
            {
                case 2:
                    {
                        T* ptr1 = ptr + 1;
                        T a = *ptr;
                        T b = *ptr1;
                        if (new PackedPrimitive<T>(a) > b)
                        {
                            *ptr = b;
                            *ptr1 = a;
                        }
                    }
                    return true;
                case 3:
                    {
                        T* ptr1 = ptr + 1;
                        T* ptr2 = ptr + 2;
                        Sort3NC(ptr, ptr1, ptr2);
                    }
                    return true;
                default:
                    return false;
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe void Sort3NC<T>(T* ptrA, T* ptrB, T* ptrC)
        {
            PackedPrimitive<T> a = *ptrA;
            PackedPrimitive<T> b = *ptrB;
            PackedPrimitive<T> c = *ptrC;
            if (a < b)
            {
                if (b > c)
                {
                    if (a < c)
                    {
                        (*ptrB, *ptrC) = (c.Value, b.Value);
                    }
                    else
                    {
                        T temp = a.Value;
                        *ptrA = c.Value;
                        *ptrC = b.Value;
                        *ptrB = temp;
                    }
                }
                return;
            }
            else
            {
                if (b < c)
                {
                    if (a < c)
                    {
                        (*ptrA, *ptrB) = (b.Value, a.Value);
                    }
                    else
                    {
                        T temp = a.Value;
                        *ptrA = b.Value;
                        *ptrB = c.Value;
                        *ptrC = temp;
                    }
                }
                else
                {
                    (*ptrA, *ptrC) = (c.Value, a.Value);
                }
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe bool Sort3<T>(ref T a, ref T b, ref T c, IComparer<T> comparer)
        {
            if (comparer.Compare(a, b) < 0)
            {
                if (comparer.Compare(b, c) > 0)
                {
                    if (comparer.Compare(a, c) < 0)
                    {
                        (b, c) = (c, b);
                    }
                    else
                    {
                        T temp = a;
                        a = c;
                        c = b;
                        b = temp;
                    }
                    return true;
                }
                return false;
            }
            else
            {
                if (comparer.Compare(b, c) < 0)
                {
                    if (comparer.Compare(a, c) < 0)
                    {
                        (a, b) = (b, a);
                    }
                    else
                    {
                        T temp = a;
                        a = b;
                        b = c;
                        c = temp;
                    }
                }
                else
                {
                    (a, c) = (c, a);
                }
                return true;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OptimizeSort<T>(IList<T> list, int startIndex, int endIndex, int count, IComparer<T> comparer)
        {
            if (count > 16)
                return CheckPattern(list, startIndex, endIndex, comparer);
            if (ShortCircuitSort(list, startIndex, count, comparer) || CheckPattern(list, startIndex, endIndex, comparer))
                return true;
            if (!ArraySortsConfig.OptimizeTinySequenceSorting)
                return false;
            BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool OptimizeSort<T>(T* ptr, T* ptrEnd, long count, IComparer<T> comparer)
        {
            if (count > 16L)
                return CheckPattern(ptr, ptrEnd, comparer);
            if (ShortCircuitSort(ptr, count, comparer) || CheckPattern(ptr, ptrEnd, comparer))
                return true;
            if (!ArraySortsConfig.OptimizeTinySequenceSorting)
                return false;
            BinaryInsertionSortImplUnmanaged<T>.SortWithoutCheck(ptr, ptrEnd, comparer);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool OptimizeSortNC<T>(T* ptr, T* ptrEnd, long count)
        {
            if (count > 16L)
                return CheckPatternNC(ptr, ptrEnd);
            if (ShortCircuitSortNC(ptr, count) || CheckPatternNC(ptr, ptrEnd))
                return true;
            if (!ArraySortsConfig.OptimizeTinySequenceSorting)
                return false;
            BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool CheckPattern<T>(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            if (!ArraySortsConfig.OptimizeSortedSequence)
                return false;
            int i = startIndex + 1;
            if (i >= endIndex)
                return true;
            int headCompare = comparer.Compare(list[i], list[startIndex]);
            while (++i < endIndex)
            {
                int newCompare = comparer.Compare(list[i], list[i - 1]);
                if (headCompare != newCompare)
                    return false;
            }
            if (headCompare < 0)
                Reverse(list, startIndex, endIndex);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool CheckPattern<T>(T* ptr, T* ptrEnd, IComparer<T> comparer)
        {
            if (!ArraySortsConfig.OptimizeSortedSequence)
                return false;
            T* iterator = ptr + 1;
            if (iterator >= ptrEnd)
                return true;
            int headCompare = comparer.Compare(*iterator, *ptr);
            while (++iterator < ptrEnd)
            {
                int newCompare = comparer.Compare(*iterator, *(iterator - 1));
                if (headCompare != newCompare)
                    return false;
            }
            if (headCompare < 0)
                Reverse(ptr, ptrEnd);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool CheckPatternNC<T>(T* ptr, T* ptrEnd)
        {
            if (!ArraySortsConfig.OptimizeSortedSequence)
                return false;
            T* iterator = ptr + 1;
            if (iterator >= ptrEnd)
                return true;
            PackedPrimitive<T> headCompare = new PackedPrimitive<T>(*iterator) - *ptr;
            while (++iterator < ptrEnd)
            {
                PackedPrimitive<T> compare = new PackedPrimitive<T>(*iterator) - *(iterator - 1);
                if (headCompare != compare)
                    return false;
            }
            if (headCompare < default(T))
                Reverse(ptr, ptrEnd);
            return true;
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Reverse<T>(IList<T> list, int startIndex, int endIndex)
        {
            for (int i = startIndex, j = endIndex - 1; i < j; i++, j--)
            {
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Reverse<T>(T* ptr, T* ptrEnd)
        {
            for (T* iterator = ptr, iterator2 = ptrEnd - 1; iterator < iterator2; iterator++, iterator2--)
            {
                (*iterator, *iterator2) = (*iterator2, *iterator);
            }
        }
    }
}
