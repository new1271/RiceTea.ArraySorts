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

            ArraySortsConfig.MemoryAllocator = new Win32MemoryAllocator();

            Console.WriteLine("--------------------");
            Console.WriteLine("T[] form:");

            DoTest(sequence, referenceSequence, SortFunction.IntroSort);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.MergeSort);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.BinaryInsertionSort);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.InsertionSort);
            GC.Collect();

            Console.WriteLine("--------------------");
            Console.WriteLine("List<T> form:");

            DoTestInList(sequence, referenceSequence, SortFunction.IntroSort);
            GC.Collect();
            DoTestInList(sequence, referenceSequence, SortFunction.MergeSort);
            GC.Collect();
            DoTestInList(sequence, referenceSequence, SortFunction.BinaryInsertionSort);
            GC.Collect();
            DoTestInList(sequence, referenceSequence, SortFunction.InsertionSort);
            GC.Collect();

            Console.WriteLine("--------------------");

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

        private static void DoTest<T>(T[] sequence, T[] referenceSequence, SortFunction function) where T : unmanaged
        {
            IComparer<T> comparer = Comparer<T>.Default;
            T[] testSequence = sequence.Clone() as T[];
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
            Console.WriteLine(name + "() cost " + stopwatch.ElapsedMilliseconds.ToString() + "." +
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
        
        private static void DoTestInList<T>(T[] sequence, T[] referenceSequence, SortFunction function) where T : unmanaged
        {
            IComparer<T> comparer = Comparer<T>.Default;
            List<T> testSequence = new List<T>(sequence);
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
            Console.WriteLine(name + "() cost " + stopwatch.ElapsedMilliseconds.ToString() + "." +
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
