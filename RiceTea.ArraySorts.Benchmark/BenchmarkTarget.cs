using BenchmarkDotNet.Attributes;

using RiceTea.ArraySorts.Config;

using System;
using System.Collections.Generic;

namespace RiceTea.ArraySorts.Benchmark
{
    [RPlotExporter]
    public class BenchmarkTarget
    {
        private int[] _array;

        [ParamsSource(nameof(GenerateParams))]
        public int N;

        [GlobalSetup]
        public void Setup()
        {
            ArraySortsConfig.OptimizeSortedSequence = true;
            ArraySortsConfig.OptimizeTinySequenceSorting = true;

            int[] array = new int[N];
            for (int i = 0; i < N; i++)
            {
                array[i] = i;
            }

            Shuffle(array);
            _array = array;
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

        public IEnumerable<int> GenerateParams()
        {
            for (int i = 0, j = 256; i < 16; i++, j += 256)
            {
                yield return j;
            }
        }

        [Benchmark]
        public void IntroSort() => ArraySorts.IntroSort(_array.Clone() as int[]);

        [Benchmark]
        public void HeapSort() => ArraySorts.HeapSort(_array.Clone() as int[]);

        [Benchmark]
        public void QuickSort() => ArraySorts.QuickSort(_array.Clone() as int[]);

        [Benchmark]
        public void MergeSort() => ArraySorts.MergeSort(_array.Clone() as int[]);

        [Benchmark]
        public void InPlaceMergeSort() => ArraySorts.InPlaceMergeSort(_array.Clone() as int[]);

        [Benchmark]
        public void ShellSort() => ArraySorts.ShellSort(_array.Clone() as int[]);
    }
}
