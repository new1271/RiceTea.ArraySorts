using InlineMethod;

using RiceTea.ArraySorts.Internal;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts
{
    partial class ArraySorts
    {
        /// <inheritdoc cref="IntroSort{T}(IList{T}, int, int, IComparer{T})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list)
        {
            int count = CheckArgumentsAndReturnCount(list);
            IntroSortCore(list, 0, count, null);
        }

        /// <inheritdoc cref="IntroSort{T}(IList{T}, int, int, IComparer{T})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list, IComparer<T> comparer)
        {
            int count = CheckArgumentsAndReturnCount(list);
            IntroSortCore(list, 0, count, comparer);
        }

        /// <inheritdoc cref="IntroSort{T}(IList{T}, int, int, IComparer{T})"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IntroSort<T>(IList<T> list, int index, int count)
        {
            CheckArguments(list, index, count);
            IntroSortCore(list, 0, count, null);
        }

        /// <summary>
        /// Sorts the elements in <see cref="IList{T}" /> with IntroSort algorithm.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="list">The list to sort.</param>
        /// <param name="index">The starting index of the range to sort.</param>
        /// <param name="count">The number of elements in the range to sort.</param>
        /// <param name="comparer">The <see cref=" IComparer{T}"/> generic interface implementation to use when comparing elements, <br/>
        /// or <see cref="null"/> to use the <see cref="IComparable{T}"/> generic interface implementation of each element.</param>
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
            switch (list)
            {
                case T[] array:
                    Array.Sort(array, index, count, comparer);
                    return;
                case List<T> _list:
                    _list.Sort(index, count, comparer);
                    return;
                case IList _list:
                    {
                        ArrayList arrayList = ArrayList.Adapter(_list);
                        if (comparer is null)
                        {
                            arrayList.Sort(index, count, null);
                            return;
                        }
                        if (comparer is IComparer objectComparer)
                        {
                            arrayList.Sort(index, count, objectComparer);
                            return;
                        }
                        arrayList.Sort(index, count, new GenericComparerWrapper<T>(comparer));
                    }
                    return;
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
