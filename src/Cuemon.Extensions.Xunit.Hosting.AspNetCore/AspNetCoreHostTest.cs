using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Represents a base class from which all implementations of unit testing, that uses Microsoft Dependency Injection and depends on ASP.NET Core, should derive.
    /// </summary>
    /// <typeparam name="T">The type of the object that implements the <see cref="IAspNetCoreHostFixture"/> interface.</typeparam>
    /// <seealso cref="Test" />
    /// <seealso cref="HostTest{T}" />
    public abstract class AspNetCoreHostTest<T> : HostTest<T> where T : class, IAspNetCoreHostFixture
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AspNetCoreHostTest{T}"/> class.
        /// </summary>
        /// <param name="hostFixture">An implementation of the <see cref="IAspNetCoreHostFixture"/> interface.</param>
        /// <param name="output">An implementation of the <see cref="ITestOutputHelper"/> interface.</param>
        protected AspNetCoreHostTest(T hostFixture, ITestOutputHelper output = null) : base(hostFixture, output)
        {
        }

        /// <summary>
        /// Initializes the specified host fixture.
        /// </summary>
        /// <param name="hostFixture">The host fixture to initialize.</param>
        protected override void InitializeHostFixture(T hostFixture)
        {
            if (!hostFixture.HasValidState())
            {
                hostFixture.ConfigureCallback = Configure;
                hostFixture.ConfigureHostCallback = ConfigureHost;
                hostFixture.ConfigureServicesCallback = ConfigureServices;
                hostFixture.ConfigureApplicationCallback = ConfigureApplication;
                hostFixture.ConfigureHost(this);
            }
            Host = hostFixture.Host;
            ServiceProvider = hostFixture.Host.Services;
            Application = hostFixture.Application;
            Configure(hostFixture.Configuration, hostFixture.HostingEnvironment);
        }
        
        /// <summary>
        /// Gets the <see cref="IApplicationBuilder"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IApplicationBuilder"/> initialized by the <see cref="IHost"/>.</value>
        public IApplicationBuilder Application { get; protected set; }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The type that provides the mechanisms to configure the HTTP request pipeline.</param>
        public abstract void ConfigureApplication(IApplicationBuilder app);
    }
}