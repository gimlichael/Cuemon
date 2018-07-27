using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Cuemon.Threading.Tasks
{
    /// <summary>
    /// Extension methods for the <see cref="Task"/>.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Configures an awaiter to marshal the continuation back to the captured synchronization context.
        /// </summary>
        /// <param name="t">The <see cref="Task"/> to extend.</param>
        /// <returns>An object used to await this task.</returns>
        public static ConfiguredTaskAwaitable ContinueWithCapturedContext(this Task t)
        {
            return t.ConfigureAwait(true);
        }

        /// <summary>
        /// Configures an awaiter to marshal the continuation back to the captured synchronization context.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by this <see cref="Task{TResult}"/>.</typeparam>
        /// <param name="t">The <see cref="Task"/> to extend.</param>
        /// <returns>An object used to await this task.</returns>
        public static ConfiguredTaskAwaitable<TResult> ContinueWithCapturedContext<TResult>(this Task<TResult> t)
        {
            return t.ConfigureAwait(true);
        }

        /// <summary>
        /// Configures an awaiter to suppress capturing a synchronization context back to the continuation.
        /// </summary>
        /// <param name="t">The <see cref="Task"/> to extend.</param>
        /// <returns>An object used to await this task.</returns>
        public static ConfiguredTaskAwaitable ContinueWithSuppressedContext(this Task t)
        {
            return t.ConfigureAwait(false);
        }

        /// <summary>
        /// Configures an awaiter to suppress capturing a synchronization context back to the continuation.
        /// </summary>
        /// <typeparam name="TResult">The type of the result produced by this <see cref="Task{TResult}"/>.</typeparam>
        /// <param name="t">The <see cref="Task"/> to extend.</param>
        /// <returns>An object used to await this task.</returns>
        public static ConfiguredTaskAwaitable<TResult> ContinueWithSuppressedContext<TResult>(this Task<TResult> t)
        {
            return t.ConfigureAwait(false);
        }
    }
}