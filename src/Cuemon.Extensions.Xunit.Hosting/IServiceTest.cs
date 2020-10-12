using System;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core services testing.
    /// </summary>
    public interface IServiceTest
    {
        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IServiceProvider"/> initialized by the <see cref="IHost"/>.</value>
        IServiceProvider ServiceProvider { get; }
    }
}