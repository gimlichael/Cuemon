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
        /// <remarks>Uses per-test-class fixture data.</remarks>
        public static IMiddlewareTest Create(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return Create(true, pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Creates and returns an <see cref="IMiddlewareTest" /> implementation.
        /// </summary>
        /// <param name="useClassFixture">Specify <c>true</c> to indicate a test which has per-test-class fixture data; otherwise, <c>false</c>.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IMiddlewareTest" /> implementation.</returns>
        public static IMiddlewareTest Create(bool useClassFixture, Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            var host = new MiddlewareTest(pipelineSetup, serviceSetup, hostSetup, new AspNetCoreHostFixture());
            return useClassFixture ? new MiddlewareTestDecorator(host) : host;
        }

        /// <summary>
        /// Creates and returns an <see cref="IMiddlewareTest" /> implementation.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IMiddlewareTest" /> implementation.</returns>
        /// <remarks>Uses per-test-class fixture data.</remarks>
        public static IMiddlewareTest CreateWithHostBuilderContext(Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return CreateWithHostBuilderContext(true, pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Creates and returns an <see cref="IMiddlewareTest" /> implementation.
        /// </summary>
        /// <param name="useClassFixture">Specify <c>true</c> to indicate a test which has per-test-class fixture data; otherwise, <c>false</c>.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IMiddlewareTest" /> implementation.</returns>
        public static IMiddlewareTest CreateWithHostBuilderContext(bool useClassFixture, Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            var host = new MiddlewareTest(pipelineSetup, serviceSetup, hostSetup, new AspNetCoreHostFixture());
            return useClassFixture ? new MiddlewareTestDecorator(host) : host;
        }
        
        /// <summary>
        /// Runs a middleware test.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        /// <remarks>Uses per-test-class fixture data.</remarks>
        public static Task Run(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return Run(true, pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Runs a middleware test.
        /// </summary>
        /// <param name="useClassFixture">Specify <c>true</c> to indicate a test which has per-test-class fixture data; otherwise, <c>false</c>.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        public static async Task Run(bool useClassFixture, Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            using (var middleware = Create(useClassFixture, pipelineSetup, serviceSetup, hostSetup))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var pipeline = middleware.Application.Build();
                await pipeline(context!).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Runs a middleware test.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        /// <remarks>Uses per-test-class fixture data.</remarks>
        public static Task RunWithHostBuilderContext(Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return RunWithHostBuilderContext(true, pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Runs a middleware test.
        /// </summary>
        /// <param name="useClassFixture">Specify <c>true</c> to indicate a test which has per-test-class fixture data; otherwise, <c>false</c>.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        public static async Task RunWithHostBuilderContext(bool useClassFixture, Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            using (var middleware = CreateWithHostBuilderContext(useClassFixture, pipelineSetup, serviceSetup, hostSetup))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var pipeline = middleware.Application.Build();
                await pipeline(context!).ConfigureAwait(false);
            }
        }
    }
}
