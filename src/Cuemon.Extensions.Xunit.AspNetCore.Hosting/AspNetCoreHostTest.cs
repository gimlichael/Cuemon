using Cuemon.Extensions.Xunit.Hosting.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
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
        /// <param name="aspNetCoreHostFixture">An implementation of the <see cref="IAspNetCoreHostFixture"/> interface.</param>
        /// <param name="output">An implementation of the <see cref="ITestOutputHelper"/> interface.</param>
        protected AspNetCoreHostTest(T aspNetCoreHostFixture, ITestOutputHelper output = null) : base(aspNetCoreHostFixture, output)
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
                hostFixture.ConfigureServicesCallback = ConfigureServices;
                hostFixture.ConfigureApplicationCallback = ConfigureApplication;
                hostFixture.ConfigureHost(this);
            }
            Host = hostFixture.Host;
            ServiceProvider = hostFixture.ServiceProvider;
            Application = hostFixture.Application;
        }

        /// <summary>
        /// Gets the <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.
        /// </summary>
        /// <value>The <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.</value>
        public new IWebHost Host { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IApplicationBuilder"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IApplicationBuilder"/> initialized by the <see cref="IHost"/>.</value>
        public IApplicationBuilder Application { get; protected set; }

        /// <summary>
        /// Adds services to the container.
        /// </summary>
        /// <param name="services">The collection of service descriptors.</param>
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IHttpContextAccessor, FakeHttpContextAccessor>();
        }

        /// <summary>
        /// Configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The type that provides the mechanisms to configure the HTTP request pipeline.</param>
        public abstract void ConfigureApplication(IApplicationBuilder app);
    }
}