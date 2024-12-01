using InlineMethod;

using RiceTea.Numerics;

using System.Runtime.CompilerServices;

namespace RiceTea.ArraySorts.Internal.ShellSort
{
#pragma warning disable CS8500 // 這會取得 Managed 類型的位址、大小，或宣告指向它的指標
    internal static unsafe class ShellSortImplUnmanagedNC<T>
    {
        public static void Sort(T* ptr, T* ptrEnd)
        {
            long count = ptrEnd - ptr;
            if (count < 2 || SortUtils.ShortCircuitSortNC(ptr, count))
                return;
            SortInternal(ptr, ptrEnd);
        }

        public static void SortWithoutCheck(T* ptr, T* ptrEnd)
        {
            SortInternal(ptr, ptrEnd);
        }

        public static void SortOnce(T* ptr, T* ptrEnd, long step)
        {
            SortCore(ptr, ptrEnd, step);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortInternal(T* ptr, T* ptrEnd)
        {
            long step = ptrEnd - ptr;
            do
            {
                step >>= 1;
                SortCore(ptr, ptrEnd, step);
            } while (step > 1);
        }

        [Inline(InlineBehavior.Remove)]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SortCore(T* ptr, T* ptrEnd, long step)
        {
            for (long i = 0; i < step; i++)
            {
                T* ptrStart = ptr + i;
                for (T* iterator = ptrStart; iterator < ptrEnd; iterator += step)
                {
                    T item = *iterator;
                    T* iteratorReverse;
                    for (iteratorReverse = iterator - step; iteratorReverse >= ptrStart; iteratorReverse -= step)
                    {
                        T itemComparing = *iteratorReverse;
                        T* replaceIterator = iteratorReverse + step;
                        if (new PackedPrimitive<T>(item) < itemComparing)
                        {
                            *replaceIterator = itemComparing;
                            continue;
                        }
                        if (replaceIterator == iterator)
                            break;
                        *replaceIterator = item;
                        break;
                    }
                    if (iteratorReverse < ptrStart && ptrStart < iterator)
                    {
                        *ptrStart = item;
                    }
                }
            }
        }
    }
}
