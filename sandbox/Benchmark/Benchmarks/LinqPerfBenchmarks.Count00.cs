﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Engines;
using Microsoft.VSDiagnostics;
using ZLinq;

namespace Benchmark;

public partial class LinqPerfBenchmarks
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    // [CPUUsageDiagnoser]
    public class Count00
    {
        const int IterationsCount00 = 1000000;
        List<Product> TestData = default!;

        [GlobalSetup]
        public void Setup() => TestData = Product.GetProductList();

        [Benchmark]
        [BenchmarkCategory(Categories.LINQ)]
        public bool Linq()
        {
            var products = TestData;
            int count = 0;
            for (int i = 0; i < IterationsCount00; i++)
            {
                count += products.Count(p => p.UnitsInStock == 0);
            }

            return (count == 5 * IterationsCount00);
        }

        [Benchmark]
        [BenchmarkCategory(Categories.ZLinq)]
        public bool ZLinq()
        {
            var products = TestData.AsValueEnumerable();
            int count = 0;
            for (int i = 0; i < IterationsCount00; i++)
            {
                count += products.Count(p => p.UnitsInStock == 0);
            }

            return (count == 5 * IterationsCount00);
        }
    }
}
