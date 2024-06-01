using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Provides a set of static methods for ASP.NET Core (including, but not limited to MVC, Razor and related) unit testing.
    /// </summary>
    /// <seealso cref="IHostTest"/>.
    public static class WebHostTestFactory
    {
        /// <summary>
        /// Creates and returns an <see cref="IWebHostTest"/> implementation.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IWebHostTest"/> implementation.</returns>
        public static IWebHostTest Create(Action<IServiceCollection> serviceSetup = null, Action<IApplicationBuilder> pipelineSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new WebHostTest(serviceSetup, pipelineSetup, hostSetup, new AspNetCoreHostFixture());
        }

        /// <summary>
        /// Creates and returns an <see cref="IWebHostTest"/> implementation.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IWebHostTest"/> implementation.</returns>
        public static IWebHostTest CreateWithHostBuilderContext(Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new WebHostTest(serviceSetup, pipelineSetup, hostSetup, new AspNetCoreHostFixture());
        }

        /// <summary>
        /// Runs a filter/middleware test.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="serviceSetup"/> did not have a registered service of <see cref="IHttpContextAccessor"/>.
        /// </exception>
        public static async Task Run(Action<IServiceCollection> serviceSetup = null, Action<IApplicationBuilder> pipelineSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            using (var middleware = Create(serviceSetup, pipelineSetup, hostSetup))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var pipeline = middleware.Application.Build();
                await pipeline(context!).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs a filter/middleware test.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="serviceSetup"/> did not have a registered service of <see cref="IHttpContextAccessor"/>.
        /// </exception>
        public static async Task RunWithHostBuilderContext(Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            using (var middleware = CreateWithHostBuilderContext(serviceSetup, pipelineSetup, hostSetup))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var pipeline = middleware.Application.Build();
                await pipeline(context!).ConfigureAwait(false);
            }
        }
    }
}
