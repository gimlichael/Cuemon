using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cuemon.Extensions.Asp.Versioning
{
    /// <summary>
    /// Extension methods for the <see cref="IApplicationBuilder"/> interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a middleware to the pipeline that will intercept status codes written directly to the response and throw an appropriate <see cref="HttpStatusCodeException"/> that can then be translated and re-written in a consistent way.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="statusCodeExceptionFactory">The function delegate that will resolve and throw a proper <see cref="HttpStatusCodeException"/> from status codes written directly to the response.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="builder"/> cannot be null.
        /// </exception>
        /// <remarks>This method was introduced because of the design decisions made of the author of Asp.Versioning; for more information have a read at https://github.com/dotnet/aspnet-api-versioning/issues/886</remarks>
        public static IApplicationBuilder UseRestfulApiVersioning(this IApplicationBuilder builder, Func<HttpContext, HttpStatusCodeException> statusCodeExceptionFactory = null)
        {
            Validator.ThrowIfNull(builder);
            statusCodeExceptionFactory ??= context =>
            {
                if (HttpStatusCodeException.TryParse(context.Response.StatusCode, out var statusCodeEquivalentException))
                {
                    return statusCodeEquivalentException;
                }
                return new InternalServerErrorException();
            };
            return builder.UseStatusCodePages(app =>
            {
                app.Use((HttpContext context, Func<Task> _) => throw statusCodeExceptionFactory(context));
            });
        }
    }
}
