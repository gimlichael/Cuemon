using System;
using System.Linq;
using System.Net.Http.Headers;
using Cuemon.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Cuemon.AspNetCore.Http.Headers
{
    /// <summary>
    /// Extension methods for the <see cref="IHeaderDictionary"/> interface tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class HeaderDictionaryDecoratorExtensions
    {
        /// <summary>
        /// Attempts to add or update an existing element with the provided key and value to the enclosed <see cref="IHeaderDictionary"/> of the <paramref name="decorator"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="key">The string to use as the key of the element to add.</param>
        /// <param name="value">The string to use as the value of the element to add.</param>
        /// <param name="useAsciiEncodingConversion">if set to <c>true</c> an ASCII encoding conversion is applied to the <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static bool TryAddOrUpdateHeader(this IDecorator<IHeaderDictionary> decorator, string key, StringValues value, bool useAsciiEncodingConversion = true)
        {
            var headerValue = useAsciiEncodingConversion ? new StringValues(Decorator.Enclose<string>(value).ToAsciiEncodedString()) : value;
            if (headerValue != StringValues.Empty)
            {
                return decorator.TryAddOrUpdate(key, Decorator.Enclose(headerValue.ToString().Where(c => !char.IsControl(c))).ToStringEquivalent());
            }
            return false;
        }

        /// <summary>
        /// Attempts to add or update one or more elements from the provided collection of <paramref name="responseHeaders"/> to the enclosed <see cref="IHeaderDictionary"/> of the <paramref name="decorator"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="responseHeaders">The <see cref="HttpResponseHeaders"/> to copy.</param>
        public static void TryAddOrUpdateHeaders(this IDecorator<IHeaderDictionary> decorator, HttpResponseHeaders responseHeaders)
        {
            if (decorator == null || responseHeaders == null) { return; }
            foreach (var header in responseHeaders)
            {
                decorator.TryAddOrUpdate(header.Key, header.Value != null ? DelimitedString.Create(header.Value) : "");
            }
        }
    }
}