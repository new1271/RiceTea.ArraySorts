using RiceTea.ArraySorts.Config;

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RiceTea.ArraySorts.Tester
{
    internal static class Program
    {
        private static int[] _sequence = null;

        private enum SortFunction
        {
            IntroSort,
            QuickSort,
            MergeSort,
            InsertionSort,
            BinaryInsertionSort,
        }

        public static void Main(string[] args)
        {
            Console.Write("Please input the length of test sequence (Default is 64): ");
            int count = int.TryParse(Console.ReadLine(), out int result) ? Math.Max(result, 1) : 64;
            //int count = 8152;

            int[] sequence = new int[count];
            int[] referenceSequence;
            _sequence = sequence;
            for (int i = 0; i < count; i++)
            {
                sequence[i] = i;
            }

            referenceSequence = sequence.Clone() as int[];

            //Array.Reverse(sequence);
            Shuffle(sequence);

            Func<int[], int[]> arrayCloneFunction = new Func<int[], int[]>(arr => arr.Clone() as int[]);
            Func<int[], List<int>> listCloneFunction = new Func<int[], List<int>>(arr => new List<int>(arr));

            ArraySortsConfig.MemoryAllocator = new Win32MemoryAllocator();

            Console.WriteLine("--------------------");
            Console.WriteLine("T[] form:");

            DoTest(sequence, referenceSequence, SortFunction.IntroSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.QuickSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.MergeSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.BinaryInsertionSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.InsertionSort, arrayCloneFunction);
            GC.Collect();

            Console.WriteLine("--------------------");
            Console.WriteLine("List<T> form:");

            DoTest(sequence, referenceSequence, SortFunction.IntroSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.QuickSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.MergeSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.BinaryInsertionSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.InsertionSort, listCloneFunction);
            GC.Collect();

            Console.WriteLine("--------------------");
            sequence = null;
            referenceSequence = null;
            GC.Collect();

            Console.Read();
        }

        private static void Shuffle<T>(T[] sequence)
        {
            Random random = new Random();
            for (int i = 0, length = sequence.Length; i < length; i++)
            {
                int swapIndex = random.Next(length);
                (sequence[i], sequence[swapIndex]) = (sequence[swapIndex], sequence[i]);
            }
        }

        private static void DoTest<T>(T[] sequence, T[] referenceSequence, SortFunction function, Func<T[], IList<T>> cloneFunction) where T : unmanaged
        {
            IComparer<T> comparer = Comparer<T>.Default;
            IList<T> testSequence = cloneFunction.Invoke(sequence);
            Stopwatch stopwatch = new Stopwatch();
            string name;
            switch (function)
            {
                case SortFunction.IntroSort:
                    name = nameof(ArraySorts.IntroSort);
#if !DEBUG
                    ArraySorts.IntroSort(referenceSequence, comparer);
#endif
                    stopwatch.Restart();
                    ArraySorts.IntroSort(testSequence, comparer);
                    stopwatch.Stop();
                    break;
                case SortFunction.QuickSort:
                    name = nameof(ArraySorts.QuickSort);
#if !DEBUG
                    ArraySorts.QuickSort(referenceSequence, comparer);
#endif
                    stopwatch.Restart();
                    ArraySorts.QuickSort(testSequence, comparer);
                    stopwatch.Stop();
                    break;
                case SortFunction.MergeSort:
                    name = nameof(ArraySorts.MergeSort);
#if !DEBUG
                    ArraySorts.MergeSort(referenceSequence, comparer);
#endif
                    stopwatch.Restart();
                    ArraySorts.MergeSort(testSequence, comparer);
                    stopwatch.Stop();
                    break;
                case SortFunction.InsertionSort:
                    name = nameof(ArraySorts.InsertionSort);
#if !DEBUG
                    ArraySorts.InsertionSort(referenceSequence, comparer);
#endif
                    stopwatch.Restart();
                    ArraySorts.InsertionSort(testSequence, comparer);
                    stopwatch.Stop();
                    break;
                case SortFunction.BinaryInsertionSort:
                    name = nameof(ArraySorts.BinaryInsertionSort);
#if !DEBUG
                    ArraySorts.BinaryInsertionSort(referenceSequence, comparer);
#endif
                    stopwatch.Restart();
                    ArraySorts.BinaryInsertionSort(testSequence, comparer);
                    stopwatch.Stop();
                    break;
                default:
                    throw new NotImplementedException();
            }
            Console.WriteLine(name + "() spends " + stopwatch.ElapsedMilliseconds.ToString() + "." +
                (stopwatch.ElapsedTicks % TimeSpan.TicksPerMillisecond).ToString("0000") + " ms to sort the sequence!");
            Console.WriteLine("checking...");
            for (int i = 0, length = referenceSequence.Length; i < length; i++)
            {
                if (comparer.Compare(testSequence[i], referenceSequence[i]) != 0)
                {
                    Console.WriteLine("item mismatch in " + i.ToString() + " of " + length.ToString() + " !");
                    return;
                }
            }
            Console.WriteLine("complete match!");
            return;
        }
    }
}
