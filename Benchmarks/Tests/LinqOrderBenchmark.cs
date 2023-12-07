using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;

namespace Benchmarks.Tests
{
    [MemoryDiagnoser]
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    public class LinqOrderBenchmark
    {
        private static readonly List<(int[] Numbers, int Number)> data;
        private static readonly LinqOrder LinqOrder = new();

        static LinqOrderBenchmark()
        {
            data = ReadData();
        }

        [Benchmark]
        public void OrderByOnly()
        {
            LinqOrder.OrderByOnly(data);
        }

        [Benchmark]
        public void OrderByAndThenBy()
        {
            LinqOrder.OrderByAndThenBy(data);
        }

        [Benchmark]
        public void OrderByOnlyWithTuple()
        {
            LinqOrder.OrderByOnlyWithTuple(data);
        }

        [Benchmark]
        public void OrderByAndThenByWithTuple()
        {
            LinqOrder.OrderByAndThenByWithTuple(data);
        }

        public static List<(int[] Numbers, int Number)> ReadData()
        {
            var input = File.ReadAllText($"Resources\\LinqOrderInput.txt");

            return input.Split(Environment.NewLine)
                        .Select(x => x.Split(" "))
                        .Select(x => (Cards: x[0].Select(s => (int)s).ToArray(), Score: int.Parse(x[1].Replace(" ", string.Empty))))
                        .ToList();
        }
    }
}
