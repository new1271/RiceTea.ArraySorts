using InlineMethod;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts
{
    /// <summary>
    /// Sorts the elements in <see cref="IList{T}" /> with sorting algorithms.
    /// </summary>
    public static partial class ArraySorts
    {
        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void CheckArguments<T>(T[] array, int index, int length)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index), $"'{nameof(index)}' is less than zero.");
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), $"'{nameof(length)}' is less than zero.");
            if (array.Length - index < length)
                throw new ArgumentException($"'{nameof(index)}' and '{nameof(length)}' do not specify a valid range in '{nameof(array)}'.");
        }       
        
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
        private static int CheckArgumentsAndReturnLength<T>(T[] array)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            return array.Length;
        }
        
        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CheckArgumentsAndReturnCount<T>(IList<T> list)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            return list.Count;
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CheckArgumentsAndReturnLength<T>(T[] array, Comparison<T> comparison)
        {
            if (array is null)
                throw new ArgumentNullException(nameof(array));
            if (comparison is null)
                throw new ArgumentNullException(nameof(comparison));
            return array.Length;
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CheckArgumentsAndReturnCount<T>(IList<T> list, Comparison<T> comparison)
        {
            if (list is null)
                throw new ArgumentNullException(nameof(list));
            if (comparison is null)
                throw new ArgumentNullException(nameof(comparison));
            return list.Count;
        }
    }
}
