using System;
using System.Threading;
using System.Threading.Tasks;

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
            var f1 = FuncFactory.Create(tester, default);
            var f2 = ActionFactory.Create(catcher, default);
            return SafeInvokeCore(f1, initializer, f2);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="catcher">The delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T, TResult>(Func<TResult> initializer, Func<TResult, T, TResult> tester, T arg, Action<Exception, T> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = FuncFactory.Create(tester, default, arg);
            var f2 = ActionFactory.Create(catcher, default, arg);
            return SafeInvokeCore(f1, initializer, f2);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="catcher">The delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T1, T2, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, TResult> tester, T1 arg1, T2 arg2, Action<Exception, T1, T2> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = FuncFactory.Create(tester, default, arg1, arg2);
            var f2 = ActionFactory.Create(catcher, default, arg1, arg2);
            return SafeInvokeCore(f1, initializer, f2);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="catcher">The delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T1, T2, T3, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, TResult> tester, T1 arg1, T2 arg2, T3 arg3, Action<Exception, T1, T2, T3> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = FuncFactory.Create(tester, default, arg1, arg2, arg3);
            var f2 = ActionFactory.Create(catcher, default, arg1, arg2, arg3);
            return SafeInvokeCore(f1, initializer, f2);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="catcher">The delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T1, T2, T3, T4, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, T4, TResult> tester, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<Exception, T1, T2, T3, T4> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = FuncFactory.Create(tester, default, arg1, arg2, arg3, arg4);
            var f2 = ActionFactory.Create(catcher, default, arg1, arg2, arg3, arg4);
            return SafeInvokeCore(f1, initializer, f2);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="catcher">The delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static TResult SafeInvoke<T1, T2, T3, T4, T5, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, T4, T5, TResult> tester, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<Exception, T1, T2, T3, T4, T5> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = FuncFactory.Create(tester, default, arg1, arg2, arg3, arg4, arg5);
            var f2 = ActionFactory.Create(catcher, default, arg1, arg2, arg3, arg4, arg5);
            return SafeInvokeCore(f1, initializer, f2);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <param name="catcher">The function delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static Task<TResult> SafeInvokeAsync<TResult>(Func<TResult> initializer, Func<TResult, CancellationToken, Task<TResult>> tester, CancellationToken ct = default, Func<Exception, CancellationToken, Task> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = TaskFuncFactory.Create(tester, default);
            var f2 = TaskActionFactory.Create(catcher, default);
            return SafeInvokeAsyncCore(f1, initializer, f2, ct);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <param name="catcher">The function delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static Task<TResult> SafeInvokeAsync<T, TResult>(Func<TResult> initializer, Func<TResult, T, CancellationToken, Task<TResult>> tester, T arg, CancellationToken ct = default, Func<Exception, T, CancellationToken, Task> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = TaskFuncFactory.Create(tester, default, arg);
            var f2 = TaskActionFactory.Create(catcher, default, arg);
            return SafeInvokeAsyncCore(f1, initializer, f2, ct);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <param name="catcher">The function delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static Task<TResult> SafeInvokeAsync<T1, T2, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, CancellationToken, Task<TResult>> tester, T1 arg1, T2 arg2, CancellationToken ct = default, Func<Exception, T1, T2, CancellationToken, Task> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = TaskFuncFactory.Create(tester, default, arg1, arg2);
            var f2 = TaskActionFactory.Create(catcher, default, arg1, arg2);
            return SafeInvokeAsyncCore(f1, initializer, f2, ct);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <param name="catcher">The function delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static Task<TResult> SafeInvokeAsync<T1, T2, T3, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, CancellationToken, Task<TResult>> tester, T1 arg1, T2 arg2, T3 arg3, CancellationToken ct = default, Func<Exception, T1, T2, T3, CancellationToken, Task> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = TaskFuncFactory.Create(tester, default, arg1, arg2, arg3);
            var f2 = TaskActionFactory.Create(catcher, default, arg1, arg2, arg3);
            return SafeInvokeAsyncCore(f1, initializer, f2, ct);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <param name="catcher">The function delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static Task<TResult> SafeInvokeAsync<T1, T2, T3, T4, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, T4, CancellationToken, Task<TResult>> tester, T1 arg1, T2 arg2, T3 arg3, T4 arg4, CancellationToken ct = default, Func<Exception, T1, T2, T3, T4, CancellationToken, Task> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = TaskFuncFactory.Create(tester, default, arg1, arg2, arg3, arg4);
            var f2 = TaskActionFactory.Create(catcher, default, arg1, arg2, arg3, arg4);
            return SafeInvokeAsyncCore(f1, initializer, f2, ct);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate that initializes an object implementing the <see cref="IDisposable"/> interface.</param>
        /// <param name="tester">The function delegate that is used to ensure that operations performed on <typeparamref name="TResult"/> abides CA2000.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="tester"/> and delegate <paramref name="catcher"/>.</param>
        /// <param name="ct">The token to monitor for cancellation requests. The default value is <see cref="CancellationToken.None"/>.</param>
        /// <param name="catcher">The function delegate that will handle any exceptions might thrown by <paramref name="tester"/>.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the return value of the function delegate <paramref name="initializer"/> if the operations succeeded; otherwise null if the operation failed.</returns>
        public static Task<TResult> SafeInvokeAsync<T1, T2, T3, T4, T5, TResult>(Func<TResult> initializer, Func<TResult, T1, T2, T3, T4, T5, CancellationToken, Task<TResult>> tester, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, CancellationToken ct = default, Func<Exception, T1, T2, T3, T4, T5, CancellationToken, Task> catcher = null) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            Validator.ThrowIfNull(tester, nameof(tester));
            var f1 = TaskFuncFactory.Create(tester, default, arg1, arg2, arg3, arg4, arg5);
            var f2 = TaskActionFactory.Create(catcher, default, arg1, arg2, arg3, arg4, arg5);
            return SafeInvokeAsyncCore(f1, initializer, f2, ct);
        }

        private static TResult SafeInvokeCore<TTester, TResult, TCatcher>(FuncFactory<TTester, TResult> testerFactory, Func<TResult> initializer, ActionFactory<TCatcher> catcherFactory)
            where TResult : class, IDisposable
            where TTester : Template<TResult>
            where TCatcher : Template<Exception>
        {
            TResult result = null;
            try
            {
                testerFactory.GenericArguments.Arg1 = initializer();
                testerFactory.GenericArguments.Arg1 = testerFactory.ExecuteMethod();
                result = testerFactory.GenericArguments.Arg1;
                testerFactory.GenericArguments.Arg1 = null;
            }
            catch (Exception e)
            {
                if (!catcherFactory.HasDelegate)
                {
                    throw;
                }
                else
                {
                    catcherFactory.GenericArguments.Arg1 = e;
                    catcherFactory.ExecuteMethod();
                }
            }
            finally
            {
                testerFactory.GenericArguments.Arg1?.Dispose();
            }
            return result;
        }

        private static async Task<TResult> SafeInvokeAsyncCore<TTester, TResult, TCatcher>(TaskFuncFactory<TTester, TResult> testerFactory, Func<TResult> initializer, TaskActionFactory<TCatcher> catcherFactory, CancellationToken ct)
            where TResult : class, IDisposable
            where TTester : Template<TResult>
            where TCatcher : Template<Exception>
        {
            TResult result = null;
            try
            {
                testerFactory.GenericArguments.Arg1 = initializer();
                testerFactory.GenericArguments.Arg1 = await testerFactory.ExecuteMethodAsync(ct).ConfigureAwait(false);
                result = testerFactory.GenericArguments.Arg1;
                testerFactory.GenericArguments.Arg1 = null;
            }
            catch (Exception e)
            {
                if (!catcherFactory.HasDelegate)
                {
                    throw;
                }
                else
                {
                    catcherFactory.GenericArguments.Arg1 = e;
                    await catcherFactory.ExecuteMethodAsync(ct).ConfigureAwait(false);
                }
            }
            finally
            {
                testerFactory.GenericArguments.Arg1?.Dispose();
            }
            return result;
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