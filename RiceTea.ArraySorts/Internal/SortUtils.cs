using InlineMethod;

using RiceTea.Numerics;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if !DEBUG
using InlineIL;
#endif

namespace RiceTea.ArraySorts.Internal
{
    internal static partial class SortUtils
    {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標

        [Inline(InlineBehavior.Remove)]
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
                        Sort3NC(ref *ptr, ref *ptr1, ref *ptr2);
                    }
                    return true;
                default:
                    return false;
            }
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe bool Sort3NC<T>(ref T a, ref T b, ref T c)
        {
            bool isDirty = false;
            if (new PackedPrimitive<T>(a) > b)
            {
                (a, b) = (b, a);
                isDirty = true;
            }
            if (new PackedPrimitive<T>(b) > c)
            {
                (b, c) = (c, b);
                isDirty = true;
            }
            if (new PackedPrimitive<T>(a) > c)
            {
                (a, c) = (c, a);
                isDirty = true;
            }
            return isDirty;
        }

        [Inline(InlineBehavior.Remove)]
        private static unsafe bool Sort3<T>(ref T a, ref T b, ref T c, IComparer<T> comparer)
        {
            bool isDirty = false;
            if (comparer.Compare(a, b) > 0)
            {
                (a, b) = (b, a);
                isDirty = true;
            }
            if (comparer.Compare(b, c) > 0)
            {
                (b, c) = (c, b);
                isDirty = true;
            }
            if (comparer.Compare(a, c) > 0)
            {
                (a, c) = (c, a);
                isDirty = true;
            }
            return isDirty;
        }
    }
}
