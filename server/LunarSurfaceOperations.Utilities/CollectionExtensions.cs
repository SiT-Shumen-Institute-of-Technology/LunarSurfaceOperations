namespace Quantum.DMS.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <summary>
    /// Static class containing helper or extension methods for better work with collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Use this method to get an empty string if the extended one is null, empty or consists of whitespace characters only.
        /// </summary>
        [return: NotNull]
        public static string OrEmptyIfNull([AllowNull] this string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text;
        }

        /// <summary>
        /// Use this method to get an empty collection if the extended one is null. This may be useful in foreach statements in order to avoid unexpected exceptions.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> OrEmptyIfNull<T>([AllowNull] this IEnumerable<T> items) => items ?? new List<T>(capacity: 0);

        /// <summary>
        /// Use this method to get an empty dictionary if the extended one is null. This may be useful in foreach statements in order to avoid unexpected exceptions.
        /// </summary>
        [return: NotNull]
        public static IDictionary<TKey, TValue> OrEmptyIfNull<TKey, TValue>([AllowNull] this IDictionary<TKey, TValue> dictionary) => dictionary ?? new Dictionary<TKey, TValue>(capacity: 0);

        /// <summary>
        /// Use this method to shorten the syntax for iterating over all values that are not null.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> IgnoreNullValues<T>(this IEnumerable<T> items)
            where T : class
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            return items.Where(x => x != null);
        }
        
        /// <summary>
        /// Use this method to shorten the syntax for iterating over all values that are not null or whitespace.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<string> IgnoreNullOrWhitespaceValues(this IEnumerable<string> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            return items.Where(x => string.IsNullOrWhiteSpace(x) == false);
        }

        /// <summary>
        /// Use this method to shorten the syntax for iterating over all values that are not default.
        /// </summary>
        [return: NotNull]
        public static IEnumerable<T> IgnoreDefaultValues<T>(this IEnumerable<T> items)
            where T : struct, IEquatable<T>
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            return items.Where(x => x.Equals(default) == false);
        }

        /// <summary>
        /// Use this method to remove a single item from the passed <paramref name="source"/> that does fulfil the passed <paramref name="condition"/>.
        /// </summary>
        /// <param name="source">The original collection that should be modified.</param>
        /// <param name="condition">The condition that should be used to retrieve the item that should be removed.</param>
        /// <typeparam name="T">The type of the contained items within the collection.</typeparam>
        /// <returns>The modified collection.</returns>
        public static IEnumerable<T> RemoveSingle<T>(this IEnumerable<T> source, Predicate<T> condition)
        {
            if (source is null || condition is null)
                return source;

            var isRemoved = false;

            return source.Where(
                x =>
                {
                    if (isRemoved)
                        return true;

                    var fulfills = condition(x);
                    if (fulfills == false)
                        return true;

                    isRemoved = true;
                    return false;
                });
        }

        /// <summary>
        /// Use this method to ensure that a value collection within a dictionary exists against the requested key. 
        /// </summary>
        /// <param name="dictionary">The dictionary that should be used to execute the operation.</param>
        /// <param name="key">The key used to lookup whether or not such collection exists in the requested <paramref name="dictionary"/>.</param>
        /// <param name="list">The collection that can be newly created or an already existing one.</param>
        /// <typeparam name="TKey">The type of the keys stored within the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values stored as a collection against any of the keys for the dictionary.</typeparam>
        /// <returns>Returns true if the operation was executed successfully. Else, returns false.</returns>
        public static bool EnsureValueCollection<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, [NotNullWhen(true)] out List<TValue> list)
        {
            list = null;
            if (dictionary is null)
                return false;

            if (dictionary.TryGetValue(key, out list))
                return true;

            list = new List<TValue>();
            dictionary[key] = list;
            return true;
        }

        public static IEnumerable<T> ConcatenateWith<T>(this IEnumerable<T> original, IEnumerable<T> suffix)
        {
            foreach (var x in original.OrEmptyIfNull())
                yield return x;
            foreach (var x in suffix.OrEmptyIfNull())
                yield return x;
        }

        public static IEnumerable<T> ConcatenateWith<T>(this IEnumerable<T> original, T suffix) => original.ConcatenateWith(suffix.AsEnumerable());

        public static IEnumerable<T> AsEnumerable<T>(this T value) => new[] { value };
    }
}