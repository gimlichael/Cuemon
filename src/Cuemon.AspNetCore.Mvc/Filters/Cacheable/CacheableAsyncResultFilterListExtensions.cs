using System;
using System.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Extension methods for the <see cref="IList{ICacheableAsyncResultFilter}"/> interface.
    /// </summary>
    public static class CacheableAsyncResultFilterListExtensions
    {
        /// <summary>
        /// Adds a HTTP related filter to the list.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ICacheableAsyncResultFilter"/>.</typeparam>
        /// <typeparam name="TOptions">The type of delegate setup to configure <typeparamref name="T"/>.</typeparam>
        /// <param name="filters">The list of cache related HTTP head filters.</param>
        /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
        public static void AddFilter<T, TOptions>(this IList<ICacheableAsyncResultFilter> filters, Action<TOptions> setup) 
            where T : ICacheableAsyncResultFilter
            where TOptions : class, new()
        {
            filters.Add(ActivatorUtility.CreateInstance<Action<TOptions>, T>(setup));
        }

        /// <summary>
        /// Inserts a HTTP related filter to the list at the specified <paramref name="index" />.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="ICacheableAsyncResultFilter"/>.</typeparam>
        /// <typeparam name="TOptions">The type of delegate setup to configure <typeparamref name="T"/>.</typeparam>
        /// <param name="filters">The list of cache related HTTP head filters.</param>
        /// <param name="index">The zero-based index at which a HTTP related filter should be inserted.</param>
        /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
        public static void InsertFilter<T, TOptions>(this IList<ICacheableAsyncResultFilter> filters, int index, Action<TOptions> setup) 
            where T : ICacheableAsyncResultFilter
            where TOptions : class, new()
        {
            filters.Insert(index, ActivatorUtility.CreateInstance<Action<TOptions>, T>(setup));
        }

        /// <summary>
        /// Adds an <see cref="HttpEntityTagHeader" /> filter to the list.
        /// </summary>
        /// <param name="filters">The list of cache related HTTP filters.</param>
        /// <param name="setup">The <see cref="HttpEntityTagHeaderOptions"/> which need to be configured.</param>
        public static void AddEntityTagHeaderHeader(this IList<ICacheableAsyncResultFilter> filters, Action<HttpEntityTagHeaderOptions> setup = null)
        {
            filters.AddFilter<HttpEntityTagHeader, HttpEntityTagHeaderOptions>(setup);
        }

        /// <summary>
        /// Adds an <see cref="HttpLastModifiedHeader"/> filter to the list.
        /// </summary>
        /// <param name="filters">The list of cache related HTTP filters.</param>
        /// <param name="setup">The <see cref="HttpLastModifiedHeaderOptions"/> which need to be configured.</param>
        public static void AddLastModifiedHeader(this IList<ICacheableAsyncResultFilter> filters, Action<HttpLastModifiedHeaderOptions> setup = null)
        {
            filters.InsertFilter<HttpLastModifiedHeader, HttpLastModifiedHeaderOptions>(0, setup);
        }
    }
}