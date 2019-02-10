using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to handle those rare scenarios where you have knowledge about potential exceptions that can be safely ignored.
    /// </summary>
    public static class TesterFuncUtility
    {
        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the method.</typeparam>
        /// <param name="method">The function delegate to invoke to try and get the <typeparamref name="TResult"/>.</param>
        /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> from <paramref name="method"/>, or <b>default</b>(<typeparamref name="TResult"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<TResult>(Func<TResult> method, out TResult result)
        {
            var factory = FuncFactory.Create(method);
            return TryExecuteFunctionCore(factory, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method.</typeparam>
        /// <param name="method">The function delegate to invoke to try and get the <typeparamref name="TResult"/>.</param>
        /// <param name="arg">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> from <paramref name="method"/>, or <b>default</b>(<typeparamref name="TResult"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T, TResult>(Func<T, TResult> method, T arg, out TResult result)
        {
            var factory = FuncFactory.Create(method, arg);
            return TryExecuteFunctionCore(factory, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method.</typeparam>
        /// <param name="method">The function delegate to invoke to try and get the <typeparamref name="TResult"/>.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> from <paramref name="method"/>, or <b>default</b>(<typeparamref name="TResult"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T1, T2, TResult>(Func<T1, T2, TResult> method, T1 arg1, T2 arg2, out TResult result)
        {
            var factory = FuncFactory.Create(method, arg1, arg2);
            return TryExecuteFunctionCore(factory, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method.</typeparam>
        /// <param name="method">The function delegate to invoke to try and get the <typeparamref name="TResult"/>.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> from <paramref name="method"/>, or <b>default</b>(<typeparamref name="TResult"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, T1 arg1, T2 arg2, T3 arg3, out TResult result)
        {
            var factory = FuncFactory.Create(method, arg1, arg2, arg3);
            return TryExecuteFunctionCore(factory, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method.</typeparam>
        /// <param name="method">The function delegate to invoke to try and get the <typeparamref name="TResult"/>.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the <paramref name="method"/>.</param>
        /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> from <paramref name="method"/>, or <b>default</b>(<typeparamref name="TResult"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, out TResult result)
        {
            var factory = FuncFactory.Create(method, arg1, arg2, arg3, arg4);
            return TryExecuteFunctionCore(factory, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the method.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method.</typeparam>
        /// <param name="method">The function delegate to invoke to try and get the <typeparamref name="TResult"/>.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the <paramref name="method"/>.</param>
        /// <param name="result">When this method returns, contains the <typeparamref name="TResult"/> from <paramref name="method"/>, or <b>default</b>(<typeparamref name="TResult"/>) if an exception is thrown.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, out TResult result)
        {
            var factory = FuncFactory.Create(method, arg1, arg2, arg3, arg4, arg5);
            return TryExecuteFunctionCore(factory, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> was invoked without an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <param name="method">The function delegate to invoke.</param>
        /// <param name="arg">The parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T, TResult>(Func<T, TResult> method, T arg)
        {
            TResult result;
            return TryExecuteFunction(method, arg, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> was invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <param name="method">The function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T1, T2, TResult>(Func<T1, T2, TResult> method, T1 arg1, T2 arg2)
        {
            TResult result;
            return TryExecuteFunction(method, arg1, arg2, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> was invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <param name="method">The function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T1, T2, T3, TResult>(Func<T1, T2, T3, TResult> method, T1 arg1, T2 arg2, T3 arg3)
        {
            TResult result;
            return TryExecuteFunction(method, arg1, arg2, arg3, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> was invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <param name="method">The function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T1, T2, T3, T4, TResult>(Func<T1, T2, T3, T4, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            TResult result;
            return TryExecuteFunction(method, arg1, arg2, arg3, arg4, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> was invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the method.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <param name="method">The function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static bool TryExecuteFunction<T1, T2, T3, T4, T5, TResult>(Func<T1, T2, T3, T4, T5, TResult> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            TResult result;
            return TryExecuteFunction(method, arg1, arg2, arg3, arg4, arg5, out result);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <param name="method">The delegate to invoke.</param>
        /// <returns><c>true</c> if the delegate <paramref name="method"/> was invoked without triggering an exception; otherwise <c>false</c>.</returns>
        public static bool TryExecuteAction(Action method)
        {
            var factory = ActionFactory.Create(method);
            return TryExecuteActionCore(factory);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the method.</typeparam>
        /// <param name="method">The delegate to invoke.</param>
        /// <param name="arg">The first parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if the delegate <paramref name="method"/> was invoked without triggering an exception; otherwise <c>false</c>.</returns>
        public static bool TryExecuteAction<T>(Action<T> method, T arg)
        {
            var factory = ActionFactory.Create(method, arg);
            return TryExecuteActionCore(factory);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <param name="method">The delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if the delegate <paramref name="method"/> was invoked without triggering an exception; otherwise <c>false</c>.</returns>
        public static bool TryExecuteAction<T1, T2>(Action<T1, T2> method, T1 arg1, T2 arg2)
        {
            var factory = ActionFactory.Create(method, arg1, arg2);
            return TryExecuteActionCore(factory);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <param name="method">The delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if the delegate <paramref name="method"/> was invoked without triggering an exception; otherwise <c>false</c>.</returns>
        public static bool TryExecuteAction<T1, T2, T3>(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            var factory = ActionFactory.Create(method, arg1, arg2, arg3);
            return TryExecuteActionCore(factory);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the method.</typeparam>
        /// <param name="method">The delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if the delegate <paramref name="method"/> was invoked without triggering an exception; otherwise <c>false</c>.</returns>
        public static bool TryExecuteAction<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4);
            return TryExecuteActionCore(factory);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="method"/> can be invoked without an exception.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the method.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the method.</typeparam>
        /// <param name="method">The delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="method"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="method"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="method"/>.</param>
        /// <param name="arg4">The fourth parameter of the <paramref name="method"/>.</param>
        /// <param name="arg5">The fifth parameter of the <paramref name="method"/>.</param>
        /// <returns><c>true</c> if the delegate <paramref name="method"/> was invoked without triggering an exception; otherwise <c>false</c>.</returns>
        public static bool TryExecuteAction<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4, arg5);
            return TryExecuteActionCore(factory);
        }

        private static bool TryExecuteActionCore<TTuple>(ActionFactory<TTuple> factory) where TTuple : Template
        {
            try
            {
                factory.ExecuteMethod();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TryExecuteFunctionCore<TTuple, TResult>(FuncFactory<TTuple, TResult> factory, out TResult result) where TTuple : Template
        {
            try
            {
                result = factory.ExecuteMethod();
                return true;
            }
            catch (Exception)
            {
                result = default(TResult);
                return false;
            }
        }
    }
}