using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make common delegate operations easier to work with.
    /// </summary>
    public static class DelegateUtility
    {
        /// <summary>
        /// Provides an easy and reflection less way to get a value from a property or field that is delegate compatible (such as <see cref="Func{TResult}"/> and the likes thereof).
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="output"/>.</typeparam>
        /// <param name="output">The value of the member to be routed as output through this Wrap{TResult} method.</param>
        /// <returns>The value from <paramref name="output"/>.</returns>
        public static TResult Wrap<TResult>(TResult output)
        {
            return output;
        }

        /// <summary>
        /// Provides a way to dynamically wrap a return value (typically from a property or field) inside an anonymous method.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of <paramref name="output" />.</typeparam>
        /// <param name="output">The value to dynamically wrap as a return value (typically from a property of field) inside an anonymous method.</param>
        /// <returns>An anonymous method that returns the value of <paramref name="output" />.</returns>
        public static Func<TResult> DynamicWrap<TResult>(TResult output)
        {
            return () => output;
        }

        /// <summary>
        /// If the specified <paramref name="body"/> meets the condition of not being null, it will be invoked.
        /// </summary>
        /// <param name="body">The delegate to invoke when assigned a method.</param>
        public static void InvokeIfNotNull(Action body)
        {
            if (Condition.IsNull(body)) { return; }
            body();
        }

        /// <summary>
        /// If the specified <paramref name="body"/> meets the condition of not being null, it will be invoked with a single generic parameter.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <param name="body">The delegate to invoke when assigned a method.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="body"/>.</param>
        public static void InvokeIfNotNull<T>(Action<T> body, T arg)
        {
            if (Condition.IsNull(body)) { return; }
            body(arg);
        }

        /// <summary>
        /// If the specified <paramref name="body"/> meets the condition of not being null, it will be invoked with two generic parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <param name="body">The delegate to invoke when assigned a method.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body"/>.</param>
        public static void InvokeIfNotNull<T1, T2>(Action<T1, T2> body, T1 arg1, T2 arg2)
        {
            if (Condition.IsNull(body)) { return; }
            body(arg1, arg2);
        }

        /// <summary>
        /// If the specified <paramref name="body"/> meets the condition of not being null, it will be invoked with three generic parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <param name="body">The delegate to invoke when assigned a method.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body"/>.</param>
        public static void InvokeIfNotNull<T1, T2, T3>(Action<T1, T2, T3> body, T1 arg1, T2 arg2, T3 arg3)
        {
            if (Condition.IsNull(body)) { return; }
            body(arg1, arg2, arg3);
        }

        /// <summary>
        /// If the specified <paramref name="body"/> meets the condition of not being null, it will be invoked with four generic parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <param name="body">The delegate to invoke when assigned a method.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body"/>.</param>
        public static void InvokeIfNotNull<T1, T2, T3, T4>(Action<T1, T2, T3, T4> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (Condition.IsNull(body)) { return; }
            body(arg1, arg2, arg3, arg4);
        }

        /// <summary>
        /// If the specified <paramref name="body"/> meets the condition of not being null, it will be invoked with five generic parameters.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="body"/>.</typeparam>
        /// <param name="body">The delegate to invoke when assigned a method.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="body"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="body"/>.</param>
        public static void InvokeIfNotNull<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> body, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (Condition.IsNull(body)) { return; }
            body(arg1, arg2, arg3, arg4, arg5);
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate to invoke following the rule description of CA2000 (Dispose objects before losing scope).</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/>.</returns>
        public static TResult SafeInvokeDisposable<TResult>(Func<TResult> initializer) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            TResult result;
            TResult temp = null;
            try
            {
                temp = initializer();
                result = temp;
                temp = null;
            }
            finally
            {
                temp?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate to invoke following the rule description of CA2000 (Dispose objects before losing scope).</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/>.</returns>
        public static TResult SafeInvokeDisposable<T, TResult>(Func<T, TResult> initializer, T arg) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            TResult result;
            TResult temp = null;
            try
            {
                temp = initializer(arg);
                result = temp;
                temp = null;
            }
            finally
            {
                temp?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate to invoke following the rule description of CA2000 (Dispose objects before losing scope).</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/>.</returns>
        public static TResult SafeInvokeDisposable<T1, T2, TResult>(Func<T1, T2, TResult> initializer, T1 arg1, T2 arg2) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            TResult result;
            TResult temp = null;
            try
            {
                temp = initializer(arg1, arg2);
                result = temp;
                temp = null;
            }
            finally
            {
                temp?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate to invoke following the rule description of CA2000 (Dispose objects before losing scope).</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/>.</returns>
        public static TResult SafeInvokeDisposable<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> initializer, T1 arg1, T2 arg2, T3 arg3) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            TResult result;
            TResult temp = null;
            try
            {
                temp = initializer(arg1, arg2, arg3);
                result = temp;
                temp = null;
            }
            finally
            {
                temp?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate to invoke following the rule description of CA2000 (Dispose objects before losing scope).</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/>.</returns>
        public static TResult SafeInvokeDisposable<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> initializer, T1 arg1, T2 arg2, T3 arg3, T4 arg4) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            TResult result;
            TResult temp = null;
            try
            {
                temp = initializer(arg1, arg2, arg3, arg4);
                result = temp;
                temp = null;
            }
            finally
            {
                temp?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Provides a generic way to abide the rule description of CA2000 (Dispose objects before losing scope).
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="initializer"/>.</typeparam>
        /// <param name="initializer">The function delegate to invoke following the rule description of CA2000 (Dispose objects before losing scope).</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="initializer"/>.</param>
        /// <returns>The return value of the function delegate <paramref name="initializer"/>.</returns>
        public static TResult SafeInvokeDisposable<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> initializer, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) where TResult : class, IDisposable
        {
            Validator.ThrowIfNull(initializer, nameof(initializer));
            TResult result;
            TResult temp = null;
            try
            {
                temp = initializer(arg1, arg2, arg3, arg4, arg5);
                result = temp;
                temp = null;
            }
            finally
            {
                temp?.Dispose();
            }
            return result;
        }

        /// <summary>
        /// Provides a generic way to support the options pattern which enables using custom options classes to represent a group of related settings.
        /// </summary>
        /// <typeparam name="TOptions">The type of the custom options class.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <typeparamref name="TOptions"/>.</param>
        /// <param name="initializer">The function delegate that will initialize the default, parameterless constructed instance of <typeparamref name="TOptions"/>.</param>
        /// <returns>A default constructed instance of <typeparamref name="TOptions"/> initialized with the options of <paramref name="setup"/>.</returns>
        public static TOptions ConfigureAction<TOptions>(Action<TOptions> setup, Action<TOptions> initializer = null)
        {
            TOptions options = Activator.CreateInstance<TOptions>();
            initializer?.Invoke(options);
            setup?.Invoke(options);
            return options;
        }
    }
}