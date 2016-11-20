using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// Extension methods for the <see cref="IApplicationBuilder"/> interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Conditionally creates a branch in the request pipeline that is rejoined to the main pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="condition">if set to <c>true</c>, the <paramref name="configurator"/> is invoked.</param>
        /// <param name="configurator">The delegate to invoke when <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseWhen(this IApplicationBuilder app, bool condition, Action<IApplicationBuilder> configurator)
        {
            Validator.ThrowIfNull(configurator, nameof(configurator));
            return UseWhen(app, context => condition, configurator);
        }

        /// <summary>
        /// Conditionally creates a branch in the request pipeline that is rejoined to the main pipeline.
        /// </summary>
        /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <param name="condition">The condition.</param>
        /// <param name="configurator">The configuration.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance.</returns>
        public static IApplicationBuilder UseWhen(this IApplicationBuilder app, Func<HttpContext, bool> condition, Action<IApplicationBuilder> configurator)
        {
            Validator.ThrowIfNull(app, nameof(app));
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(configurator, nameof(configurator));

            var builder = app.New();
            configurator(builder);

            return app.Use(next =>
            {
                builder.Run(next);
                var branch = builder.Build();
                return context => condition(context) ? branch(context) : next(context);
            });
        }
    }
}