﻿using RiceTea.ArraySorts.Config;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;

namespace RiceTea.ArraySorts.Tester
{
    internal static class Program
    {
        private static int[] _sequence = null;

        private enum SortFunction
        {
            IntroSort,
            HeapSort,
            QuickSort,
            MergeSort,
            InPlaceMergeSort,
            InsertionSort,
            BinaryInsertionSort,
            ShellSort,
        }

        public static void Main(string[] args)
        {
            GCSettings.LatencyMode = GCLatencyMode.Batch;

            Console.Write("Please input the length of test sequence (Default is 64): ");
            int count = int.TryParse(Console.ReadLine(), out int result) ? Math.Max(result, 1) : 64;

            Console.Write("Use optimize sorting? (T or F, default is F): ");
            if (Console.ReadLine().Trim().Equals("T", StringComparison.OrdinalIgnoreCase))
            {
                ArraySortsConfig.OptimizeSorting = true;
            }
            //int count = 8152;

            int[] sequence = new int[count];
            int[] referenceSequence;
            _sequence = sequence;
            for (int i = 0; i < count; i++)
            {
                sequence[i] = i;
            }

            referenceSequence = sequence.Clone() as int[];

            Array.Reverse(sequence);
            Shuffle(sequence);

            Func<int[], int[]> arrayCloneFunction = new Func<int[], int[]>(arr => arr.Clone() as int[]);
            Func<int[], List<int>> listCloneFunction = new Func<int[], List<int>>(arr => new List<int>(arr));

            Console.WriteLine("--------------------");
            Console.WriteLine("T[] form:");

            DoTest(sequence, referenceSequence, SortFunction.IntroSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.HeapSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.QuickSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.MergeSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.InPlaceMergeSort, arrayCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.ShellSort, arrayCloneFunction);
            GC.Collect();

            Console.WriteLine("--------------------");
            Console.WriteLine("List<T> form:");

            DoTest(sequence, referenceSequence, SortFunction.IntroSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.HeapSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.QuickSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.MergeSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.InPlaceMergeSort, listCloneFunction);
            GC.Collect();
            DoTest(sequence, referenceSequence, SortFunction.ShellSort, listCloneFunction);
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
            Console.WriteLine();
            IComparer<T> comparer = Comparer<T>.Default;
            IList<T> testSequence = cloneFunction.Invoke(sequence);
            Stopwatch stopwatch = new Stopwatch();
            string name;
            switch (function)
            {
                case SortFunction.IntroSort:
                    {
                        name = nameof(ArraySorts.IntroSort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                ArraySorts.IntroSort(testSequence, comparer);
                            else
                                ArraySorts.IntroSort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            stopwatch.Restart();
                            ArraySorts.IntroSort(testSequence, comparer);
                            stopwatch.Stop();
                        }
                        else
                        {
                            stopwatch.Restart();
                            ArraySorts.IntroSort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;               
                case SortFunction.HeapSort:
                    {
                        name = nameof(ArraySorts.HeapSort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                ArraySorts.HeapSort(testSequence, comparer);
                            else
                                ArraySorts.HeapSort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            stopwatch.Restart();
                            ArraySorts.HeapSort(testSequence, comparer);
                            stopwatch.Stop();
                        }
                        else
                        {
                            stopwatch.Restart();
                            ArraySorts.HeapSort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;
                case SortFunction.QuickSort:
                    {
                        name = nameof(ArraySorts.QuickSort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                ArraySorts.QuickSort(testSequence, comparer);
                            else
                                ArraySorts.QuickSort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            stopwatch.Restart();
                            ArraySorts.QuickSort(testSequence, comparer);
                            stopwatch.Stop();
                        }
                        else
                        {
                            stopwatch.Restart();
                            ArraySorts.QuickSort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;
                case SortFunction.MergeSort:
                    {
                        name = nameof(ArraySorts.MergeSort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                ArraySorts.MergeSort(testSequence, comparer);
                            else
                                ArraySorts.MergeSort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            stopwatch.Restart();
                            ArraySorts.MergeSort(testSequence, comparer);
                            stopwatch.Stop();
                        }
                        else
                        {
                            stopwatch.Restart();
                            ArraySorts.MergeSort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;
                case SortFunction.InPlaceMergeSort:
                    {
                        name = nameof(ArraySorts.InPlaceMergeSort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                ArraySorts.InPlaceMergeSort(testSequence, comparer);
                            else
                                ArraySorts.InPlaceMergeSort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            stopwatch.Restart();
                            ArraySorts.InPlaceMergeSort(testSequence, comparer);
                            stopwatch.Stop();
                        }
                        else
                        {
                            stopwatch.Restart();
                            ArraySorts.InPlaceMergeSort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;
                case SortFunction.InsertionSort:
                    {
                        name = nameof(ArraySorts.InsertionSort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                ArraySorts.InsertionSort(testSequence, comparer);
                            else
                                ArraySorts.InsertionSort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            stopwatch.Restart();
                            ArraySorts.InsertionSort(testSequence, comparer);
                            stopwatch.Stop();
                        }
                        else
                        {
                            stopwatch.Restart();
                            ArraySorts.InsertionSort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;
                case SortFunction.BinaryInsertionSort:
                    {
                        name = nameof(ArraySorts.BinaryInsertionSort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                ArraySorts.BinaryInsertionSort(testSequence, comparer);
                            else
                                ArraySorts.BinaryInsertionSort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            stopwatch.Restart();
                            ArraySorts.BinaryInsertionSort(testSequence, comparer);
                            stopwatch.Stop();
                        }
                        else
                        {
                            stopwatch.Restart();
                            ArraySorts.BinaryInsertionSort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;
                case SortFunction.ShellSort:
                    {
                        name = nameof(ArraySorts.ShellSort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                ArraySorts.ShellSort(testSequence, comparer);
                            else
                                ArraySorts.ShellSort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            stopwatch.Restart();
                            ArraySorts.ShellSort(testSequence, comparer);
                            stopwatch.Stop();
                        }
                        else
                        {
                            stopwatch.Restart();
                            ArraySorts.ShellSort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }
            Console.WriteLine(name + "() spent " + stopwatch.ElapsedMilliseconds.ToString() + "." +
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
