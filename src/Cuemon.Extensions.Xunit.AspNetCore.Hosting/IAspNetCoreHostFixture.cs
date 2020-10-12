using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Provides a way to use Microsoft Dependency Injection in unit tests tailored for ASP.NET Core.
    /// </summary>
    /// <seealso cref="IHostFixture" />
    public interface IAspNetCoreHostFixture : IHostFixture, IPipelineTest
    {
        /// <summary>
        /// Gets or sets the delegate that configures the HTTP request pipeline.
        /// </summary>
        /// <value>The delegate that configures the HTTP request pipeline.</value>
        Action<IApplicationBuilder> ConfigureApplicationCallback { get; set; }

        /// <summary>
        /// Gets the <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.
        /// </summary>
        /// <value>The <see cref="IHost"/> initialized by the <see cref="IHostFixture"/>.</value>
        public new IWebHost Host { get; }
    }
}