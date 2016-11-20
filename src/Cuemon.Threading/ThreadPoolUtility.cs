using System;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    /// <summary>
    /// Provide ways to work more efficient with <see cref="TaskFactory.StartNew(System.Action)"/> related tasks.
    /// </summary>
    public static class ThreadPoolUtility
    {
        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork(Action method)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T>(Action<T> method, T arg)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2>(Action<T1, T2> method, T1 arg1, T2 arg2)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2, T3>(Action<T1, T2, T3> method, T1 arg1, T2 arg2, T3 arg3)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2, arg3);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2, T3, T4>(Action<T1, T2, T3, T4> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4, arg5);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            QueueUserWorkItemCore(factory);
        }

        /// <summary>
        /// Queues the specified <paramref name="method"/> for execution. The <paramref name="method"/> executes when a thread pool thread becomes available.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter of the delegate <paramref name="method" />.</typeparam>
        /// <param name="method">The delegate that is being invoked when a thread pool thread becomes available.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg6">The sixth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg7">The seventh parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg8">The eighth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg9">The ninth parameter of the delegate <paramref name="method" />.</param>
        /// <param name="arg10">The tenth parameter of the delegate <paramref name="method" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static void QueueWork<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> method, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            Validator.ThrowIfNull(method, nameof(method));
            var factory = ActionFactory.Create(method, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            QueueUserWorkItemCore(factory);
        }

        private static void QueueUserWorkItemCore<TTuple>(ActionFactory<TTuple> factory) where TTuple : Template
        {
            if (factory == null) { return; }
            Task.Factory.StartNew(factory.ExecuteMethod);
        }
    }
}