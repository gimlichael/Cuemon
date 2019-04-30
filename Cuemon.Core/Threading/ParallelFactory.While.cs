using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    /// <summary>
    /// Provides a factory based way to encapsulate and re-use existing code while adding support for typically long-running parallel loops and regions.
    /// </summary>
    public static partial class ParallelFactory
    {
        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task WhileAsync(Func<bool> condition, Action worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(condition);
            var f2 = ActionFactory.Create(worker);
            await WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task WhileAsync<T>(Func<T, bool> condition, T arg, Action<T> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(condition, arg);
            var f2 = ActionFactory.Create(worker, arg);
            await WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task WhileAsync<T1, T2>(Func<T1, T2, bool> condition, T1 arg1, T2 arg2, Action<T1, T2> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(condition, arg1, arg2);
            var f2 = ActionFactory.Create(worker, arg1, arg2);
            await WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task WhileAsync<T1, T2, T3>(Func<T1, T2, T3, bool> condition, T1 arg1, T2 arg2, T3 arg3, Action<T1, T2, T3> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(condition, arg1, arg2, arg3);
            var f2 = ActionFactory.Create(worker, arg1, arg2, arg3);
            await WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task WhileAsync<T1, T2, T3, T4>(Func<T1, T2, T3, T4, bool> condition, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<T1, T2, T3, T4> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(condition, arg1, arg2, arg3, arg4);
            var f2 = ActionFactory.Create(worker, arg1, arg2, arg3, arg4);
            await WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="condition"/> and function delegate <paramref name="worker"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task WhileAsync<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, bool> condition, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<T1, T2, T3, T4, T5> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(condition, arg1, arg2, arg3, arg4, arg5);
            var f2 = ActionFactory.Create(worker, arg1, arg2, arg3, arg4, arg5);
            await WhileAsyncCore(f1, f2, setup);
        }

        private static async Task WhileAsyncCore<TCondition, TWorker>(FuncFactory<TCondition, bool> conditionFactory, ActionFactory<TWorker> workerFactory, Action<TaskFactoryOptions> setup)
            where TCondition : Template
            where TWorker : Template
        {
            var options = Patterns.Configure(setup);
            var exceptions = new ConcurrentBag<Exception>();
            for (;;)
            {
                var workChunks = options.ChunkSize;
                var queue = new List<Task>();
                while (workChunks > 1 && conditionFactory.ExecuteMethod())
                {
                    queue.Add(Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            Interlocked.Decrement(ref workChunks);
                            workerFactory.ExecuteMethod();
                        }
                        catch (Exception e)
                        {
                            exceptions.Add(e);
                        }
                    }, options.CancellationToken, options.CreationOptions, options.Scheduler));
                }
                if (queue.Count == 0) { break; }
                await Task.WhenAll(queue).ConfigureAwait(false);
                if (workChunks > 0) { break; }
            }
            if (exceptions.Count > 0) { throw new AggregateException(exceptions); }
        }
    }
}