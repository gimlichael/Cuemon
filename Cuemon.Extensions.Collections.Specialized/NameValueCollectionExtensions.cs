using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Cuemon.Extensions.Collections.Specialized
{
    /// <summary>
    /// Extension methods for the <see cref="NameValueCollection"/> class.
    /// </summary>
    public static class NameValueCollectionExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="nvc"/> contains an entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="nvc">The <see cref="NameValueCollection"/> to search for <paramref name="key"/>.</param>
        /// <param name="key">The key to locate in <paramref name="nvc"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="nvc"/> contains an entry with the <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>This method performs an <see cref="StringComparison.OrdinalIgnoreCase"/> search for <paramref name="key"/>.</remarks>
        public static bool ContainsKey(this NameValueCollection nvc, string key)
        {
            if (nvc == null) { return false; }
            if (key == null) { return false; }
            if (nvc.Get(key) == null)
            {
                return nvc.AllKeys.Contains(key, StringComparer.OrdinalIgnoreCase);
            }
            return true;
        }

        /// <summary>
        /// Creates a <see cref="IDictionary{TKey,TValue}"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">A <see cref="NameValueCollection"/> to convert into an <see cref="IDictionary{TKey,TValue}"/> equivalent.</param>
        /// <returns>A <see cref="IDictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/>.</returns>
        public static IDictionary<string, string[]> ToDictionary(this NameValueCollection source)
        {
            Validator.ThrowIfNull(source, nameof(source));
            var result = new Dictionary<string, string[]>();
            foreach (string item in source)
            {
                result.Add(item, source[item].Split(','));
            }
            return result;
        }
    }
}