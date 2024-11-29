using InlineMethod;

using RiceTea.ArraySorts.Internal.IntroSort;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts
{
    partial class ArraySorts
    {
        /// <summary>
        /// Sorts the elements with IntroSort algorithm in an entire <see cref="IList{T}" /> using the <see cref="IComparable{T}"/> generic interface implementation of each element of the <see cref="IList{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list)
        {
            int count = CheckArgumentsAndReturnCount(list);
            IntroSortCore(list, 0, count, null);
        }

        /// <summary>
        /// Sorts the elements with IntroSort algorithm in an <see cref="IList{T}" /> using the specified <see cref="IComparer{T}"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparer">The <see cref=" IComparer{T}"/> generic interface implementation to use when comparing elements, <br/>
        /// or <see langword="null"/> to use the <see cref="IComparable{T}"/> generic interface implementation of each element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list, IComparer<T> comparer)
        {
            int count = CheckArgumentsAndReturnCount(list);
            IntroSortCore(list, 0, count, comparer);
        }

        /// <summary>
        /// Sorts the elements with IntroSort algorithm in an <see cref="IList{T}" /> using the specified <see cref="Comparison{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list, Comparison<T> comparison)
        {
            int count = CheckArgumentsAndReturnCount(list, comparison);
            IntroSortCore(list, 0, count, Comparer<T>.Create(comparison));
        }

        /// <summary>
        /// Sorts the elements in a range of elements in an <see cref="IList{T}" /> using the <see cref="IComparable{T}"/> generic interface implementation of each element of the <see cref="IList{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="count">The number of elements in the range to sort.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list, int index, int count)
        {
            CheckArguments(list, index, count);
            IntroSortCore(list, 0, count, null);
        }

        /// <summary>
        /// Sorts the elements in a range of elements in an <see cref="IList{T}" /> using the specified <see cref="IComparer{T}"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="count">The number of elements in the range to sort.</param>
        /// <param name="comparer">The <see cref=" IComparer{T}"/> generic interface implementation to use when comparing elements, <br/>
        /// or <see langword="null"/> to use the <see cref="IComparable{T}"/> generic interface implementation of each element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list, int index, int count, IComparer<T> comparer)
        {
            CheckArguments(list, index, count);
            IntroSortCore(list, 0, count, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void IntroSortCore<T>(IList<T> list, int index, int count, IComparer<T> comparer)
        {
            if (count <= 0)
                return;
            IntroSortImpl.Sort(list, index, count, comparer);
        }
    }
}
