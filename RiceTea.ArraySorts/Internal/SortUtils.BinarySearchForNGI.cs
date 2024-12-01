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
    partial class SortUtils
    {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T* BinarySearchForNGI<T>(T* ptr, T* ptrEnd, T item) //NGI = Nearest Greater Item
        {
            do
            {
                long count = ptrEnd - ptr;
                if (count < 4L)
                {
                    if (count < 2L)
                    {
                        if (ptr < ptrEnd)
                            return (new PackedPrimitive<T>(item) >= *ptr) ? ptr + 1 : ptr;
                        return ptr;
                    }
                    while (ptr < ptrEnd && (new PackedPrimitive<T>(item) >= *ptr))
                    {
                        ptr++;
                    }
                    return ptr;
                }
                T* ptrMiddle = ptr + ((count - 1) >> 1);
                int compare = new PackedPrimitive<T>(item).CompareTo(*ptrMiddle);
                if (compare == 0)
                    return ptrMiddle + 1;
                if (compare > 0)
                {
                    ptr = ptrMiddle + 1;
                    continue;
                }
                ptrEnd = ptrMiddle;
            }
            while (true);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe T* BinarySearchForNGI<T>(T* ptr, T* ptrEnd, T item, IComparer<T> comparer) //NGI = Nearest Greater Item
        {
            do
            {
                long count = ptrEnd - ptr;
                if (count < 4L)
                {
                    if (count < 2L)
                    {
                        if (ptr < ptrEnd)
                            return comparer.Compare(item, *ptr) >= 0 ? ptr + 1 : ptr;
                        return ptr;
                    }
                    while (ptr < ptrEnd && comparer.Compare(item, *ptr) >= 0)
                    {
                        ptr++;
                    }
                    return ptr;
                }
                T* ptrMiddle = ptr + ((count - 1) >> 1);
                int compare = comparer.Compare(item, *ptrMiddle);
                if (compare == 0)
                    return ptrMiddle + 1;
                if (compare > 0)
                {
                    ptr = ptrMiddle + 1;
                    continue;
                }
                ptrEnd = ptrMiddle;
            }
            while (true);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int BinarySearchForNGI<T>(IList<T> list, int startIndex, int endIndex, T item, IComparer<T> comparer) //NGI = Nearest Greater Item
        {
            do
            {
                int count = endIndex - startIndex + 1;
                if (count < 4)
                {
                    if (count < 2)
                    {
                        if (startIndex < endIndex)
                            return comparer.Compare(item, list[startIndex]) >= 0 ? startIndex + 1 : startIndex;
                        return startIndex;
                    }
                    while (startIndex < endIndex && comparer.Compare(item, list[startIndex]) >= 0)
                    {
                        startIndex++;
                    }
                    return startIndex;
                }
                int middleIndex = startIndex + ((count - 1) >> 1);
                int compare = comparer.Compare(item, list[middleIndex]);
                if (compare == 0)
                    return middleIndex + 1;
                if (compare > 0)
                {
                    startIndex = middleIndex + 1;
                    continue;
                }
                endIndex = middleIndex;
            }
            while (true);
        }
    }
}
