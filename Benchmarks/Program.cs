using BenchmarkDotNet.Running;
using Benchmarks.Tests;

BenchmarkRunner.Run<LinqOrderBenchmark>();

//var list = LinqOrderBenchmark.ReadData();
//var lo = new LinqOrder();

//var r1 = lo.OrderByOnly(list);
//var r2 = lo.OrderByAndThenBy(list);
//var r3 = lo.OrderByOnlyWithTuple(list);
//var r4 = lo.OrderByAndThenByWithTuple(list);

//bool success = true;
//for (int i = 0; i < list.Count; i++)
//{
//    if (r1[i] != r2[i] || r1[i] != r3[i] || r1[i] != r4[i])
//        success = false;
//}

//Console.WriteLine(success);