using InlineIL;

using InlineMethod;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
    internal static class SortUtils
    {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyToArray<T>(T[] destination, IList<T> source, int sourceStartIndex, int destStartIndex, int count)
        {
            switch (source)
            {
                case T[] array:
                    CopyToInternal(destination, destStartIndex, array, sourceStartIndex, count);
                    return;
                case List<T> list:
                    list.CopyTo(sourceStartIndex, destination, destStartIndex, count);
                    return;
                default:
                    {
                        for (int i = 0; i < count; i++)
                            destination[destStartIndex + i] = source[sourceStartIndex + i];
                    }
                    return;
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void CopyToList<T>(IList<T> destination, T[] source, int sourceStartIndex, int destStartIndex, int count)
        {
            switch (destination)
            {
                case T[] array:
                    CopyToInternal(array, destStartIndex, source, sourceStartIndex, count);
                    return;
                default:
                    {
                        for (int i = 0; i < count; i++)
                            destination[destStartIndex + i] = source[sourceStartIndex + i];
                    }
                    return;
            }
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CopyToInternal<T>(T[] destination, int destStartIndex, T[] source, int sourceStartIndex, int count)
        {
            Array.Copy(source, sourceStartIndex, destination, destStartIndex, count);
        }

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
                        T* ptr1 = ptr;
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

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe T* BinarySearchForNGI<T>(T* ptr, T* ptrEnd, T item, IComparer<T> comparer) //NGI = Nearest Greater Item
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
#if DEBUG
                return BinarySearchForNGI(ptrMiddle + 1, ptrEnd, item, comparer);
#else
                IL.Push(ptrMiddle + 1);
                IL.Push(ptrEnd);
                IL.Push(item);
                IL.Push(comparer);
                IL.Emit.Tail();
                IL.Emit.Call(new MethodRef(typeof(SortUtils), nameof(BinarySearchForNGI), typeof(T*), typeof(T*), typeof(T), typeof(IComparer<T>))
                    .MakeGenericMethod(typeof(T)));
                IL.Emit.Ret();
                throw IL.Unreachable();
#endif
            }
#if DEBUG
            return BinarySearchForNGI(ptr, ptrMiddle, item, comparer);
#else
            IL.Push(ptr);
            IL.Push(ptrMiddle);
            IL.Push(item);
            IL.Push(comparer);
            IL.Emit.Tail();
            IL.Emit.Call(new MethodRef(typeof(SortUtils), nameof(BinarySearchForNGI), typeof(T*), typeof(T*), typeof(T), typeof(IComparer<T>))
                .MakeGenericMethod(typeof(T)));
            IL.Emit.Ret();
            throw IL.Unreachable();
#endif
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int BinarySearchForNGI<T>(IList<T> list, int startIndex, int endIndex, T item, IComparer<T> comparer) //NGI = Nearest Greater Item
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
#if DEBUG
                return BinarySearchForNGI(list, middleIndex + 1, endIndex, item, comparer);
#else
                IL.Push(list);
                IL.Push(middleIndex + 1);
                IL.Push(endIndex);
                IL.Push(item);
                IL.Push(comparer);
                IL.Emit.Tail();
                IL.Emit.Call(new MethodRef(typeof(SortUtils), nameof(BinarySearchForNGI), typeof(IList<T>), typeof(int), typeof(int),
                    typeof(T), typeof(IComparer<T>)).MakeGenericMethod(typeof(T)));
                IL.Emit.Ret();
                throw IL.Unreachable();
#endif
            }
#if DEBUG
            return BinarySearchForNGI(list, startIndex, middleIndex, item, comparer);
#else
            IL.Push(list);
            IL.Push(startIndex);
            IL.Push(middleIndex);
            IL.Push(item);
            IL.Push(comparer);
            IL.Emit.Tail();
            IL.Emit.Call(new MethodRef(typeof(SortUtils), nameof(BinarySearchForNGI), typeof(IList<T>), typeof(int), typeof(int),
                typeof(T), typeof(IComparer<T>)).MakeGenericMethod(typeof(T)));
            IL.Emit.Ret();
            throw IL.Unreachable();
#endif
        }
    }
}
