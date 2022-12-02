using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Provides a set of static methods for <see cref="IHost"/> unit testing.
    /// </summary>
    public static class GenericHostTestFactory
    {
        /// <summary>
        /// Creates and returns an <see cref="IGenericHostTest" /> implementation.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IGenericHostTest" /> implementation.</returns>
        public static IGenericHostTest CreateGenericHostTest(Action<IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new GenericHostTest(serviceSetup, hostSetup, new HostFixture());
        }

        /// <summary>
        /// Creates and returns an <see cref="IGenericHostTest" /> implementation.
        /// </summary>
        /// <param name="serviceSetup">The <see cref="IServiceCollection" /> which may be configured.</param>
        /// <param name="hostSetup">The <see cref="IHostBuilder"/> which may be configured.</param>
        /// <returns>An instance of an <see cref="IGenericHostTest" /> implementation.</returns>
        public static IGenericHostTest CreateGenericHostTest(Action<HostBuilderContext, IServiceCollection> serviceSetup = null, Action<IHostBuilder> hostSetup = null)
        {
            return new GenericHostTest(serviceSetup, hostSetup, new HostFixture());
        }
    }
}
