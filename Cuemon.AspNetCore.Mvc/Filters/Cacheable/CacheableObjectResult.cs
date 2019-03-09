using System;
using Cuemon.Configuration;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// Provides a base class for <see cref="ICacheableObjectResult"/> related operations.
    /// </summary>
    /// <typeparam name="T">The type of the object to make cacheable.</typeparam>
    /// <typeparam name="TOptions">The type of the options to the cacheable object.</typeparam>
    /// <seealso cref="Configurable{TOptions}" />
    /// <seealso cref="ICacheableObjectResult" />
    public abstract class CacheableObjectResult<T, TOptions> : Configurable<TOptions>, ICacheableObjectResult where TOptions : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheableObjectResult{T, TOptions}"/> class.
        /// </summary>
        /// <param name="instance">The object to make cacheable.</param>
        /// <param name="setup">The <see cref="Action{TOptions}"/> which need to be configured.</param>
        protected CacheableObjectResult(T instance, Action<TOptions> setup) : base(setup.Configure())
        {
            Value = instance;
        }

        /// <summary>
        /// Gets or sets the value of the cacheable object.
        /// </summary>
        /// <value>The value of the cacheable object.</value>
        public object Value { get; set; }
    }
}