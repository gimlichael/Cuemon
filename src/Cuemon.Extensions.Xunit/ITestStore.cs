using System;
using System.Collections.Generic;

namespace Cuemon.Extensions.Xunit
{
    /// <summary>
    /// Represents the members needed for adding and querying a store tailored for unit testing.
    /// </summary>
    /// <typeparam name="T">The type of the object to reference in the store.</typeparam>
    public interface ITestStore<T>
    {
        /// <summary>
        /// Gets the number of elements contained in the <see cref="ITestStore{T}"/>.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="ITestStore{T}"/>.</value>
        int Count { get; }

        /// <summary>
        /// Adds an item to the <see cref="ITestStore{T}"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="ITestStore{T}"/>.</param>
        void Add(T item);

        /// <summary>
        /// Filters the elements contained in the <see cref="ITestStore{T}"/> based on an optional <paramref name="predicate"/>.
        /// </summary>
        /// <param name="predicate">The function delegate to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> that satisfies the condition.</returns>
        IEnumerable<T> Query(Func<T, bool> predicate = null);

        /// <summary>
        /// Filters the elements contained in the <see cref="ITestStore{T}"/> based on the type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="TResult">The concrete type that implements <typeparamref name="T"/>.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}"/> that satisfies the condition.</returns>
        IEnumerable<TResult> QueryFor<TResult>() where TResult : T;
    }
}