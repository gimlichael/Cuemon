using System;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

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
        /// <returns>An instance of an <see cref="IMvcFilterTest"/> implementation.</returns>
        public static IMvcFilterTest CreateMvcFilterTest(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null)
        {
            serviceSetup ??= services => services.AddScoped<IHttpContextAccessor, FakeHttpContextAccessor>();
            return new MvcFilterAspNetCoreHostTest(pipelineSetup, serviceSetup);
        }
    }
}