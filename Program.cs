using BenchmarkDotNet.Running;
using SpanVSList;

var summary = BenchmarkRunner.Run<SpanVsListBenchmark>();
