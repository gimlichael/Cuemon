using Microsoft.Extensions.Hosting;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Represents the members needed for bare-bone DI testing with support for <see cref="IHost" />.
    /// </summary>
    /// <seealso cref="IServiceTest" />
    /// <seealso cref="IConfigurationTest" />
    /// <seealso cref="IHostingEnvironmentTest" />
    /// <seealso cref="ITest" />
    public interface IGenericHostTest : IServiceTest, IConfigurationTest, IHostingEnvironmentTest, ITest
    {
    }
}
