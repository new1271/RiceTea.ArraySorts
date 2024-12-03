using RiceTea.ArraySorts.Config;

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
            Array_Sort,
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

            SettingAlgorithmOptions();
            SettingTestData(out int[] sequence, out int[] referenceSequence);

            Func<int[], int[]> arrayCloneFunction = new Func<int[], int[]>(arr => arr.Clone() as int[]);
            Func<int[], List<int>> listCloneFunction = new Func<int[], List<int>>(arr => new List<int>(arr));

            Console.WriteLine("--------------------");
            Console.WriteLine("T[] form:");

            DoTest(sequence, referenceSequence, SortFunction.Array_Sort, arrayCloneFunction);
            GC.Collect();
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

            DoTest(sequence, referenceSequence, SortFunction.Array_Sort, listCloneFunction);
            GC.Collect();
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

        private static void SettingTestData(out int[] sequence, out int[] referenceSequence)
        {
            Console.WriteLine("★ Test data options:");
            Console.Write("→ Please input the length of test sequence (Default is 256): ");
            int count = int.TryParse(Console.ReadLine(), out int result) ? Math.Max(result, 1) : 256;
            sequence = new int[count];
            _sequence = sequence;
            for (int i = 0; i < count; i++)
            {
                sequence[i] = i;
            }

            referenceSequence = sequence.Clone() as int[];

            Console.Write("→ Which preprocessing for the test sequence you want? \n(0 = Shuffled, 1 = Sorted, 2 = Reverse-sorted, default is 0): ");
            switch (int.TryParse(Console.ReadLine(), out int selection) ? selection : 0)
            {
                case 0:
                    Array.Reverse(sequence);
                    Shuffle(sequence);
                    break;
                case 1:
                    break;
                case 2:
                    Array.Reverse(sequence);
                    break;
                default:
                    goto case 0;
            }
        }

        private static void SettingAlgorithmOptions()
        {
            Console.WriteLine("★ Algorithm options:");
            Console.Write("→ Optimize tiny sequence sorting? (T or F, default is F): ");
            if (Console.ReadLine().Trim().Equals("T", StringComparison.OrdinalIgnoreCase))
            {
                ArraySortsConfig.OptimizeTinySequenceSorting = true;
            }
            Console.Write("→ Optimize sorted sequence? (T or F, default is F): ");
            if (Console.ReadLine().Trim().Equals("T", StringComparison.OrdinalIgnoreCase))
            {
                ArraySortsConfig.OptimizeSortedSequence = true;
            }
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
                case SortFunction.Array_Sort:
                    {
                        name = nameof(Array) + "." + nameof(Array.Sort);
                        T[] testArray = testSequence as T[];
#if !DEBUG
                        Console.WriteLine("warm up...");
                        {
                            if (testArray is null)
                                (testSequence as List<T>)?.Sort(comparer);
                            else
                                Array.Sort(testArray, comparer);
                        }
                        testSequence = cloneFunction.Invoke(sequence);
                        testArray = testSequence as T[];
                        GC.Collect();
#endif
                        Console.WriteLine("testing...");
                        if (testArray is null)
                        {
                            if (testSequence is List<T> list)
                            {
                                stopwatch.Restart();
                                list.Sort(comparer);
                                stopwatch.Stop();
                            }
                        }
                        else
                        {
                            stopwatch.Restart();
                            Array.Sort(testArray, comparer);
                            stopwatch.Stop();
                        }
                    }
                    break;
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
