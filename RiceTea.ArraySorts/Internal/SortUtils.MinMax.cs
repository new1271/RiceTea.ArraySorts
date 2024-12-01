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
    unsafe partial class SortUtils
    {
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
        [Inline(InlineBehavior.Remove)]
        public static T Min<T>(T a, T b)
        {
            if (new PackedPrimitive<T>(a) > b)
                return b;
            return a;
        }

        [Inline(InlineBehavior.Remove)]
        public static T Min<T>(T a, T b, IComparer<T> comparer)
        {
            if (comparer.Compare(a, b) > 0)
                return b;
            return a;
        }

        [Inline(InlineBehavior.Remove)]
        public static T* Min<T>(T* a, T* b)
        {
            if (new PackedPrimitive<T>(*a) > *b)
                return b;
            return a;
        }

        [Inline(InlineBehavior.Remove)]
        public static T* Min<T>(T* a, T* b, IComparer<T> comparer)
        {
            if (comparer.Compare(*a, *b) > 0)
                return b;
            return a;
        }

        [Inline(InlineBehavior.Remove)]
        public static T Max<T>(T a, T b)
        {
            if (new PackedPrimitive<T>(a) < b)
                return b;
            return a;
        }

        [Inline(InlineBehavior.Remove)]
        public static T Max<T>(T a, T b, IComparer<T> comparer)
        {
            if (comparer.Compare(a, b) < 0)
                return b;
            return a;
        }

        [Inline(InlineBehavior.Remove)]
        public static T* Max<T>(T* a, T* b)
        {
            if (new PackedPrimitive<T>(*a) < *b)
                return b;
            return a;
        }

        [Inline(InlineBehavior.Remove)]
        public static T* Max<T>(T* a, T* b, IComparer<T> comparer)
        {
            if (comparer.Compare(*a, *b) < 0)
                return b;
            return a;
        }
    }
}
