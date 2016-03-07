using System.Collections.Generic;
using System.Collections.Specialized;

namespace Cuemon.Collections.Specialized.Extensions
{
    /// <summary>
    /// This is an extension implementation of the <see cref="NameValueCollectionConverter"/> class.
    /// </summary>
    public static class NameValueCollectionConverterExtension
    {
        /// <summary>
        /// Creates a <see cref="NameValueCollection"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">An <see cref="IDictionary{TKey,TValue}"/> to convert into an <see cref="NameValueCollection"/> equivalent.</param>
        /// <returns>A <see cref="NameValueCollection"/> that is equivalent to the specified <paramref name="source"/>.</returns>
        public static NameValueCollection ToNameValueCollection(this IDictionary<string, string[]> source)
        {
            return NameValueCollectionConverter.FromDictionary(source);
        }

        /// <summary>
        /// Creates a <see cref="IDictionary{TKey,TValue}"/> from the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">A <see cref="NameValueCollection"/> to convert into an <see cref="IDictionary{TKey,TValue}"/> equivalent.</param>
        /// <returns>A <see cref="IDictionary{TKey,TValue}"/> that is equivalent to the specified <paramref name="source"/>.</returns>
        public static IDictionary<string, string[]> ToDictionary(this NameValueCollection source)
        {
            return NameValueCollectionConverter.ToDictionary(source);
        }
    }
}