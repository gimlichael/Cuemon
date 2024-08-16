using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
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
        /// Runs a middleware and returns an <see cref="HttpClient"/> for making HTTP requests to the test server.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <param name="responseFactory">The function delegate that creates a <see cref="HttpResponseMessage"/> from the <see cref="HttpClient"/>. Default is a GET request to the root URL ("/").</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the <see cref="HttpResponseMessage"/> for the test server.</returns>
        public static async Task<HttpResponseMessage> RunAsync(Action<IServiceCollection> serviceSetup = null, Action<IApplicationBuilder> pipelineSetup = null, Action<IHostBuilder> hostSetup = null, Func<HttpClient, Task<HttpResponseMessage>> responseFactory = null)
        {
            using var client = Create(serviceSetup, pipelineSetup, hostSetup).Host.GetTestClient();
            return await client.ToHttpResponseMessageAsync(responseFactory).ConfigureAwait(false);
        }

        /// <summary>
        /// Runs a filter/middleware test.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <param name="responseFactory">The function delegate that creates a <see cref="HttpResponseMessage"/> from the <see cref="HttpClient"/>. Default is a GET request to the root URL ("/").</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation. The task result contains the <see cref="HttpResponseMessage"/> for the test server.</returns>
        public static async Task<HttpResponseMessage> RunWithHostBuilderContextAsync(Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<IHostBuilder> hostSetup = null, Func<HttpClient, Task<HttpResponseMessage>> responseFactory = null)
        {
            using var client = CreateWithHostBuilderContext(serviceSetup, pipelineSetup, hostSetup).Host.GetTestClient();
            return await client.ToHttpResponseMessageAsync().ConfigureAwait(false);
        }
    }
}
