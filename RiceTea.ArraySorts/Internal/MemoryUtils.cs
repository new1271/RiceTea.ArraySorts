using InlineMethod;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal
{
    internal static class MemoryUtils
    {
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
    }
}
