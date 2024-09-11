using System.Threading;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit
{
    /// <summary>
    /// Provides a default implementation of the <see cref="ITestOutputHelper"/> interface.
    /// </summary>
    public class TestOutputHelperAccessor : ITestOutputHelperAccessor
    {
        private static readonly AsyncLocal<ITestOutputHelper> Current = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestOutputHelperAccessor"/> class with the specified <see cref="ITestOutputHelper"/> instance.
        /// </summary>
        /// <param name="output">The <see cref="ITestOutputHelper"/> instance to be used for outputting test results.</param>
        public TestOutputHelperAccessor(ITestOutputHelper output = null)
        {
            Current.Value = output;
        }

        /// <summary>
        /// Gets or sets the <see cref="ITestOutputHelper"/> instance used for outputting test results.
        /// </summary>
        /// <value>The <see cref="ITestOutputHelper"/> instance.</value>
        public ITestOutputHelper TestOutput
        {
            get => Current.Value;
            set => Current.Value = value;
        }
    }
}
