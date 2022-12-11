using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Extensions.Xunit
{
    /// <summary>
    /// Provides a default base implementation of the <see cref="ITestStore{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">The type of the object to reference in the memory store.</typeparam>
    /// <seealso cref="ITestStore{T}" />
    public class InMemoryTestStore<T> : ITestStore<T>
    {
        private readonly List<T> _store = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryTestStore{T}"/> class.
        /// </summary>
        public InMemoryTestStore()
        {
        }

        /// <summary>
        /// Gets the reference to the encapsulated store.
        /// </summary>
        /// <value>The reference to the encapsulated store.</value>
        protected IList<T> InnerStore => _store;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="InMemoryTestStore{T}" />.
        /// </summary>
        /// <value>The number of elements contained in the <see cref="InMemoryTestStore{T}" />.</value>
        public int Count => _store.Count;

        /// <summary>
        /// Adds an item to the <see cref="InMemoryTestStore{T}" />.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="InMemoryTestStore{T}" />.</param>
        public void Add(T item)
        {
            _store.Add(item);
        }

        /// <summary>
        /// Filters the elements contained in the <see cref="InMemoryTestStore{T}" /> based on an optional <paramref name="predicate" />.
        /// </summary>
        /// <param name="predicate">The function delegate to test each element for a condition.</param>
        /// <returns>An <see cref="IEnumerable{T}" /> that satisfies the condition.</returns>
        public virtual IEnumerable<T> Query(Func<T, bool> predicate = null)
        {
            return Condition.TernaryIf(predicate == null, () => _store, () => _store.Where(predicate!));
        }

        /// <summary>
        /// Filters the elements contained in the <see cref="InMemoryTestStore{T}" /> based on the type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="TResult">The concrete type that implements <typeparamref name="T" />.</typeparam>
        /// <returns>An <see cref="IEnumerable{T}" /> that satisfies the condition.</returns>
        public IEnumerable<TResult> QueryFor<TResult>() where TResult : T
        {
            return Query(item => item.GetType() == typeof(TResult)).Cast<TResult>();
        }
    }
}
