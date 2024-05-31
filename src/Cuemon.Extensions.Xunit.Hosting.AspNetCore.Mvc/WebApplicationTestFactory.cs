using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    /// <summary>
    /// Provides a set of static methods for ASP.NET Core, ASP.NET Core MVC, Razor and related unit testing.
    /// </summary>
    /// <seealso cref="IHostTest"/>.
    [Obsolete("This class is obsolete and will be removed in a future version. Please use WebHostTestFactory instead (located in Cuemon.Extensions.Xunit.Hosting.AspNetCore).")]
    public static class WebApplicationTestFactory
    {
        /// <summary>
        /// Creates and returns an <see cref="IWebApplicationTest"/> implementation.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IWebApplicationTest"/> implementation.</returns>
        [Obsolete("This method is obsolete and will be removed in a future version. Please use WebHostTestFactory equivalent instead (located in Cuemon.Extensions.Xunit.Hosting.AspNetCore).")]
        public static IWebApplicationTest Create(Action<IServiceCollection> serviceSetup = null, Action<IApplicationBuilder> pipelineSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new WebApplicationTest(serviceSetup, pipelineSetup, hostSetup);
        }

        /// <summary>
        /// Creates and returns an <see cref="IWebApplicationTest"/> implementation.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IWebApplicationTest"/> implementation.</returns>
        [Obsolete("This method is obsolete and will be removed in a future version. Please use WebHostTestFactory equivalent instead (located in Cuemon.Extensions.Xunit.Hosting.AspNetCore).")]
        public static IWebApplicationTest CreateWithHostBuilderContext(Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<HostBuilderContext, IApplicationBuilder> pipelineSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new WebApplicationTest(serviceSetup, pipelineSetup, hostSetup);
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
        [Obsolete("This method is obsolete and will be removed in a future version. Please use WebHostTestFactory equivalent instead (located in Cuemon.Extensions.Xunit.Hosting.AspNetCore).")]
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
        [Obsolete("This method is obsolete and will be removed in a future version. Please use WebHostTestFactory equivalent instead (located in Cuemon.Extensions.Xunit.Hosting.AspNetCore).")]
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
