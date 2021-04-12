using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Provides a set of static methods for ASP.NET Core middleware unit testing.
    /// </summary>
    public static class MiddlewareTestFactory
    {
        /// <summary>
        /// Creates and returns an <see cref="IMiddlewareTest" /> implementation.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IMiddlewareTest" /> implementation.</returns>
        public static IMiddlewareTest CreateMiddlewareTest(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new MiddlewareAspNetCoreHostTest(pipelineSetup, serviceSetup, hostSetup, new AspNetCoreHostFixture());
        }

        /// <summary>
        /// Runs a middleware test.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        public static async Task RunMiddlewareTest(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            using (var middleware = CreateMiddlewareTest(pipelineSetup, serviceSetup, hostSetup))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var pipeline = middleware.Application.Build();
                await pipeline(context).ConfigureAwait(false);
            }
        }
    }
}