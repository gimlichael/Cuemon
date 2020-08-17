using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Runtime.Caching
{
    public sealed partial class CacheCollection
    {
        private const string MemoizationGroup = "Memoization";
        private const long NullHashCode = 854726591;

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<TResult> Memoize<TResult>(Func<TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<TResult> Memoize<TResult>(Func<TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<TResult> Memoize<TResult>(Func<TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<TResult> Memoize<TResult>(Func<TResult> method, Func<IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T, TResult> Memoize<T, TResult>(Func<T, TResult> method, Func<T, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, TResult> Memoize<T1, T2, TResult>(Func<T1, T2, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, TResult> Memoize<T1, T2, TResult>(Func<T1, T2, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, TResult> Memoize<T1, T2, TResult>(Func<T1, T2, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, TResult> Memoize<T1, T2, TResult>(Func<T1, T2, TResult> method, Func<T1, T2, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, TResult> Memoize<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, Func<T1, T2, T3, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, TResult> Memoize<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, Func<T1, T2, T3, T4, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, TResult> Memoize<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, TResult> Memoize<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, TResult> Memoize<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, TResult> Memoize<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, Func<T1, T2, T3, T4, T5, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, TResult> Memoize<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, TResult> Memoize<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, TResult> Memoize<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, TResult> Memoize<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method, Func<T1, T2, T3, T4, T5, T6, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">The function delegate that is used to assign dependencies to the memoized <paramref name="method"/>. When any dependency changes, the object becomes invalid and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="absoluteExpiration">The time at which the memoized function delegate expires and is removed from the cache.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, DateTime absoluteExpiration)
        {
            return MemoizeCore(method, absoluteExpiration, TimeSpan.Zero, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="slidingExpiration">The interval between the time the memoized function delegate was last accessed and the time at which that memoization expires. If this value is the equivalent of 20 minutes, the memoization expires and is removed from the cache 20 minutes after it was last accessed.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, TimeSpan slidingExpiration)
        {
            return MemoizeCore(method, DateTime.MaxValue, slidingExpiration, null);
        }

        /// <summary>
        /// Memoizes the specified <paramref name="method"/> in the cache for fast access.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="method"/>.</typeparam>
        /// <param name="method">The function delegate that is invoked once and then stored in cache for fast access.</param>
        /// <param name="dependencyResolver">Establishes one or more <see cref="Dependency"/> relations to this memoized function delegate.</param>
        /// <returns>A memoized function delegate that is otherwise equivalent to <paramref name="method"/>.</returns>
        public Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> Memoize<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<IDependency>> dependencyResolver)
        {
            return MemoizeCore(method, DateTime.MaxValue, TimeSpan.Zero, dependencyResolver);
        }

        private Func<TResult> MemoizeCore<TResult>(Func<TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate
            {
                string key = CalculateCompositeKey(method);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T, TResult> MemoizeCore<T, TResult>(Func<T, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T arg)
            {
                string key = CalculateCompositeKey(method, arg);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, TResult> MemoizeCore<T1, T2, TResult>(Func<T1, T2, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2)
            {
                string key = CalculateCompositeKey(method, arg1, arg2);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, T3, TResult> MemoizeCore<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, T3, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2, T3 arg3)
            {
                string key = CalculateCompositeKey(method, arg1, arg2, arg3);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2, arg3);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2, arg3);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, T3, T4, TResult> MemoizeCore<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4)
            {
                string key = CalculateCompositeKey(method, arg1, arg2, arg3, arg4);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2, arg3, arg4);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2, arg3, arg4);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, T3, T4, T5, TResult> MemoizeCore<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, T5, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
            {
                string key = CalculateCompositeKey(method, arg1, arg2, arg3, arg4, arg5);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2, arg3, arg4, arg5);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2, arg3, arg4, arg5);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, T3, T4, T5, T6, TResult> MemoizeCore<T1, T2, T3, T4, T5, T6, TResult>(Func<T1, T2, T3, T4, T5, T6, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, T5, T6, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
            {
                string key = CalculateCompositeKey(method, arg1, arg2, arg3, arg4, arg5, arg6);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2, arg3, arg4, arg5, arg6);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, T3, T4, T5, T6, T7, TResult> MemoizeCore<T1, T2, T3, T4, T5, T6, T7, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, T5, T6, T7, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
            {
                string key = CalculateCompositeKey(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> MemoizeCore<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, T5, T6, T7, T8, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
            {
                string key = CalculateCompositeKey(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> MemoizeCore<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
            {
                string key = CalculateCompositeKey(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> MemoizeCore<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> method, DateTime absoluteExpiration, TimeSpan slidingExpiration, Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, IEnumerable<IDependency>> dependencyResolver)
        {
            return delegate (T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
            {
                string key = CalculateCompositeKey(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                if (!TryGetValue(key, MemoizationGroup, out TResult result))
                {
                    var f1 = FuncFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                    var f2 = dependencyResolver == null ? null : FuncFactory.Create(dependencyResolver, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
                    result = (TResult)GetOrAddCore(f1, key, MemoizationGroup, () => absoluteExpiration, () => slidingExpiration, f2).Value;
                }
                return result;
            };
        }

        private static string CalculateCompositeKey(Delegate del, params object[] args)
        {
            int result = del == null || del.GetMethodInfo() == null ? NullHashCode.GetHashCode() : MethodDescriptor.Create(del.GetMethodInfo()).ToString().GetHashCode();
            for (int i = 0; i < args.Length; i++)
            {
                object current = args[i] ?? NullHashCode;
                byte[] bytes = current as byte[];
                result ^= bytes == null ? current.GetHashCode() : Generate.HashCode32(bytes.Cast<IConvertible>());
            }
            return result.ToString(CultureInfo.InvariantCulture);
        }
    }
}