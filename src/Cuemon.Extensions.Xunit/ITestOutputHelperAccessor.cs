using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit
{
    /// <summary>
    /// Provides an interface for accessing the <see cref="ITestOutputHelper"/> instance.
    /// </summary>
    public interface ITestOutputHelperAccessor
    {
        /// <summary>
        /// Gets or sets the <see cref="ITestOutputHelper"/> instance used for outputting test results.
        /// </summary>
        /// <value>The <see cref="ITestOutputHelper"/> instance.</value>
        ITestOutputHelper TestOutput { get; set; }
    }
}
