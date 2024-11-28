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

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe T* BinarySearchForNGI<T>(T* ptr, T* ptrEnd, T item) //NGI = Nearest Greater Item
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
            PackedPrimitive<T> packedItem = item;
            PackedPrimitive<T> packedMiddle = *ptrMiddle;
            if (packedItem == packedMiddle)
                return ptrMiddle + 1;
            if (packedItem > packedMiddle)
            {
#if DEBUG
                return BinarySearchForNGI(ptrMiddle + 1, ptrEnd, item);
#else
                IL.Push(ptrMiddle + 1);
                IL.Push(ptrEnd);
                IL.Push(item);
                IL.Emit.Tail();
                IL.Emit.Call(new MethodRef(typeof(SortUtils), nameof(BinarySearchForNGI), typeof(T*), typeof(T*), typeof(T))
                    .MakeGenericMethod(typeof(T)));
                IL.Emit.Ret();
                throw IL.Unreachable();
#endif
            }
#if DEBUG
            return BinarySearchForNGI(ptr, ptrMiddle, item);
#else
            IL.Push(ptr);
            IL.Push(ptrMiddle);
            IL.Push(item);
            IL.Emit.Tail();
            IL.Emit.Call(new MethodRef(typeof(SortUtils), nameof(BinarySearchForNGI), typeof(T*), typeof(T*), typeof(T))
                .MakeGenericMethod(typeof(T)));
            IL.Emit.Ret();
            throw IL.Unreachable();
#endif
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
