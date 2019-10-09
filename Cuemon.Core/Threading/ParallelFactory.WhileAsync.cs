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
        /// Executes a parallel while loop.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition" /> evaluates <c>true</c>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TElement>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Action<TElement> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default);
            return WhileCoreAsync(new ForwardIterator<TReader, TElement>(reader, condition, provider), wf, setup);
        }

        /// <summary>
        /// Executes a parallel while loop.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T">The type of the parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg">The parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TElement, T>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Action<TElement, T> worker, T arg, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg);
            return WhileCoreAsync(new ForwardIterator<TReader, TElement>(reader, condition, provider), wf, setup);
        }

        /// <summary>
        /// Executes a parallel while loop.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TElement, T1, T2>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Action<TElement, T1, T2> worker, T1 arg1, T2 arg2, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg1, arg2);
            return WhileCoreAsync(new ForwardIterator<TReader, TElement>(reader, condition, provider), wf, setup);
        }

        /// <summary>
        /// Executes a parallel while loop.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TElement, T1, T2, T3>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Action<TElement, T1, T2, T3> worker, T1 arg1, T2 arg2, T3 arg3, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg1, arg2, arg3);
            return WhileCoreAsync(new ForwardIterator<TReader, TElement>(reader, condition, provider), wf, setup);
        }

        /// <summary>
        /// Executes a parallel while loop.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TElement, T1, T2, T3, T4>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Action<TElement, T1, T2, T3, T4> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg1, arg2, arg3, arg4);
            return WhileCoreAsync(new ForwardIterator<TReader, TElement>(reader, condition, provider), wf, setup);
        }

        /// <summary>
        /// Executes a parallel while loop.
        /// </summary>
        /// <typeparam name="TReader">The type of the <paramref name="reader"/> that provides forward-only access to data.</typeparam>
        /// <typeparam name="TElement">The type of the result provided by <paramref name="reader"/>.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the delegate <paramref name="worker"/>.</typeparam>
        /// <param name="reader">The reader that provides forward-only access to data.</param>
        /// <param name="condition">The function delegate that is responsible for the while loop condition.</param>
        /// <param name="provider">The function delegate that provides data from the specified <paramref name="reader"/>.</param>
        /// <param name="worker">The delegate that will perform work while <paramref name="condition"/> evaluates <c>true</c>.</param>
        /// <param name="arg1">The first parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg2">The second parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg3">The third parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg4">The fourth parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="arg5">The fifth parameter of the delegate <paramref name="worker"/>.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
        public static Task WhileAsync<TReader, TElement, T1, T2, T3, T4, T5>(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider, Action<TElement, T1, T2, T3, T4, T5> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(condition, nameof(condition));
            Validator.ThrowIfNull(provider, nameof(provider));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = ActionFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5);
            return WhileCoreAsync(new ForwardIterator<TReader, TElement>(reader, condition, provider), wf, setup);
        }

        private static async Task WhileCoreAsync<TReader, TElement, TWorker>(ForwardIterator<TReader, TElement> iterator, ActionFactory<TWorker> workerFactory, Action<TaskFactoryOptions> setup)
            where TWorker : Template<TElement>
        {
            var options = Patterns.Configure(setup);
            var exceptions = new ConcurrentBag<Exception>();
            var readForward = true;

            for (;;)
            {
                var workChunks = options.PartitionSize;
                var queue = new List<Task>();
                while (workChunks > 1 && readForward)
                {
                    readForward = await iterator.ReadAsync();
                    if (!readForward) { break; }
                    var shallowWorkerFactory = workerFactory.Clone();
                    queue.Add(Task.Factory.StartNew(element =>
                    {
                        try
                        {
                            Interlocked.Decrement(ref workChunks);
                            shallowWorkerFactory.GenericArguments.Arg1 = (TElement)element;
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