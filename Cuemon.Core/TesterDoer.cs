namespace Cuemon
{
    /// <summary>
    /// Provides a set of methods that can assist with the tester-doer pattern.
    /// </summary>
    public static class TesterDoer
    {
        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="testerDoerBody"/> was invoked as <typeparamref name="TSuccess"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the method.</typeparam>
        /// <param name="testerDoerBody">The tester function delegate to invoke.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static TSuccess IgnoreResult<TResult, TSuccess>(TesterFunc<TResult, TSuccess> testerDoerBody)
        {
            Validator.ThrowIfNull(testerDoerBody, nameof(testerDoerBody));
            TResult ignore;
            return testerDoerBody(out ignore);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="testerDoerBody"/> was invoked as <typeparamref name="TSuccess"/>.
        /// </summary>
        /// <typeparam name="T">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the method.</typeparam>
        /// <param name="testerDoerBody">The tester function delegate to invoke.</param>
        /// <param name="arg">The first parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static TSuccess IgnoreResult<T, TResult, TSuccess>(TesterFunc<T, TResult, TSuccess> testerDoerBody, T arg)
        {
            Validator.ThrowIfNull(testerDoerBody, nameof(testerDoerBody));
            TResult ignore;
            return testerDoerBody(arg, out ignore);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="testerDoerBody"/> was invoked as <typeparamref name="TSuccess"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the method.</typeparam>
        /// <param name="testerDoerBody">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static TSuccess IgnoreResult<T1, T2, TResult, TSuccess>(TesterFunc<T1, T2, TResult, TSuccess> testerDoerBody, T1 arg1, T2 arg2)
        {
            Validator.ThrowIfNull(testerDoerBody, nameof(testerDoerBody));
            TResult ignore;
            return testerDoerBody(arg1, arg2, out ignore);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="testerDoerBody"/> was invoked as <typeparamref name="TSuccess"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the method.</typeparam>
        /// <param name="testerDoerBody">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static TSuccess IgnoreResult<T1, T2, T3, TResult, TSuccess>(TesterFunc<T1, T2, T3, TResult, TSuccess> testerDoerBody, T1 arg1, T2 arg2, T3 arg3)
        {
            Validator.ThrowIfNull(testerDoerBody, nameof(testerDoerBody));
            TResult ignore;
            return testerDoerBody(arg1, arg2, arg3, out ignore);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="testerDoerBody"/> was invoked as <typeparamref name="TSuccess"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the method.</typeparam>
        /// <param name="testerDoerBody">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg4">The fourth parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static TSuccess IgnoreResult<T1, T2, T3, T4, TResult, TSuccess>(TesterFunc<T1, T2, T3, T4, TResult, TSuccess> testerDoerBody, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Validator.ThrowIfNull(testerDoerBody, nameof(testerDoerBody));
            TResult ignore;
            return testerDoerBody(arg1, arg2, arg3, arg4, out ignore);
        }

        /// <summary>
        /// Returns a value that indicates whether the specified <paramref name="testerDoerBody"/> was invoked as <typeparamref name="TSuccess"/>.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the method.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the method.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the method.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the method.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the method.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that you explicitly have chosen to ignore.</typeparam>
        /// <typeparam name="TSuccess">The type of the return value that indicates success of the method.</typeparam>
        /// <param name="testerDoerBody">The tester function delegate to invoke.</param>
        /// <param name="arg1">The first parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg2">The second parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg3">The third parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg4">The fourth parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <param name="arg5">The fifth parameter of the <paramref name="testerDoerBody"/>.</param>
        /// <returns><c>true</c> if an instance of <typeparamref name="TResult"/> has been created; otherwise <c>false</c>.</returns>
        public static TSuccess IgnoreResult<T1, T2, T3, T4, T5, TResult, TSuccess>(TesterFunc<T1, T2, T3, T4, T5, TResult, TSuccess> testerDoerBody, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Validator.ThrowIfNull(testerDoerBody, nameof(testerDoerBody));
            TResult ignore;
            return testerDoerBody(arg1, arg2, arg3, arg4, arg5, out ignore);
        }
    }
}