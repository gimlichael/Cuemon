using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a mechanism for releasing both managed and unmanaged resources with focus on the former.
    /// Implements the <see cref="IDisposable" />
    /// </summary>
    /// <seealso cref="IDisposable" />
    public abstract class Disposable : IDisposable
    {
        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="catcher">The delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<TResult>(Func<TResult> initializer, Func<TResult, TResult> tester, Action<Exception> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            TResult result = null;
            TResult temp = null;
            try
            {
                temp = initializer();
                temp = tester(temp);
                result = temp;
                temp = null;
            }
            catch (Exception e)
            {
                if (catcher == null)
                {
                    throw;
                }
                else
                {
                    catcher(e);
                }
            }
            finally
            {
                temp?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Disposable"/> class.
        /// </summary>
        protected Disposable()
        {
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Disposable"/> object is disposed.
        /// </summary>
        /// <value><c>true</c> if this <see cref="Disposable"/> object is disposed; otherwise, <c>false</c>.</value>
        public bool Disposed { get; private set; }

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Dispose()"/> or <see cref="Dispose(bool)"/> having <c>disposing</c> set to <c>true</c> and <see cref="Disposed"/> is <c>false</c>.
        /// </summary>
        protected abstract void OnDisposeManagedResources();

        /// <summary>
        /// Called when this object is being disposed by either <see cref="Dispose()"/> or <see cref="Dispose(bool)"/> and <see cref="Disposed"/> is <c>false</c>.
        /// </summary>
        protected virtual void OnDisposeUnmanagedResources()
        {
        }

        /// <summary>
        /// Releases all resources used by the <see cref="Disposable"/> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="Disposable"/> object and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (Disposed) { return; }
            if (disposing)
            {
                OnDisposeManagedResources();
            }
            OnDisposeUnmanagedResources();
            Disposed = true;
        }
    }
}