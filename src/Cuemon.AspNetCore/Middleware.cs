using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore
{
    /// <summary>
    /// Provides a base-class for middleware implementation in ASP.NET Core.
    /// </summary>
    public abstract class Middleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Middleware"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        protected Middleware(RequestDelegate next)
        {
            Validator.ThrowIfNull(next, nameof(next));
            Next = next;
        }

        /// <summary>
        /// Gets the delegate of the request pipeline to invoke.
        /// </summary>
        /// <value>The delegate of the request pipeline to invoke.</value>
        protected RequestDelegate Next { get; private set; }

        /// <summary>
        /// Executes the <see cref="Middleware"/>.
        /// </summary>
        /// <param name="context">The context of the current request.</param>
        /// <returns>A task that represents the execution of this middleware.</returns>
        public abstract Task Invoke(HttpContext context);
    }

    /// <summary>
    /// Provides a base-class for middleware implementation in ASP.NET Core with support of the options pattern.
    /// </summary>
    /// <typeparam name="TOptions">The type of the options to setup.</typeparam>
    /// <seealso cref="Middleware" />
    public abstract class Middleware<TOptions> : Middleware
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Middleware{TOptions}"/> class.
        /// </summary>
        /// <param name="next">The delegate of the request pipeline to invoke.</param>
        /// <param name="setup">The <see cref="Action{T}"/> which need to be configured.</param>
        protected Middleware(RequestDelegate next, Action<TOptions> setup)
            : base(next)
        {
            Options = setup.ConfigureOptions();
        }

        /// <summary>
        /// Gets the configured options of this <see cref="Middleware"/>.
        /// </summary>
        /// <value>The configured options of this <see cref="Middleware"/>.</value>
        public TOptions Options { get; }
    }
}