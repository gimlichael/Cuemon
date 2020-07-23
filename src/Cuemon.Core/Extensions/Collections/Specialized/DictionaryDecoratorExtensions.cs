using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Cuemon.Collections.Specialized
{
    /// <summary>
    /// Extension methods for the <see cref="IDictionary{TKey,TValue}"/> interface tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class DictionaryDecoratorExtensions
    {
        /// <summary>
        /// Creates a <see cref="NameValueCollection"/> from the enclosed <see cref="T:IDictionary{string,string[]}"/> of the <paramref name="decorator"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="setup">The <see cref="T:DelimitedStringOptions{string}"/> which may be configured.</param>
        /// <returns>A <see cref="NameValueCollection"/> that is equivalent to the enclosed <see cref="T:IDictionary{string,string[]}"/> of the <paramref name="decorator"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static NameValueCollection ToNameValueCollection(this IDecorator<IDictionary<string, string[]>> decorator, Action<DelimitedStringOptions<string>> setup = null)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            var result = new NameValueCollection();
            foreach (var item in decorator.Inner)
            {
                result.Add(item.Key, DelimitedString.Create(item.Value, setup));
            }
            return result;
        }
    }
}