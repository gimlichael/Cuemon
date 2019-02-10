using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    public class SwaggerMiddleware : Middleware<SwaggerOptions>
    {
        public SwaggerMiddleware(RequestDelegate next, Action<SwaggerOptions> setup) : base(next, setup)
        {
        }

        public override Task Invoke(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This is a factory implementation of the <see cref="SwaggerMiddleware"/> class.
    /// </summary>
    public static class SwaggerBuilderExtension
    {
        /// <summary>
        /// Adds a Swagger 2.0 implementation to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="options">The Swagger middleware options.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, Action<SwaggerOptions> options)
        {
            return builder.UseMiddleware<SwaggerMiddleware>(options);
        }
    }
}