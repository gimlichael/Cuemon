using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit
{
    /// <summary>
    /// Represents the base class from which all implementations of unit testing should derive.
    /// </summary>
    /// <seealso cref="Disposable"/>
    /// <seealso cref="ITestOutputHelper"/>
    public abstract class Test : Disposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class.
        /// </summary>
        /// <param name="output">An implementation of the <see cref="ITestOutputHelper"/> interface.</param>
        /// <remarks><paramref name="output"/> is initialized automatically in an xUnit project.</remarks>
        protected Test(ITestOutputHelper output = null)
        {
            TestOutput = output;
        }


        /// <summary>
        /// Gets the console substitute to write out unit test information.
        /// </summary>
        /// <value>The console substitute to write out unit test information.</value>
        protected ITestOutputHelper TestOutput { get; }

        /// <summary>
        /// Gets a value indicating whether <see cref="TestOutput"/> has a reference to an implementation of <see cref="ITestOutputHelper"/>.
        /// </summary>
        /// <value><c>true</c> if this instance has has a reference to an implementation of <see cref="ITestOutputHelper"/>; otherwise, <c>false</c>.</value>
        protected bool HasTestOutput => TestOutput != null;

        /// <summary>
        /// Called when this object is being disposed by either <see cref="M:Cuemon.Disposable.Dispose" /> or <see cref="M:Cuemon.Disposable.Dispose(System.Boolean)" /> having <c>disposing</c> set to <c>true</c> and <see cref="P:Cuemon.Disposable.Disposed" /> is <c>false</c>.
        /// </summary>
        protected override void OnDisposeManagedResources()
        {
        }
    }
}