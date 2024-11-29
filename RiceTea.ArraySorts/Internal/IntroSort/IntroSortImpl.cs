using InlineMethod;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.IntroSort
{
    internal class IntroSortImpl
    {
        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(T[] array, int index, int count, IComparer<T> comparer) //Use CLR default implementation
        {
            Array.Sort(array, index, count, comparer);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Sort<T>(IList<T> list, int index, int count, IComparer<T> comparer) //Use CLR default implementation
        {
            switch (list)
            {
                case T[] array:
                    Sort(array, index, count, comparer);
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
