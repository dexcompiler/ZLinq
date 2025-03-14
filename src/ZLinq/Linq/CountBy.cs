﻿namespace ZLinq
{
    partial class ValueEnumerableExtensions
    {
        // same as AggregateBy, use Dictionary but we should remove null constraints.
        // https://github.com/dotnet/runtime/issues/98259

        public static CountBy<TEnumerable, TSource, TKey> CountBy<TEnumerable, TSource, TKey>(this TEnumerable source, Func<TSource, TKey> keySelector)
            where TEnumerable : struct, IValueEnumerable<TSource>
#if NET9_0_OR_GREATER
            , allows ref struct
#endif
            where TKey : notnull
            => new(source, keySelector, null);

        public static CountBy<TEnumerable, TSource, TKey> CountBy<TEnumerable, TSource, TKey>(this TEnumerable source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? keyComparer)
            where TEnumerable : struct, IValueEnumerable<TSource>
#if NET9_0_OR_GREATER
            , allows ref struct
#endif
            where TKey : notnull
            => new(source, keySelector, keyComparer);
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
    struct CountBy<TEnumerable, TSource, TKey>(TEnumerable source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey>? keyComparer)
        : IValueEnumerable<KeyValuePair<TKey, int>>
        where TEnumerable : struct, IValueEnumerable<TSource>
#if NET9_0_OR_GREATER
        , allows ref struct
#endif
        where TKey : notnull
    {
        TEnumerable source = source;

        Dictionary<TKey, int>? dictionary;
        Dictionary<TKey, int>.Enumerator enumerator;

        public ValueEnumerator<CountBy<TEnumerable, TSource, TKey>, KeyValuePair<TKey, int>> GetEnumerator() => new(this);

        public bool TryGetNonEnumeratedCount(out int count)
        {
            count = 0;
            return false;
        }

        public bool TryGetSpan(out ReadOnlySpan<KeyValuePair<TKey, int>> span)
        {
            span = default;
            return false;
        }


        public bool TryCopyTo(Span<KeyValuePair<TKey, int>> destination)
        {
            destination = default;
            return false;
        }

        public bool TryGetNext(out KeyValuePair<TKey, int> current)
        {
            if (dictionary == null)
            {
                // if empty, don't create dictionary.
                if (source.TryGetNonEnumeratedCount(out var count) && count == 0)
                {
                    Unsafe.SkipInit(out current);
                    return false;
                }

                Initialize();
            }

            if (enumerator.MoveNext())
            {
                current = enumerator.Current;
                return true;
            }

            Unsafe.SkipInit(out current);
            return false;
        }

        void Initialize()
        {
            var dict = keyComparer != null
                ? new Dictionary<TKey, int>(keyComparer)
                : new Dictionary<TKey, int>();

            using (source)
            {
                if (source.TryGetSpan(out var span))
                {
                    foreach (var item in span)
                    {
                        var key = keySelector(item);
#if NETSTANDARD2_1
                        if (dict.TryGetValue(key, out var count))
                        {
                            dict[key] = checked(count + 1);
                        }
                        else
                        {
                            dict[key] = 1;
                        }
#else
                        ref var count = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
                        checked { count += 1; }
#endif
                    }
                }
                else
                {
                    while (source.TryGetNext(out var item))
                    {
                        var key = keySelector(item);
#if NETSTANDARD2_1
                        if (dict.TryGetValue(key, out var count))
                        {
                            dict[key] = checked(count + 1);
                        }
                        else
                        {
                            dict[key] = 1;
                        }
#else
                        ref var count = ref CollectionsMarshal.GetValueRefOrAddDefault(dict, key, out var exists);
                        checked { count += 1; }
#endif
                    }
                }
            }

            dictionary = dict;
            enumerator = dictionary.GetEnumerator();
        }

        public void Dispose()
        {
            if (dictionary != null)
            {
                enumerator.Dispose();
            }
            source.Dispose();
        }
    }

}
