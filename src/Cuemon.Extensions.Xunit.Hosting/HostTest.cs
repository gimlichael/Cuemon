using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{

    /// <summary>
    /// Represents a base class from which all implementations of unit testing, that uses Microsoft Dependency Injection, should derive.
    /// </summary>
    /// <typeparam name="T">The type of the object that implements the <see cref="IHostFixture"/> interface.</typeparam>
    /// <seealso cref="Test" />
    /// <seealso cref="IClassFixture{TFixture}" />
    /// <remarks>The class needed to be designed in this rather complex way, as this is the only way that xUnit supports a shared context. The need for shared context is theoretical at best, but it does opt-in for Scoped instances.</remarks>
    public abstract class HostTest<T> : Test, IClassFixture<T> where T : class, IHostFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HostTest{T}"/> class.
        /// </summary>
        /// <param name="hostFixture">An implementation of the <see cref="IHostFixture"/> interface.</param>
        /// <param name="output">An implementation of the <see cref="ITestOutputHelper"/> interface.</param>
        protected HostTest(IHostFixture hostFixture, ITestOutputHelper output = null) : base(output)
        {
            Validator.ThrowIfNull(hostFixture, nameof(hostFixture));
            if (!hostFixture.HasValidState())
            {
                hostFixture.ConfigureServicesCallback = ConfigureServices;
                hostFixture.ConfigureHost(GetType());
            }

            Host = hostFixture.Host;
            ServiceProvider = hostFixture.ServiceProvider;
            Configuration = hostFixture.Configuration;
            HostingEnvironment = hostFixture.HostingEnvironment;
        }

        /// <summary>
        /// Gets the <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.
        /// </summary>
        /// <value>The <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.</value>
        public IHost Host { get; }

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IServiceProvider"/> initialized by the <see cref="IHost"/>.</value>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the <see cref="IConfiguration"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IConfiguration"/> initialized by the <see cref="IHost"/>.</value>
        public IConfiguration Configuration { get; }

        #if NETSTANDARD
        /// <summary>
        /// Gets the <see cref="IHostingEnvironment"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IHostingEnvironment"/> initialized by the <see cref="IHost"/>.</value>
        public IHostingEnvironment HostingEnvironment { get; }
        #elif NETCOREAPP
        /// <summary>
        /// Gets the <see cref="IHostEnvironment"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IHostEnvironment"/> initialized by the <see cref="IHost"/>.</value>
        public IHostEnvironment HostingEnvironment { get; }
        #endif

        /// <summary>
        /// Adds services to the container.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        public abstract void ConfigureServices(IServiceCollection services);
    }
}