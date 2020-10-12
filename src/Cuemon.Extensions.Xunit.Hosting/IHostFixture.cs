using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Provides a way to use Microsoft Dependency Injection in unit tests.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IHostFixture : IServiceTest, IDisposable
    {
        /// <summary>
        /// Gets or sets the delegate that adds services to the container.
        /// </summary>
        /// <value>The delegate that adds services to the container.</value>
        Action<IServiceCollection> ConfigureServicesCallback { get; set; }
        
        /// <summary>
        /// Gets the <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.
        /// </summary>
        /// <value>The <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.</value>
        IHost Host { get; }

        /// <summary>
        /// Gets the <see cref="IConfiguration"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IConfiguration"/> initialized by the <see cref="IHost"/>.</value>
        IConfiguration Configuration { get; }

        #if NETSTANDARD
        /// <summary>
        /// Gets the <see cref="IHostingEnvironment"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IHostingEnvironment"/> initialized by the <see cref="IHost"/>.</value>
        IHostingEnvironment HostingEnvironment { get; }
        #elif NETCOREAPP
        /// <summary>
        /// Gets the <see cref="IHostEnvironment"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IHostEnvironment"/> initialized by the <see cref="IHost"/>.</value>
        IHostEnvironment HostingEnvironment { get; }
        #endif

        /// <summary>
        /// Creates and configures the <see cref="IHost"/> of this <see cref="IHostFixture"/>.
        /// </summary>
        /// <param name="hostTest">The object that inherits from <see cref="HostTest{T}"/>.</param>
        /// <remarks><paramref name="hostTest"/> was added to support those cases where the caller is required in the host configuration.</remarks>
        void ConfigureHost(Test hostTest);
    }
}