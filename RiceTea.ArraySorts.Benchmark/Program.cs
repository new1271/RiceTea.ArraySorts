using BenchmarkDotNet.Running;

namespace RiceTea.ArraySorts.Benchmark
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkTarget>();
        }
    }
}
