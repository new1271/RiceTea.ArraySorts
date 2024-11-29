using InlineMethod;

using RiceTea.ArraySorts.Internal.QuickSort;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts
{
    partial class ArraySorts
    {
        /// <inheritdoc cref="QuickSort{T}(IList{T}, int, int, IComparer{T})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuickSort<T>(IList<T> list)
        {
            int count = CheckArgumentsAndReturnCount(list);
            QuickSortCore(list, 0, count, null);
        }

        /// <inheritdoc cref="QuickSort{T}(IList{T}, int, int, IComparer{T})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuickSort<T>(IList<T> list, IComparer<T> comparer)
        {
            int count = CheckArgumentsAndReturnCount(list);
            QuickSortCore(list, 0, count, comparer);
        }

        /// <inheritdoc cref="QuickSort{T}(IList{T}, int, int, IComparer{T})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuickSort<T>(IList<T> list, int index, int count)
        {
            CheckArguments(list, index, count);
            QuickSortCore(list, 0, count, null);
        }

        /// <summary>
        /// Sorts the elements in <see cref="IList{T}" /> with QuickSort algorithm.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="count">The number of elements in the range to sort.</param>
        /// <param name="comparer">The <see cref=" IComparer{T}"/> generic interface implementation to use when comparing elements, <br/>
        /// or <see langword="null"/> to use the <see cref="IComparable{T}"/> generic interface implementation of each element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void QuickSort<T>(IList<T> list, int index, int count, IComparer<T> comparer)
        {
            CheckArguments(list, index, count);
            QuickSortCore(list, 0, count, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void QuickSortCore<T>(IList<T> list, int index, int count, IComparer<T> comparer)
        {
            if (count <= 0)
                return;
            QuickSortImpl.Sort(list, index, count, comparer);
        }
    }
}
