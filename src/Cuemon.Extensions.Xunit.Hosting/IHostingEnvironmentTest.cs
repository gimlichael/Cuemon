using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Represents the members needed for DI testing with support for HostingEnvironment.
    /// </summary>
    public interface IHostingEnvironmentTest
    {
        #if NETSTANDARD
        /// <summary>
        /// Gets the <see cref="IHostingEnvironment"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IHostingEnvironment"/> initialized by the <see cref="IHost"/>.</value>
        IHostingEnvironment HostingEnvironment { get; }
        #else
        /// <summary>
        /// Gets the <see cref="IHostEnvironment"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IHostEnvironment"/> initialized by the <see cref="IHost"/>.</value>
        IHostEnvironment HostingEnvironment { get; }
        #endif       
    }
}