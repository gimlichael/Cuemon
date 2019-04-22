﻿using System;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make common delegate operations easier to work with.
    /// </summary>
    public static class DelegateUtility
    {
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
    }
}