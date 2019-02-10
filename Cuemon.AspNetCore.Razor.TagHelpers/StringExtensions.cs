using System;

namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    /// <summary>
    /// Extension methods for the <see cref="String"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Suffixes the <paramref name="source"/> with the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="source">The string to extend.</param>
        /// <param name="value">The value to suffix <paramref name="source"/>.</param>
        /// <returns>A string with a suffix <paramref name="value"/>.</returns>
        public static string SuffixWith(this string source, string value)
        {
            if (!source.EndsWith(value, StringComparison.OrdinalIgnoreCase)) { source = string.Concat(source, value); }
            return source;
        }

        /// <summary>
        /// Suffixes the <paramref name="source"/> with a forwarding slash.
        /// </summary>
        /// <param name="source">The string to extend.</param>
        /// <returns>A string with a suffix forwarding slash.</returns>
        public static string SuffixWithForwardingSlash(this string source)
        {
            return source.SuffixWith("/");
        }

        /// <summary>
        /// Prefixes the <paramref name="source"/> with the specified <paramref name="value"/>.
        /// </summary>
        /// <param name="source">The string to extend.</param>
        /// <param name="value">The value to prefix <paramref name="source"/>.</param>
        /// <returns>A string with a prefix <paramref name="value"/>.</returns>
        public static string PrefixWith(this string source, string value)
        {
            if (!source.StartsWith(value, StringComparison.OrdinalIgnoreCase)) { source = string.Concat(value, source); }
            return source;
        }
    }
}