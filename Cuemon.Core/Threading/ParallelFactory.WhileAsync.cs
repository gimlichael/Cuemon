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
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TResult">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition" /> evaluates <c>true</c>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TResult>(TReader reader, Func<Task<bool>> condition, Func<TReader, TResult> provider, Action<TResult> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(() => new ForwardIterator<TReader, TResult, Template<TReader>>(condition, FuncFactory.Create(provider, reader)));
            var f2 = ActionFactory.Create(worker, default);
            return WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TResult">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TResult, T>(TReader reader, Func<Task<bool>> condition, Func<TReader, T, TResult> provider, Action<TResult, T> worker, T arg, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(() => new ForwardIterator<TReader, TResult, Template<TReader, T>>(condition, FuncFactory.Create(provider, reader, arg)));
            var f2 = ActionFactory.Create(worker, default, arg);
            return WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TResult">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TResult, T1, T2>(TReader reader, Func<Task<bool>> condition, Func<TReader, T1, T2, TResult> provider, Action<TResult, T1, T2> worker, T1 arg1, T2 arg2, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(() => new ForwardIterator<TReader, TResult, Template<TReader, T1, T2>>(condition, FuncFactory.Create(provider, reader, arg1, arg2)));
            var f2 = ActionFactory.Create(worker, default, arg1, arg2);
            return WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TResult">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static async Task WhileAsync<TReader, TResult, T1, T2, T3>(TReader reader, Func<Task<bool>> condition, Func<TReader, T1, T2, T3, TResult> provider, Action<TResult, T1, T2, T3> worker, T1 arg1, T2 arg2, T3 arg3, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(() => new ForwardIterator<TReader, TResult, Template<TReader, T1, T2, T3>>(condition, FuncFactory.Create(provider, reader, arg1, arg2, arg3)));
            var f2 = ActionFactory.Create(worker, default, arg1, arg2, arg3);
            await WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TResult">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TResult, T1, T2, T3, T4>(TReader reader, Func<Task<bool>> condition, Func<TReader, T1, T2, T3, T4, TResult> provider, Action<TResult, T1, T2, T3, T4> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(() => new ForwardIterator<TReader, TResult, Template<TReader, T1, T2, T3, T4>>(condition, FuncFactory.Create(provider, reader, arg1, arg2, arg3, arg4)));
            var f2 = ActionFactory.Create(worker, default, arg1, arg2, arg3, arg4);
            return WhileAsyncCore(f1, f2, setup);
        }

        /// <summary>
        /// Provides a generic way of executing a parallel while-loop while providing ways to encapsulate and re-use existing code.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TResult">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter for the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while-loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="provider"/> and delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TResult, T1, T2, T3, T4, T5>(TReader reader, Func<Task<bool>> condition, Func<TReader, T1, T2, T3, T4, T5, TResult> provider, Action<TResult, T1, T2, T3, T4, T5> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var f1 = FuncFactory.Create(() => new ForwardIterator<TReader, TResult, Template<TReader, T1, T2, T3, T4, T5>>(condition, FuncFactory.Create(provider, reader, arg1, arg2, arg3, arg4, arg5)));
            var f2 = ActionFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5);
            return WhileAsyncCore(f1, f2, setup);
        }

        private static async Task WhileAsyncCore<TReader, TResult, TIterator, TProvider, TWorker>(FuncFactory<TIterator, ForwardIterator<TReader, TResult, TProvider>> iteratorFactory, ActionFactory<TWorker> workerFactory, Action<TaskFactoryOptions> setup)
            where TIterator : Template
            where TProvider : Template<TReader>
            where TWorker : Template<TResult>
        {
            var options = Patterns.Configure(setup);
            var exceptions = new ConcurrentBag<Exception>();
            var readForward = true;
            for (;;)
            {
                var workChunks = options.ChunkSize;
                var queue = new List<Task>();
                var iterator = iteratorFactory.ExecuteMethod();
                while (workChunks > 1 && readForward)
                {
                    readForward = await iterator.ReadAsync();
                    if (!readForward) { break; }
                    var shallowWorkerFactory = workerFactory.Clone();
                    queue.Add(Task.Factory.StartNew(result =>
                    {
                        try
                        {
                            Interlocked.Decrement(ref workChunks);
                            shallowWorkerFactory.GenericArguments.Arg1 = (TResult)result;
                            shallowWorkerFactory.ExecuteMethod();
                        }
                        catch (Exception e)
                        {
                            exceptions.Add(e);
                        }
                    }, iterator.Current, options.CancellationToken, options.CreationOptions, options.Scheduler));
                }
                if (queue.Count == 0) { break; }
                await Task.WhenAll(queue).ConfigureAwait(false);
                if (workChunks > 1) { break; }
            }
            if (exceptions.Count > 0) { throw new AggregateException(exceptions); }
        }
    }
}