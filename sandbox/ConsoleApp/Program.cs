﻿#nullable enable
#pragma warning disable

using Microsoft.DiagnosticsHub;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using ZLinq;
using ZLinq.Linq;
using ZLinq.Simd;
using ZLinq.Traversables;

//Span<int> xs = stackalloc int[255];

// caseof bool, char, decimal, nint...

// var xs = new[] { 1, 2, 3, 4, 5 };

//byte.MaxValue
// 2147483647

[assembly: ZLinq.ZLinqDropInAttribute("MyApp", ZLinq.DropInGenerateTypes.Everything, DisableEmitSource = false)]


var first = new[] { 1, 2, 3, 4, 5, 6, 7, 8 };
var second = new[] { 10, 20, 30, 40, 50, 60, 70, 80 };
var third = new[] { 100, 200, 300, 400, 500, 600, 700, 800 };

var result = first.AsVectorizable().Zip(
    second,
    third,
    (v1, v2, v3) => v1 + v2 + v3,
    (x, y, z) => x + y + z
).ToArray();



Console.WriteLine(result.Length);


//var srcFiles = new DirectoryInfo("../../../../../src/ZLinq/Linq/").GetFiles();
//var tstFiles = new DirectoryInfo("../../../../../tests/ZLinq.Tests/Linq/").GetFiles();

//var grouping = srcFiles.AsValueEnumerable()
//    .LeftJoin(tstFiles,
//        x => x.Name,
//        x => x.Name.Replace("Test", ""),
//        (outer, inner) => new { Name = outer.Name, IsTested = inner != null })
//    .GroupBy(x => x.IsTested);

//foreach (var g in grouping)
//{
//    Console.WriteLine(g.Key ? "Tested::::::::::::::::::" : "NotTested::::::::::::::::::");
//    foreach (var item in g)
//    {
//        Console.WriteLine(item.Name);
//    }
//}


return;

var xssss = new[] { 1, 2, 3 };

// ValueEnumerable.Range(1, 10).Zip(xssss, xssss);

var root = new DirectoryInfo("C:\\Program Files (x86)\\Steam");

var allDlls = root
    .Descendants()
    .OfType<FileInfo>()
    .Where(x => x.Extension == ".dll");

var grouped = allDlls
    .GroupBy(x => x.Name)
    .Select(x => new { FileName = x.Key, Count = x.Count() })
    .OrderByDescending(x => x.Count);

foreach (var item in grouped)
{
    Console.WriteLine(item);
}

static IEnumerable<T> Iterate<T>(IEnumerable<T> source)
{
    foreach (var item in source)
    {
        yield return item;
    }
}


//Console.WriteLine(a);
//Console.WriteLine(b);

//Console.WriteLine(a == b);


// System.Text.Json's JsonNode is the target of LINQ to JSON(not JsonDocument/JsonElement).
var json = JsonNode.Parse("""
{
    "nesting": {
      "level1": {
        "description": "First level of nesting",
        "value": 100,
        "level2": {
          "description": "Second level of nesting",
          "flags": [true, false, true],
          "level3": {
            "description": "Third level of nesting",
            "coordinates": {
              "x": 10.5,
              "y": 20.75,
              "z": -5.0
            },
            "level4": {
              "description": "Fourth level of nesting",
              "metadata": {
                "created": "2025-02-15T14:30:00Z",
                "modified": null,
                "version": 2.1
              },
              "level5": {
                "description": "Fifth level of nesting",
                "settings": {
                  "enabled": true,
                  "threshold": 0.85,
                  "options": ["fast", "accurate", "balanced"],
                  "config": {
                    "timeout": 30000,
                    "retries": 3,
                    "deepSetting": {
                      "algorithm": "advanced",
                      "parameters": [1, 1, 2, 3, 5, 8, 13]
                    }
                  }
                }
              }
            }
          }
        }
      }
    }
}
""");

// JsonNode
var origin = json!["nesting"]!["level1"]!["level2"]!;

// JsonNode axis, Children, Descendants, Anestors, BeforeSelf, AfterSelf and ***Self.
foreach (var item in origin.Descendants().Select(x => x.Node).OfType<JsonArray>())
{
    // [true, false, true], ["fast", "accurate", "balanced"], [1, 1, 2, 3, 5, 8, 13]
    Console.WriteLine(item.ToJsonString(JsonSerializerOptions.Web));
}


class Person
{
    public string FirstName { get; }
    public string LastName { get; }
    public int Age { get; }

    public Person(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }
}

//Console.WriteLine(hoge.Length);





class A;
class B : A;





//foreach (var item in origin.Descendants().Where(x => x.Name == "hoge"))
//{
//    if (item.Node == null)
//    {
//        Console.WriteLine(item.Name);
//    }
//    else
//    {
//        Console.WriteLine(item.Node.GetPath() + ":" + item.Name);
//    }
//}

// je.RootElement.ValueKind == System.Text.Json.JsonValueKind.Object


namespace ZLinq
{
    public static class AutoInstrumentLinq
    {
        //public static SelectValueEnumerable<FromArray<TSource>, TSource, TResult> Select<TSource, TResult>(this TSource[] source, Func<TSource, TResult> selector)
        //{
        //    return source.AsValueEnumerable().Select(selector);
        //}

        //public static ConcatValueEnumerable2<RangeValueEnumerable, int, ArrayValueEnumerable<int>> Concat2(this RangeValueEnumerable source, ArrayValueEnumerable<int> second)
        //{
        //    return ValueEnumerableExtensions.Concat2<RangeValueEnumerable, int, ArrayValueEnumerable<int>>(source, second);
        //}
    }

    internal static partial class ZLinqTypeInferenceHelper
    {
        //public static TResult Sum<TResult>(this Select<FromArray<int>, int, int?> source, Func<int?, TResult> selector) where TResult : struct, INumber<TResult>
        //{
        //    return ValueEnumerableExtensions.Sum<Select<FromArray<int>, int, int?>, int?, TResult>(source, selector);
        //}
    }

    public static class Test
    {

    }
}
