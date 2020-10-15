using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core testing with support for Configuration.
    /// </summary>
    public interface IConfigurationTest
    {
        /// <summary>
        /// Gets the <see cref="IConfiguration"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IConfiguration"/> initialized by the <see cref="IHost"/>.</value>
        IConfiguration Configuration { get; }
    }
}