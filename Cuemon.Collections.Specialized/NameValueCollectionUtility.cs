using System;
using System.Collections.Specialized;
using Cuemon.Collections.Generic;

namespace Cuemon.Collections.Specialized
{
    /// <summary>
    /// This utility class provides a set of concrete static methods for supporting the <see cref="EnumerableUtility"/>.
    /// </summary>
    public static class NameValueCollectionUtility
    {
        /// <summary>
        /// Determines whether the specified <paramref name="c"/> contains an entry with the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="c">The <see cref="NameValueCollection"/> to search for <paramref name="key"/>.</param>
        /// <param name="key">The key to locate in <paramref name="c"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="c"/> contains an entry with the <paramref name="key"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>This method performs an <see cref="StringComparison.OrdinalIgnoreCase"/> search for <paramref name="key"/>.</remarks>
        public static bool ContainsKey(NameValueCollection c, string key)
        {
            if (c == null) { return false; }
            if (key == null) { return false; }
            if (c.Get(key) == null)
            {
                return EnumerableUtility.Contains(c.AllKeys, key, StringComparer.OrdinalIgnoreCase);
            }
            return true;
        }
    }
}