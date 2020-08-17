using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Cuemon.Reflection;

namespace Cuemon.Resilience
{
    /// <summary>
    /// Provides a set of static methods that enable developers to make their applications more resilient by adding robust transient fault handling logic ideal for temporary condition such as network connectivity issues or service unavailability.
    /// </summary>
    public static partial class TransientOperation
    {
        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static TResult WithFunc<TResult>(Func<TResult> faultSensitiveMethod, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = FuncFactory.Create(faultSensitiveMethod);
            return WithFuncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static TResult WithFunc<T, TResult>(Func<T, TResult> faultSensitiveMethod, T arg, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = FuncFactory.Create(faultSensitiveMethod, arg);
            return WithFuncCore(factory, setup);
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
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static TResult WithFunc<T1, T2, TResult>(Func<T1, T2, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = FuncFactory.Create(faultSensitiveMethod, arg1, arg2);
            return WithFuncCore(factory, setup);
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
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static TResult WithFunc<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = FuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            return WithFuncCore(factory, setup);
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
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static TResult WithFunc<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = FuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            return WithFuncCore(factory, setup);
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
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The result from the <paramref name="faultSensitiveMethod"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static TResult WithFunc<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = FuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            return WithFuncCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryWithFunc<TResult, TSuccess>(TesterFunc<TResult, TSuccess> faultSensitiveMethod, out TResult result, Action<TransientOperationOptions> setup = null)
        {
            var factory = TesterFuncFactory.Create(faultSensitiveMethod);
            return TryWithFuncCore(factory, out result, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryWithFunc<T, TResult, TSuccess>(TesterFunc<T, TResult, TSuccess> faultSensitiveMethod, T arg, out TResult result, Action<TransientOperationOptions> setup = null)
        {
            var factory = TesterFuncFactory.Create(faultSensitiveMethod, arg);
            return TryWithFuncCore(factory, out result, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryWithFunc<T1, T2, TResult, TSuccess>(TesterFunc<T1, T2, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, out TResult result, Action<TransientOperationOptions> setup = null)
        {
            var factory = TesterFuncFactory.Create(faultSensitiveMethod, arg1, arg2);
            return TryWithFuncCore(factory, out result, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TResult">The type of the out result value of the function delegate encapsulates <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryWithFunc<T1, T2, T3, TResult, TSuccess>(TesterFunc<T1, T2, T3, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, out TResult result, Action<TransientOperationOptions> setup = null)
        {
            var factory = TesterFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            return TryWithFuncCore(factory, out result, setup);
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
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryWithFunc<T1, T2, T3, T4, TResult, TSuccess>(TesterFunc<T1, T2, T3, T4, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TResult result, Action<TransientOperationOptions> setup = null)
        {
            var factory = TesterFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            return TryWithFuncCore(factory, out result, setup);
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
        /// <param name="faultSensitiveMethod">The fault sensitive function delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="result">The result of the function delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <returns>The return value that indicates success of the function delegate <paramref name="faultSensitiveMethod"/>.</returns>
        public static TSuccess TryWithFunc<T1, T2, T3, T4, T5, TResult, TSuccess>(TesterFunc<T1, T2, T3, T4, T5, TResult, TSuccess> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TResult result, Action<TransientOperationOptions> setup = null)
        {
            var factory = TesterFuncFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            return TryWithFuncCore(factory, out result, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static void WithAction(Action faultSensitiveMethod, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = ActionFactory.Create(faultSensitiveMethod);
            WithActionCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        public static void WithAction<T>(Action<T> faultSensitiveMethod, T arg, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = ActionFactory.Create(faultSensitiveMethod, arg);
            WithActionCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static void WithAction<T1, T2>(Action<T1, T2> faultSensitiveMethod, T1 arg1, T2 arg2, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = ActionFactory.Create(faultSensitiveMethod, arg1, arg2);
            WithActionCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static void WithAction<T1, T2, T3>(Action<T1, T2, T3> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = ActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3);
            WithActionCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static void WithAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = ActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4);
            WithActionCore(factory, setup);
        }

        /// <summary>
        /// Repetitively executes the specified <paramref name="faultSensitiveMethod"/> until the operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</typeparam>
        /// <param name="faultSensitiveMethod">The fault sensitive delegate that is invoked until an operation is successful, the amount of retry attempts has been reached, or a failed operation is not considered related to transient fault condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="faultSensitiveMethod"/>.</param>
        /// <param name="setup">The <see cref="TransientOperationOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="faultSensitiveMethod"/> cannot be null.
        /// </exception>
        public static void WithAction<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> faultSensitiveMethod, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TransientOperationOptions> setup = null)
        {
            Validator.ThrowIfNull(faultSensitiveMethod, nameof(faultSensitiveMethod));
            var factory = ActionFactory.Create(faultSensitiveMethod, arg1, arg2, arg3, arg4, arg5);
            WithActionCore(factory, setup);
        }

        /// <summary>
        /// Gets or sets the callback delegate that is invoked when a transient fault occurs.
        /// </summary>
        /// <value>A callback delegate that is invoked when a transient fault occurs.</value>
        public static Action<TransientFaultEvidence> FaultCallback { get; set; }

        private static void WithActionCore<TTuple>(ActionFactory<TTuple> factory, Action<TransientOperationOptions> setup = null) where TTuple : Template
        {
            var options = Patterns.Configure(setup);
            if (!options.EnableRecovery)
            {
                factory.ExecuteMethod();
                return;
            }
            var timestamp = DateTime.UtcNow;
            var latency = TimeSpan.Zero;
            var totalWaitTime = TimeSpan.Zero;
            var lastWaitTime = TimeSpan.Zero;
            var isTransientFault = false;
            var throwExceptions = false;
            var aggregatedExceptions = new List<Exception>();
            for (var attempts = 0; ;)
            {
                var waitTime = options.RetryStrategy(attempts);
                try
                {
                    if (latency > options.MaximumAllowedLatency) { throw new LatencyException(string.Format(CultureInfo.InvariantCulture, "The latency of the operation exceeded the allowed maximum value of {0} seconds. Actual latency was: {1} seconds.", options.MaximumAllowedLatency.TotalSeconds, latency.TotalSeconds)); }
                    factory.ExecuteMethod();
                    break;
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
                        Sleep(waitTime);
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

        private static TResult WithFuncCore<TTuple, TResult>(FuncFactory<TTuple, TResult> factory, Action<TransientOperationOptions> setup) where TTuple : Template
        {
            var options = Patterns.Configure(setup);
            if (!options.EnableRecovery) { return factory.ExecuteMethod(); }
            var timestamp = DateTime.UtcNow;
            var latency = TimeSpan.Zero;
            var totalWaitTime = TimeSpan.Zero;
            var lastWaitTime = TimeSpan.Zero;
            var isTransientFault = false;
            var throwExceptions = false;
            var aggregatedExceptions = new List<Exception>();
            var result = default(TResult);
            for (var attempts = 0; ;)
            {
                var waitTime = options.RetryStrategy(attempts);
                try
                {
                    if (latency > options.MaximumAllowedLatency) { throw new LatencyException(string.Format(CultureInfo.InvariantCulture, "The latency of the operation exceeded the allowed maximum value of {0} seconds. Actual latency was: {1} seconds.", options.MaximumAllowedLatency.TotalSeconds, latency.TotalSeconds)); }
                    result = factory.ExecuteMethod();
                    break;
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
                        Sleep(waitTime);
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
                finally
                {
                    if (throwExceptions)
                    {
                        var disposable = result as IDisposable;
                        disposable?.Dispose();
                    }
                }
            }
            if (throwExceptions) { throw new AggregateException(aggregatedExceptions); }
            return result;
        }

        private static TSuccess TryWithFuncCore<TTuple, TSuccess, TResult>(TesterFuncFactory<TTuple, TResult, TSuccess> factory, out TResult result, Action<TransientOperationOptions> setup) where TTuple : Template
        {
            result = default;
            var options = Patterns.Configure(setup);
            if (!options.EnableRecovery) { return factory.ExecuteMethod(out result); }
            var timestamp = DateTime.UtcNow;
            var latency = TimeSpan.Zero;
            var totalWaitTime = TimeSpan.Zero;
            var lastWaitTime = TimeSpan.Zero;
            bool throwExceptions;
            var isTransientFault = false;
            var aggregatedExceptions = new List<Exception>();
            for (var attempts = 0; ;)
            {
                var exceptionThrown = false;
                var waitTime = options.RetryStrategy(attempts);
                try
                {
                    if (latency > options.MaximumAllowedLatency) { throw new LatencyException(string.Format(CultureInfo.InvariantCulture, "The latency of the operation exceeded the allowed maximum value of {0} seconds. Actual latency was: {1} seconds.", options.MaximumAllowedLatency.TotalSeconds, latency.TotalSeconds)); }
                    return factory.ExecuteMethod(out result);
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
                        Sleep(waitTime);
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
                        var disposable = result as IDisposable;
                        disposable?.Dispose();
                    }
                }
            }
            if (throwExceptions) { throw new AggregateException(aggregatedExceptions); }
            return default;
        }

        private static void Sleep(TimeSpan sleep)
        {
            new ManualResetEvent(false).WaitOne(sleep);
        }

        private static void InsertTransientFaultException(this IList<Exception> aggregatedExceptions, TransientFaultEvidence evidence)
        {
            var transientException = new TransientFaultException("The amount of retry attempts has been reached.", evidence);
            lock (aggregatedExceptions) { aggregatedExceptions.Insert(0, transientException); }
        }
    }
}