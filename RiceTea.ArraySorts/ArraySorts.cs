using InlineMethod;

using RiceTea.ArraySorts.Internal;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts
{
    public static partial class ArraySorts
    {
        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckArguments<T>(IList<T> list, int index, int count)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), $"'{nameof(index)}' is less than zero.");
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), $"'{nameof(count)}' is less than zero.");
            if (list.Count - index < count)
                throw new ArgumentException($"'{nameof(index)}' and '{nameof(count)}' do not specify a valid range in '{nameof(list)}'.");
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CheckArgumentsAndReturnCount<T>(IList<T> list)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            return list.Count;
        }
    }
}
