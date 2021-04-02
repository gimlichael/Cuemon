using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    /// <summary>
    /// Provides a set of static methods for ASP.NET Core MVC filter unit testing.
    /// </summary>
    public static class MvcFilterTestFactory
    {
        /// <summary>
        /// Creates and returns an <see cref="IMvcFilterTest"/> implementation.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IMvcFilterTest"/> implementation.</returns>
        public static IMvcFilterTest CreateMvcFilterTest(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new MvcFilterAspNetCoreHostTest(pipelineSetup, serviceSetup, hostSetup);
        }

        /// <summary>
        /// Runs a filter/middleware test.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder" /> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder" /> which may be configured.</param>
        /// <returns>A task that represents the execution of the middleware.</returns>
        public static async Task RunMvcFilterTest(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            using (var middleware = CreateMvcFilterTest(pipelineSetup, serviceSetup, hostSetup))
            {
                var context = middleware.ServiceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
                var pipeline = middleware.Application.Build();
                await pipeline(context).ConfigureAwait(false);
            }
        }
    }
}