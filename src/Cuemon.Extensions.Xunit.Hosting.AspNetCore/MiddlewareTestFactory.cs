using System;
using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Provides a set of static methods for ASP.NET Core middleware unit testing.
    /// </summary>
    public static class MiddlewareTestFactory
    {
        /// <summary>
        /// Creates and returns an <see cref="IMiddlewareTest"/> implementation.
        /// </summary>
        /// <param name="pipelineSetup">The <see cref="IApplicationBuilder"/> which may be configured.</param>
        /// <param name="serviceSetup">The <see cref="IServiceCollection"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IMiddlewareTest"/> implementation.</returns>
        public static IMiddlewareTest CreateMiddlewareTest(Action<IApplicationBuilder> pipelineSetup = null, Action<IServiceCollection> serviceSetup = null)
        {
            pipelineSetup ??= app => app.UseFakeHttpResponseTrigger();
            serviceSetup ??= services => services.AddScoped<IHttpContextAccessor, FakeHttpContextAccessor>();
            return new MiddlewareAspNetCoreHostTest(pipelineSetup, serviceSetup, new AspNetCoreHostFixture());
        }
    }
}