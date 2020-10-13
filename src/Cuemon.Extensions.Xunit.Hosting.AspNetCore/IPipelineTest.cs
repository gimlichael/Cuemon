using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core pipeline testing.
    /// </summary>
    public interface IPipelineTest
    {
        /// <summary>
        /// Gets the <see cref="IApplicationBuilder"/> initialized by the <see cref="IHost"/>.
        /// </summary>
        /// <value>The <see cref="IApplicationBuilder"/> initialized by the <see cref="IHost"/>.</value>
        IApplicationBuilder Application { get; }
    }
}