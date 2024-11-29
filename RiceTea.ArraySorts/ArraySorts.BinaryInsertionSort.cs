using InlineMethod;

using RiceTea.ArraySorts.Internal.BinaryInsertionSort;
using RiceTea.ArraySorts.Internal.QuickSort;

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts
{
    partial class ArraySorts
    {
        /// <summary>
        /// Sorts the elements with BinaryInsertionSort algorithm in an entire <see cref="Array" /> 
        /// using the <see cref="IComparable{T}"/> generic interface implementation of each element of the <see cref="Array" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(T[] array)
        {
            int length = CheckArgumentsAndReturnLength(array);
            BinaryInsertionSortCore(array, 0, length, null);
        }

        /// <summary>
        /// Sorts the elements with BinaryInsertionSort algorithm in an <see cref="Array" /> using the specified <see cref="IComparer{T}"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="comparer">The <see cref=" IComparer{T}"/> generic interface implementation to use when comparing elements, <br/>
        /// or <see langword="null"/> to use the <see cref="IComparable{T}"/> generic interface implementation of each element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(T[] array, IComparer<T> comparer)
        {
            int length = CheckArgumentsAndReturnLength(array);
            BinaryInsertionSortCore(array, 0, length, comparer);
        }

        /// <summary>
        /// Sorts the elements with BinaryInsertionSort algorithm in an <see cref="Array" /> using the specified <see cref="Comparison{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(T[] array, Comparison<T> comparison)
        {
            int length = CheckArgumentsAndReturnLength(array, comparison);
            BinaryInsertionSortCore(array, 0, length, Comparer<T>.Create(comparison));
        }

        /// <summary>
        /// Sorts the elements in a range of elements with BinaryInsertionSort algorithm in an <see cref="Array" /> 
        /// using the <see cref="IComparable{T}"/> generic interface implementation of each element of the <see cref="Array" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="length">The number of elements in the range to sort.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(T[] array, int index, int length)
        {
            CheckArguments(array, index, length);
            BinaryInsertionSortCore(array, 0, length, null);
        }

        /// <summary>
        /// Sorts the elements in a range of elements with BinaryInsertionSort algorithm in an <see cref="Array" /> using the specified <see cref="IComparer{T}"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The array to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="length">The number of elements in the range to sort.</param>
        /// <param name="comparer">The <see cref=" IComparer{T}"/> generic interface implementation to use when comparing elements, <br/>
        /// or <see langword="null"/> to use the <see cref="IComparable{T}"/> generic interface implementation of each element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(T[] array, int index, int length, IComparer<T> comparer)
        {
            CheckArguments(array, index, length);
            BinaryInsertionSortCore(array, 0, length, comparer);
        }

        /// <summary>
        /// Sorts the elements with BinaryInsertionSort algorithm in an entire <see cref="IList{T}" />
        /// using the <see cref="IComparable{T}"/> generic interface implementation of each element of the <see cref="IList{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(IList<T> list)
        {
            int count = CheckArgumentsAndReturnCount(list);
            BinaryInsertionSortCore(list, 0, count, null);
        }

        /// <summary>
        /// Sorts the elements with BinaryInsertionSort algorithm in an <see cref="IList{T}" /> using the specified <see cref="IComparer{T}"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparer">The <see cref=" IComparer{T}"/> generic interface implementation to use when comparing elements, <br/>
        /// or <see langword="null"/> to use the <see cref="IComparable{T}"/> generic interface implementation of each element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(IList<T> list, IComparer<T> comparer)
        {
            int count = CheckArgumentsAndReturnCount(list);
            BinaryInsertionSortCore(list, 0, count, comparer);
        }

        /// <summary>
        /// Sorts the elements with BinaryInsertionSort algorithm in an <see cref="IList{T}" /> using the specified <see cref="Comparison{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="comparison">The <see cref="Comparison{T}"/> to use when comparing elements.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(IList<T> list, Comparison<T> comparison)
        {
            int count = CheckArgumentsAndReturnCount(list, comparison);
            BinaryInsertionSortCore(list, 0, count, Comparer<T>.Create(comparison));
        }

        /// <summary>
        /// Sorts the elements in a range of elements with BinaryInsertionSort algorithm in an <see cref="IList{T}" /> 
        /// using the <see cref="IComparable{T}"/> generic interface implementation of each element of the <see cref="IList{T}" />.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="count">The number of elements in the range to sort.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(IList<T> list, int index, int count)
        {
            CheckArguments(list, index, count);
            BinaryInsertionSortCore(list, 0, count, null);
        }

        /// <summary>
        /// Sorts the elements in a range of elements with BinaryInsertionSort algorithm in an <see cref="IList{T}" /> using the specified <see cref="IComparer{T}"/> generic interface.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the list.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="count">The number of elements in the range to sort.</param>
        /// <param name="comparer">The <see cref=" IComparer{T}"/> generic interface implementation to use when comparing elements, <br/>
        /// or <see langword="null"/> to use the <see cref="IComparable{T}"/> generic interface implementation of each element.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void BinaryInsertionSort<T>(IList<T> list, int index, int count, IComparer<T> comparer)
        {
            CheckArguments(list, index, count);
            BinaryInsertionSortCore(list, 0, count, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void BinaryInsertionSortCore<T>(T[] array, int index, int length, IComparer<T> comparer)
        {
            if (length <= 0)
                return;
            BinaryInsertionSortImpl.Sort(array, index, length, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void BinaryInsertionSortCore<T>(IList<T> list, int index, int count, IComparer<T> comparer)
        {
            if (count <= 0)
                return;
            BinaryInsertionSortImpl.Sort(list, index, count, comparer);
        }
    }
}
