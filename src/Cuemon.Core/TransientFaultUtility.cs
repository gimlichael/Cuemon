using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Cuemon
{
    /// <summary>
    /// Provides developers ways to make their applications more resilient by adding robust transient fault handling logic ideal for temporary condition such as network connectivity issues or service unavailability.
    /// </summary>
    public static class TransientFaultUtility
    {
        private static byte _defaultRetryAttempts = 5;

        /// <summary>
        /// Gets or sets the default amount of retry attempts for transient faults. Default is 5 retry attempts.
        /// </summary>
        /// <value>The default amount of retry attempts for transient faults.</value>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="value"/> is zero.
        /// </exception>
        public static byte DefaultRetryAttempts
        {
            get { return _defaultRetryAttempts; }
            set
            {
                if (value == 0) { throw new ArgumentException("Value must be greater than zero.", nameof(value)); }
                _defaultRetryAttempts = value;
            }
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<TResult>(Doer<Exception, bool> isTransientFault, Doer<TResult> faultSensitiveMethod)
        {
            return ExecuteFunction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<TResult>(int retryAttempts, Doer<Exception, bool> isTransientFault, Doer<TResult> faultSensitiveMethod)
        {
            return ExecuteFunction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T, TResult>(Doer<Exception, bool> isTransientFault, Doer<T, TResult> faultSensitiveMethod, T arg)
        {
            return ExecuteFunction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T, TResult>(int retryAttempts, Doer<Exception, bool> isTransientFault, Doer<T, TResult> faultSensitiveMethod, T arg)
        {
            return ExecuteFunction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T1, T2, TResult>(Doer<Exception, bool> isTransientFault, Doer<T1, T2, TResult> faultSensitiveMethod, T1 arg1, T2 arg2)
        {
            return ExecuteFunction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T1, T2, TResult>(int retryAttempts, Doer<Exception, bool> isTransientFault, Doer<T1, T2, TResult> faultSensitiveMethod, T1 arg1, T2 arg2)
        {
            return ExecuteFunction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T1, T2, T3, TResult>(Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            return ExecuteFunction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T1, T2, T3, TResult>(int retryAttempts, Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            return ExecuteFunction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T1, T2, T3, T4, TResult>(Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, T4, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return ExecuteFunction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T1, T2, T3, T4, TResult>(int retryAttempts, Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, T4, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            return ExecuteFunction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3, arg4);
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
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T1, T2, T3, T4, T5, TResult>(Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, T4, T5, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return ExecuteFunction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
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
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TResult ExecuteFunction<T1, T2, T3, T4, T5, TResult>(int retryAttempts, Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, T4, T5, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            return ExecuteFunction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<TResult, TSuccess>(Doer<Exception, bool> isTransientFault, TesterDoer<TResult, TSuccess> faultSensitiveMethod, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod);
            return TryExecuteFunctionCore(factory, out result, DefaultRetryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T, TResult, TSuccess>(Doer<Exception, bool> isTransientFault, TesterDoer<T, TResult, TSuccess> faultSensitiveMethod, T arg, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg);
            return TryExecuteFunctionCore(factory, out result, DefaultRetryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T1, T2, TResult, TSuccess>(Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2);
            return TryExecuteFunctionCore(factory, out result, DefaultRetryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T1, T2, T3, TResult, TSuccess>(Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            return TryExecuteFunctionCore(factory, out result, DefaultRetryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T1, T2, T3, T4, TResult, TSuccess>(Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, T4, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            return TryExecuteFunctionCore(factory, out result, DefaultRetryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T1, T2, T3, T4, T5, TResult, TSuccess>(Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, T4, T5, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            return TryExecuteFunctionCore(factory, out result, DefaultRetryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<TResult, TSuccess>(int retryAttempts, Doer<Exception, bool> isTransientFault, TesterDoer<TResult, TSuccess> faultSensitiveMethod, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T, TResult, TSuccess>(int retryAttempts, Doer<Exception, bool> isTransientFault, TesterDoer<T, TResult, TSuccess> faultSensitiveMethod, T arg, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T1, T2, TResult, TSuccess>(int retryAttempts, Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T1, T2, T3, TResult, TSuccess>(int retryAttempts, Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T1, T2, T3, T4, TResult, TSuccess>(int retryAttempts, Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, T4, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static TSuccess TryExecuteFunction<T1, T2, T3, T4, T5, TResult, TSuccess>(int retryAttempts, Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, T4, T5, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, RecoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static TResult ExecuteFunction<TResult>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Doer<TResult> faultSensitiveMethod)
        {
            var factory = DoerFactory.Create(faultSensitiveMethod);
            return ExecuteFunctionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static TResult ExecuteFunction<T, TResult>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Doer<T, TResult> faultSensitiveMethod, T arg)
        {
            var factory = DoerFactory.Create(faultSensitiveMethod, arg);
            return ExecuteFunctionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static TResult ExecuteFunction<T1, T2, TResult>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Doer<T1, T2, TResult> faultSensitiveMethod, T1 arg1, T2 arg2)
        {
            var factory = DoerFactory.Create(faultSensitiveMethod, arg1, arg2);
            return ExecuteFunctionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static TResult ExecuteFunction<T1, T2, T3, TResult>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            var factory = DoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            return ExecuteFunctionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static TResult ExecuteFunction<T1, T2, T3, T4, TResult>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, T4, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var factory = DoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            return ExecuteFunctionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
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
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        public static TResult ExecuteFunction<T1, T2, T3, T4, T5, TResult>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Doer<T1, T2, T3, T4, T5, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var factory = DoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            return ExecuteFunctionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryExecuteFunction<TResult, TSuccess>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, TesterDoer<TResult, TSuccess> faultSensitiveMethod, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryExecuteFunction<T, TResult, TSuccess>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, TesterDoer<T, TResult, TSuccess> faultSensitiveMethod, T arg, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryExecuteFunction<T1, T2, TResult, TSuccess>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryExecuteFunction<T1, T2, T3, TResult, TSuccess>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryExecuteFunction<T1, T2, T3, T4, TResult, TSuccess>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, T4, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryExecuteFunction<T1, T2, T3, T4, T5, TResult, TSuccess>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, TesterDoer<T1, T2, T3, T4, T5, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TResult result)
        {
            var factory = TesterDoerFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            return TryExecuteFunctionCore(factory, out result, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        public static void ExecuteAction(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Act faultSensitiveMethod)
        {
            var factory = ActFactory.Create(faultSensitiveMethod);
            ExecuteActionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        public static void ExecuteAction<T>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Act<T> faultSensitiveMethod, T arg)
        {
            var factory = ActFactory.Create(faultSensitiveMethod, arg);
            ExecuteActionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        public static void ExecuteAction<T1, T2>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Act<T1, T2> faultSensitiveMethod, T1 arg1, T2 arg2)
        {
            var factory = ActFactory.Create(faultSensitiveMethod, arg1, arg2);
            ExecuteActionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        public static void ExecuteAction<T1, T2, T3>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Act<T1, T2, T3> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            var factory = ActFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            ExecuteActionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        public static void ExecuteAction<T1, T2, T3, T4>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Act<T1, T2, T3, T4> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var factory = ActFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            ExecuteActionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="recoveryWaitTime">The function delegate that returns a <see cref="TimeSpan"/> specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        public static void ExecuteAction<T1, T2, T3, T4, T5>(int retryAttempts, Doer<int, TimeSpan> recoveryWaitTime, Doer<Exception, bool> isTransientFault, Act<T1, T2, T3, T4, T5> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var factory = ActFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            ExecuteActionCore(factory, retryAttempts, recoveryWaitTime, isTransientFault);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction(Doer<Exception, bool> isTransientFault, Act faultSensitiveMethod)
        {
            ExecuteAction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction(int retryAttempts, Doer<Exception, bool> isTransientFault, Act faultSensitiveMethod)
        {
            ExecuteAction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T>(Doer<Exception, bool> isTransientFault, Act<T> faultSensitiveMethod, T arg)
        {
            ExecuteAction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T>(int retryAttempts, Doer<Exception, bool> isTransientFault, Act<T> faultSensitiveMethod, T arg)
        {
            ExecuteAction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T1, T2>(Doer<Exception, bool> isTransientFault, Act<T1, T2> faultSensitiveMethod, T1 arg1, T2 arg2)
        {
            ExecuteAction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T1, T2>(int retryAttempts, Doer<Exception, bool> isTransientFault, Act<T1, T2> faultSensitiveMethod, T1 arg1, T2 arg2)
        {
            ExecuteAction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T1, T2, T3>(Doer<Exception, bool> isTransientFault, Act<T1, T2, T3> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            ExecuteAction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T1, T2, T3>(int retryAttempts, Doer<Exception, bool> isTransientFault, Act<T1, T2, T3> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3)
        {
            ExecuteAction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T1, T2, T3, T4>(Doer<Exception, bool> isTransientFault, Act<T1, T2, T3, T4> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            ExecuteAction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T1, T2, T3, T4>(int retryAttempts, Doer<Exception, bool> isTransientFault, Act<T1, T2, T3, T4> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            ExecuteAction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using <see cref="DefaultRetryAttempts"/> for specifying the amount of retry attempts and the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T1, T2, T3, T4, T5>(Doer<Exception, bool> isTransientFault, Act<T1, T2, T3, T4, T5> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            ExecuteAction(DefaultRetryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="retryAttempts">The amount of retry attempts for transient faults.</param>
        /// <param name="isTransientFault">The function delegate that returns <c>true</c> if the failed operations contains clues that would suggest a transient fault; otherwise, <c>false</c>.</param>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <remarks>Defaults to using the <see cref="RecoveryWaitTime"/> function implementation for specifying the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.</remarks>
        public static void ExecuteAction<T1, T2, T3, T4, T5>(int retryAttempts, Doer<Exception, bool> isTransientFault, Act<T1, T2, T3, T4, T5> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            ExecuteAction(retryAttempts, RecoveryWaitTime, isTransientFault, faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Specifies the amount of time to wait for a transient fault to recover gracefully before trying a new attempt.
        /// </summary>
        /// <param name="currentAttempt">The current attempt.</param>
        /// <returns>A <see cref="TimeSpan"/> that defines the amount of time to wait for a transient fault to recover gracefully.</returns>
        /// <remarks>Default implementation is <paramref name="currentAttempt"/> + 2^ to a maximum of 5; eg. 1, 2, 4, 8, 16 to a total of 32 seconds.</remarks>
        public static TimeSpan RecoveryWaitTime(int currentAttempt)
        {
            return TimeSpan.FromSeconds(Math.Pow(2, currentAttempt > 5 ? 5 : currentAttempt));
        }

        private static void ExecuteActionCore<TTuple>(ActFactory<TTuple> factory, int retryAttempts, Doer<int, TimeSpan> recoveryWaitTimeCallback, Doer<Exception, bool> isTransientFaultCallback) where TTuple : Template
        {
            TimeSpan totalWaitTime = TimeSpan.Zero;
            TimeSpan lastWaitTime = TimeSpan.Zero;
            bool isTransientFault = false;
            bool throwExceptions;
            List<Exception> aggregatedExceptions = new List<Exception>();
            for (int attempts = 0; ;)
            {
                TimeSpan waitTime = recoveryWaitTimeCallback(attempts);
                try
                {
                    factory.ExecuteMethod();
                    return;
                }
                catch (Exception ex)
                {
                    try
                    {
                        lock (aggregatedExceptions) { aggregatedExceptions.Insert(0, ex); }
                        isTransientFault = isTransientFaultCallback(ex);
                        if (attempts >= retryAttempts) { throw; }
                        if (!isTransientFault) { throw; }
                        lastWaitTime = waitTime;
                        totalWaitTime = totalWaitTime.Add(waitTime);
                        attempts++;
                        Task.Delay(waitTime).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        throwExceptions = true;
                        if (isTransientFault) { aggregatedExceptions.InsertTransientFaultException(ex, attempts, retryAttempts, lastWaitTime, totalWaitTime); }
                        break;
                    }
                }
            }
            if (throwExceptions) { throw new AggregateException(aggregatedExceptions); }
        }

        private static TResult ExecuteFunctionCore<TTuple, TResult>(DoerFactory<TTuple, TResult> factory, int retryAttempts, Doer<int, TimeSpan> recoveryWaitTimeCallback, Doer<Exception, bool> isTransientFaultCallback) where TTuple : Template
        {
            TimeSpan totalWaitTime = TimeSpan.Zero;
            TimeSpan lastWaitTime = TimeSpan.Zero;
            bool isTransientFault = false;
            bool throwExceptions;
            List<Exception> aggregatedExceptions = new List<Exception>();
            TResult result = default(TResult);
            for (int attempts = 0; ;)
            {
                bool exceptionThrown = false;
                TimeSpan waitTime = recoveryWaitTimeCallback(attempts);
                try
                {
                    return factory.ExecuteMethod();
                }
                catch (Exception ex)
                {
                    try
                    {
                        lock (aggregatedExceptions) { aggregatedExceptions.Insert(0, ex); }
                        isTransientFault = isTransientFaultCallback(ex);
                        if (attempts >= retryAttempts) { throw; }
                        if (!isTransientFault) { throw; }
                        lastWaitTime = waitTime;
                        totalWaitTime = totalWaitTime.Add(waitTime);
                        attempts++;
                        Task.Delay(waitTime).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        throwExceptions = true;
                        exceptionThrown = true;
                        if (isTransientFault) { aggregatedExceptions.InsertTransientFaultException(ex, attempts, retryAttempts, lastWaitTime, totalWaitTime); }
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

        private static TSuccess TryExecuteFunctionCore<TTuple, TSuccess, TResult>(TesterDoerFactory<TTuple, TResult, TSuccess> factory, out TResult result, int retryAttempts, Doer<int, TimeSpan> recoveryWaitTimeCallback, Doer<Exception, bool> isTransientFaultCallback) where TTuple : Template
        {
            result = default(TResult);
            TimeSpan totalWaitTime = TimeSpan.Zero;
            TimeSpan lastWaitTime = TimeSpan.Zero;
            bool throwExceptions;
            bool isTransientFault = false;
            List<Exception> aggregatedExceptions = new List<Exception>();
            for (int attempts = 0; ;)
            {
                bool exceptionThrown = false;
                TimeSpan waitTime = recoveryWaitTimeCallback(attempts);
                try
                {
                    return factory.ExecuteMethod(out result);
                }
                catch (Exception ex)
                {
                    try
                    {
                        lock (aggregatedExceptions) { aggregatedExceptions.Insert(0, ex); }
                        isTransientFault = isTransientFaultCallback(ex);
                        if (attempts >= retryAttempts) { throw; }
                        if (!isTransientFault) { throw; }
                        lastWaitTime = waitTime;
                        totalWaitTime = totalWaitTime.Add(waitTime);
                        attempts++;
                        Task.Delay(waitTime).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        throwExceptions = true;
                        exceptionThrown = true;
                        if (isTransientFault) { aggregatedExceptions.InsertTransientFaultException(ex, attempts, retryAttempts, lastWaitTime, totalWaitTime); }
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
            return default(TSuccess);
        }

        private static void InsertTransientFaultException(this IList<Exception> aggregatedExceptions, Exception ex, int attempts, int retryAttempts, TimeSpan lastWaitTime, TimeSpan totalWaitTime)
        {
            TransientFaultException transientException = new TransientFaultException(attempts >= retryAttempts ? "The amount of retry attempts has been reached." : "An unhandled exception occurred during the execution of the current operation.", ex);
            transientException.Data.Add("Attempts", (attempts).ToString(CultureInfo.InvariantCulture));
            transientException.Data.Add("RecoveryWaitTimeInSeconds", lastWaitTime.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            transientException.Data.Add("TotalRecoveryWaitTimeInSeconds", totalWaitTime.TotalSeconds.ToString(CultureInfo.InvariantCulture));
            lock (aggregatedExceptions) { aggregatedExceptions.Insert(0, transientException); }
        }
    }
}