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

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool OptimizeSort<T>(IList<T> list, int startIndex, int endIndex, int count, IComparer<T> comparer)
        {
            if (count > 16)
                return false;
            if (ShortCircuitSort(list, startIndex, count, comparer))
                return true;
            if (!ArraySortsConfig.OptimizeSorting)
                return false;
            BinaryInsertionSortImplManaged<T>.SortWithoutCheck(list, startIndex, endIndex, comparer);
            return true;
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool OptimizeSort<T>(T* ptr, T* ptrEnd, long count, IComparer<T> comparer)
        {
            if (count > 16L)
                return false;
            if (ShortCircuitSort(ptr, count, comparer))
                return true;
            if (!ArraySortsConfig.OptimizeSorting)
                return false;
            BinaryInsertionSortImplUnmanaged<T>.SortWithoutCheck(ptr, ptrEnd, comparer);
            return true;
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool OptimizeSortNC<T>(T* ptr, T* ptrEnd, long count)
        {
            if (count > 16L)
                return false;
            if (ShortCircuitSortNC(ptr, count))
                return true;
            if (!ArraySortsConfig.OptimizeSorting)
                return false;
            BinaryInsertionSortImplUnmanagedNC<T>.SortWithoutCheck(ptr, ptrEnd);
            return true;
        }
    }
}
