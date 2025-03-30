using System.Runtime.CompilerServices;
using ZLinq;
using ZLinq.Linq;

namespace ZLinq.Tests;


/// <summary>
/// Assert.Same/NotSame relating test is not supported.
/// </summary>
public static partial class Assert
{
    internal static void Same<T>(
        IEnumerable<T> expected,
        ValueEnumerable<Cast<FromEnumerable<object>, object, string>, string> actual)
    {
        throw new NotSupportedException("ZLinq use struct-based enumerator.");
    }

    internal static void Same<T>(
        IEnumerable<T> expected,
        ValueEnumerable<OfType<FromEnumerable<object>, object, T>, T> actual)
    {
        throw new NotSupportedException("ZLinq use struct-based enumerator.");
    }
}
