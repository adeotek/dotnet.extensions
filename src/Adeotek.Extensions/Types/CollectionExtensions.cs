namespace Adeotek.Extensions.Types;

public static class CollectionExtensions
{
    public static IDictionary<TKey, TValue> EmptyIfNull<TKey, TValue>(this IDictionary<TKey, TValue>? dictionary) 
        where TKey : notnull => 
        dictionary ?? new Dictionary<TKey, TValue>();

    public static IEnumerable<T> EmptyIfNull<T>(this IEnumerable<T>? enumerable) => 
        enumerable ?? Enumerable.Empty<T>();

    public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> source, int size)
    {
        T[]? bucket = null;
        var count = 0;

        foreach (var item in source)
        {
            bucket ??= new T[size];
            bucket[count++] = item;

            if (count != size)
            { 
                continue; 
            }

            yield return bucket.Select(x => x);

            bucket = null;
            count = 0;
        }

        // Return the last bucket with all remaining elements
        if (bucket != null && count > 0)
        {
            Array.Resize(ref bucket, count);
            yield return bucket.Select(x => x);
        }
    }
}