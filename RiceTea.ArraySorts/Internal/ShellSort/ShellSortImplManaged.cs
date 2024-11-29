using InlineMethod;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.ShellSort
{
    internal static unsafe class ShellSortImplManaged<T>
    {
        public static void Sort(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int count = endIndex - startIndex;
            if (count < 2 || SortUtils.ShortCircuitSort(list, startIndex, count, comparer))
                return;
            SortInternal(list, startIndex, endIndex, comparer);
        }

        public static void SortWithoutCheck(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            SortInternal(list, startIndex, endIndex, comparer);
        }

        public static void SortOnce(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer, int step)
        {
            SortCore(list, startIndex, endIndex, comparer, step);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortInternal(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer)
        {
            int step = endIndex - startIndex;
            do
            {
                step >>= 1;
                SortCore(list, startIndex, endIndex, comparer, step);
            } while (step > 1);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(IList<T> list, int startIndex, int endIndex, IComparer<T> comparer, int step)
        {
            for (int i = 0; i < step; i++)
            {
                int startIndexInLoop = startIndex + i;
                for (int j = startIndexInLoop; j < endIndex; j += step)
                {
                    T item = list[j];
                    int reverseIndex;
                    for (reverseIndex = j - step; reverseIndex >= startIndexInLoop; reverseIndex -= step)
                    {
                        T itemComparing = list[reverseIndex];
                        int replaceIndex = reverseIndex + step;
                        if (comparer.Compare(item, itemComparing) < 0)
                        {
                            list[replaceIndex] = itemComparing;
                            continue;
                        }
                        if (replaceIndex == j)
                            break;
                        list[replaceIndex] = item;
                        break;
                    }
                    if (reverseIndex < startIndexInLoop && startIndexInLoop != j)
                    {
                        list[startIndexInLoop] = item;
                    }
                }
            }
        }
    }
}
