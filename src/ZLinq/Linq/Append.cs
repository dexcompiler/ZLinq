namespace ZLinq
{
    partial class ValueEnumerableExtensions
    {
        public static AppendValueEnumerable<TEnumerable, TSource> Append<TEnumerable, TSource>(this TEnumerable source, TSource element)
            where TEnumerable : struct, IValueEnumerable<TSource>
#if NET9_0_OR_GREATER
            , allows ref struct
#endif
            => new(source, element);
            
        public static ValueEnumerator<AppendValueEnumerable<TEnumerable, TSource>, TSource> GetEnumerator<TEnumerable, TSource>(this AppendValueEnumerable<TEnumerable, TSource> source)
            where TEnumerable : struct, IValueEnumerable<TSource>
#if NET9_0_OR_GREATER
            , allows ref struct
#endif
            => new(source);

    }
}

namespace ZLinq.Linq
{
    [StructLayout(LayoutKind.Auto)]
    [EditorBrowsable(EditorBrowsableState.Never)]
#if NET9_0_OR_GREATER
    public ref
#else
    public
#endif
    struct AppendValueEnumerable<TEnumerable, TSource>(TEnumerable source, TSource element)
        : IValueEnumerable<TSource>
        where TEnumerable : struct, IValueEnumerable<TSource>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
    {
        TEnumerable source = source;

        public bool TryGetNonEnumeratedCount(out int count) => throw new NotImplementedException();

        public bool TryGetSpan(out ReadOnlySpan<TSource> span)
        {
            throw new NotImplementedException();
            // span = default;
            // return false;
        }

        public bool TryGetNext(out TSource current)
        {
            throw new NotImplementedException();
            // Unsafe.SkipInit(out current);
            // return false;
        }

        public void Dispose()
        {
            source.Dispose();
        }
    }

}
