using System.Threading;

namespace Cuemon.Threading
{
    /// <summary>
    /// Specifies options that is related to asynchronous operations.
    /// </summary>
    public class AsyncOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncOptions"/> class.
        /// </summary>
        public AsyncOptions()
        {
            CancellationToken = default;
        }

        /// <summary>
        /// Gets or sets the cancellation token of an asynchronous operations.
        /// </summary>
        /// <value>The cancellation token of an asynchronous operations.</value>
        public CancellationToken CancellationToken { get; set; }
    }
}