using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    /// <summary>
    /// Provides a set of static methods for ASP.NET Core MVC, Razor and related unit testing.
    /// </summary>
    /// <seealso cref="IHostTest"/>.
    public static class WebApplicationTestFactory
    {
        /// <summary>
        /// Creates and returns an <see cref="IWebApplicationTest"/> implementation.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IWebApplicationTest"/> implementation.</returns>
        /// <remarks>Uses per-test-class fixture data.</remarks>
        public static IWebApplicationTest Create(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return Create(true, pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Creates and returns an <see cref="IWebApplicationTest" /> implementation.
        /// </summary>
        /// <param name="useClassFixture">Specify <c>true</c> to indicate a test which has per-test-class fixture data; otherwise, <c>false</c>.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>An instance of an <see cref="IWebApplicationTest" /> implementation.</returns>
        public static IWebApplicationTest Create(bool useClassFixture, Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new WebApplicationTest(useClassFixture, pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Creates and returns an <see cref="IWebApplicationTest"/> implementation.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IWebApplicationTest"/> implementation.</returns>
        /// <remarks>Uses per-test-class fixture data.</remarks>
        public static IWebApplicationTest CreateWithHostBuilderContext(Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return CreateWithHostBuilderContext(true, pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Creates and returns an <see cref="IWebApplicationTest"/> implementation.
        /// </summary>
        /// <param name="useClassFixture">Specify <c>true</c> to indicate a test which has per-test-class fixture data; otherwise, <c>false</c>.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IWebApplicationTest"/> implementation.</returns>
        public static IWebApplicationTest CreateWithHostBuilderContext(bool useClassFixture, Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new WebApplicationTest(useClassFixture, pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Runs a filter/middleware test.
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
        /// Runs a filter/middleware test.
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
        /// Runs a filter/middleware test.
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
        /// Runs a filter/middleware test.
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
