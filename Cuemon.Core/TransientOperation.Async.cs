using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Cuemon.Reflection;

namespace Cuemon
{
    public static partial class TransientOperation
    {
        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static Task<TResult> WithFuncAsync<TResult>(Func<Task<TResult>> faultSensitiveMethod, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskFuncFactory.Create(faultSensitiveMethod);
            return WithFuncAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static Task<TResult> WithFuncAsync<T, TResult>(Func<T, Task<TResult>> faultSensitiveMethod, T arg, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskFuncFactory.Create(faultSensitiveMethod, arg);
            return WithFuncAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static Task<TResult> WithFuncAsync<T1, T2, TResult>(Func<T1, T2, Task<TResult>> faultSensitiveMethod, T1 arg1, T2 arg2, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskFuncFactory.Create(faultSensitiveMethod, arg1, arg2);
            return WithFuncAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static Task<TResult> WithFuncAsync<T1, T2, T3, TResult>(Func<T1, T2, T3, Task<TResult>> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            return WithFuncAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static Task<TResult> WithFuncAsync<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, Task<TResult>> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            return WithFuncAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static Task<TResult> WithFuncAsync<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, Task<TResult>> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            return WithFuncAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        public static Task WithActionAsync(Func<Task> faultSensitiveMethod, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskActionFactory.Create(faultSensitiveMethod);
            return WithActionAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        public static Task WithActionAsync<T>(Func<T, Task> faultSensitiveMethod, T arg, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskActionFactory.Create(faultSensitiveMethod, arg);
            return WithActionAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        public static Task WithActionAsync<T1, T2>(Func<T1, T2, Task> faultSensitiveMethod, T1 arg1, T2 arg2, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskActionFactory.Create(faultSensitiveMethod, arg1, arg2);
            return WithActionAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        public static Task WithActionAsync<T1, T2, T3>(Func<T1, T2, T3, Task> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            return WithActionAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        public static Task WithActionAsync<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Task> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            return WithActionAsyncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive <see cref="Task"/> based function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which need to be configured.</param>
        public static Task WithActionAsync<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Task> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TransientOperationOptions> setup = null)
        {
            var factory = TaskActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            return WithActionAsyncCore(factory, setup);
        }

        private static async Task WithActionAsyncCore<TTuple>(TaskActionFactory<TTuple> factory, Action<TransientOperationOptions> setup = null) where TTuple : Template
        {
            var options = Patterns.Configure(setup);
            if (!options.EnableRecovery)
            {
                await factory.ExecuteMethodAsync().ConfigureAwait(false);
                return;
            }
            DateTime timestamp = DateTime.UtcNow;
            TimeSpan latency = TimeSpan.Zero;
            TimeSpan totalWaitTime = TimeSpan.Zero;
            TimeSpan lastWaitTime = TimeSpan.Zero;
            bool isTransientFault = false;
            bool throwExceptions;
            List<Exception> aggregatedExceptions = new List<Exception>();
            for (int attempts = 0; ;)
            {
                TimeSpan waitTime = options.RetryStrategy(attempts);
                try
                {
                    if (latency > options.MaximumAllowedLatency) { throw new LatencyException(string.Format(CultureInfo.InvariantCulture, "The latency of the operation exceeded the allowed maximum value of {0} seconds. Actual latency was: {1} seconds.", options.MaximumAllowedLatency.TotalSeconds, latency.TotalSeconds)); }
                    await factory.ExecuteMethodAsync().ConfigureAwait(false);
                    return;
                }
                catch (Exception ex)
                {
                    try
                    {
                        lock (aggregatedExceptions) { aggregatedExceptions.Insert(0, ex); }
                        isTransientFault = options.DetectionStrategy(ex);
                        if (attempts >= options.RetryAttempts) { throw; }
                        if (!isTransientFault) { throw; }
                        lastWaitTime = waitTime;
                        totalWaitTime = totalWaitTime.Add(waitTime);
                        attempts++;
                        await Task.Delay(waitTime).ConfigureAwait(false);
                        latency = DateTime.UtcNow.Subtract(timestamp).Subtract(totalWaitTime);
                    }
                    catch (Exception)
                    {
                        throwExceptions = true;
                        if (isTransientFault)
                        {
                            var evidence = new TransientFaultEvidence(attempts, lastWaitTime, totalWaitTime, latency, new MethodDescriptor(factory.DelegateInfo).ToString());
                            aggregatedExceptions.InsertTransientFaultException(evidence);
                            FaultCallback?.Invoke(evidence);
                        }
                        break;
                    }
                }
            }
            if (throwExceptions) { throw new AggregateException(aggregatedExceptions); }
        }

        private static async Task<TResult> WithFuncAsyncCore<TTuple, TResult>(TaskFuncFactory<TTuple, TResult> factory, Action<TransientOperationOptions> setup) where TTuple : Template
        {
            var options = Patterns.Configure(setup);
            if (!options.EnableRecovery) { return await factory.ExecuteMethodAsync().ConfigureAwait(false); }
            DateTime timestamp = DateTime.UtcNow;
            TimeSpan latency = TimeSpan.Zero;
            TimeSpan totalWaitTime = TimeSpan.Zero;
            TimeSpan lastWaitTime = TimeSpan.Zero;
            bool isTransientFault = false;
            bool throwExceptions;
            List<Exception> aggregatedExceptions = new List<Exception>();
            TResult result = default(TResult);
            for (int attempts = 0; ;)
            {
                bool exceptionThrown = false;
                TimeSpan waitTime = options.RetryStrategy(attempts);
                try
                {
                    if (latency > options.MaximumAllowedLatency) { throw new LatencyException(string.Format(CultureInfo.InvariantCulture, "The latency of the operation exceeded the allowed maximum value of {0} seconds. Actual latency was: {1} seconds.", options.MaximumAllowedLatency.TotalSeconds, latency.TotalSeconds)); }
                    return await factory.ExecuteMethodAsync().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    try
                    {
                        lock (aggregatedExceptions) { aggregatedExceptions.Insert(0, ex); }
                        isTransientFault = options.DetectionStrategy(ex);
                        if (attempts >= options.RetryAttempts) { throw; }
                        if (!isTransientFault) { throw; }
                        lastWaitTime = waitTime;
                        totalWaitTime = totalWaitTime.Add(waitTime);
                        attempts++;
                        await Task.Delay(waitTime).ConfigureAwait(false);
                        latency = DateTime.UtcNow.Subtract(timestamp).Subtract(totalWaitTime);
                    }
                    catch (Exception)
                    {
                        throwExceptions = true;
                        exceptionThrown = true;
                        if (isTransientFault)
                        {
                            var evidence = new TransientFaultEvidence(attempts, lastWaitTime, totalWaitTime, latency, new MethodDescriptor(factory.DelegateInfo).ToString());
                            aggregatedExceptions.InsertTransientFaultException(evidence);
                            FaultCallback?.Invoke(evidence);
                        }
                        break;
                    }
                }
                finally
                {
                    if (exceptionThrown)
                    {
                        IDisposable disposable = result as IDisposable;
                        disposable?.Dispose();
                    }
                }
            }
            if (throwExceptions) { throw new AggregateException(aggregatedExceptions); }
            return result;
        }
    }
}