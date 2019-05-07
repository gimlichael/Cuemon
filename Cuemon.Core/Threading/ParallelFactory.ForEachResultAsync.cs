using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    public static partial class ParallelFactory
    {
        /// <summary>
        /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="source">The sequence to iterate over parallel.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> worker, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default);
            return ForEachResultCoreAsync(source, wf, setup);
        }

        /// <summary>
        /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="source">The sequence to iterate over parallel.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T, TResult>(IEnumerable<TSource> source, Func<TSource, T, TResult> worker, T arg, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg);
            return ForEachResultCoreAsync(source, wf, setup);
        }

        /// <summary>
        /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="source">The sequence to iterate over parallel.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T1, T2, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, TResult> worker, T1 arg1, T2 arg2, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg1, arg2);
            return ForEachResultCoreAsync(source, wf, setup);
        }

        /// <summary>
        /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="source">The sequence to iterate over parallel.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T1, T2, T3, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, TResult> worker, T1 arg1, T2 arg2, T3 arg3, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg1, arg2, arg3);
            return ForEachResultCoreAsync(source, wf, setup);
        }

        /// <summary>
        /// Executes a parallel foreach loop where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="source">The sequence to iterate over parallel.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T1, T2, T3, T4, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg1, arg2, arg3, arg4);
            return ForEachResultCoreAsync(source, wf, setup);
        }

        /// <summary>
        /// Executes a parallel foreach loop    
        /// </summary>
        /// <typeparam name="TSource">The type of the data in the source.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="worker" />.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the function delegate <paramref name="worker"/>.</typeparam>
        /// <param name="source">The sequence to iterate over parallel.</param>
        /// <param name="worker">The delegate that is invoked once per iteration.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="worker" />.</param>
        /// <param name="setup">The <see cref="TaskFactoryOptions" /> which may be configured.</param>
        /// <returns>A <see cref="Task" /> that represents the asynchronous operation.
        /// The task result contains an <see cref="IReadOnlyCollection{TResult}" /> where the return value of the function delegate <paramref name="worker" /> is stored in the same sequential order as <paramref name="source" />.</returns>
        public static Task<IReadOnlyCollection<TResult>> ForEachResultAsync<TSource, T1, T2, T3, T4, T5, TResult>(IEnumerable<TSource> source, Func<TSource, T1, T2, T3, T4, T5, TResult> worker, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Action<TaskFactoryOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNull(worker, nameof(worker));
            var wf = FuncFactory.Create(worker, default, arg1, arg2, arg3, arg4, arg5);
            return ForEachResultCoreAsync(source, wf, setup);
        }

        private static async Task<IReadOnlyCollection<TResult>> ForEachResultCoreAsync<TSource, TWorker, TResult>(IEnumerable<TSource> source, FuncFactory<TWorker, TResult> workerFactory, Action<TaskFactoryOptions> setup)
            where TWorker : Template<TSource>
        {
            var options = Patterns.Configure(setup);
            var exceptions = new ConcurrentBag<Exception>();
            var result = new ConcurrentDictionary<long, TResult>();
            var skip = 0;
            var sorter = 0;

            for (;;)
            {
                var workChunks = options.ChunkSize;
                var queue = new List<Task>();
                var partition = source.Skip(skip);
                foreach (var item in partition)
                {
                    var shallowWorkerFactory = workerFactory.Clone();
                    var current = sorter;
                    queue.Add(Task.Factory.StartNew(element =>
                    {
                        try
                        {
                            Interlocked.Increment(ref skip);
                            shallowWorkerFactory.GenericArguments.Arg1 = (TSource)element;
                            var presult = shallowWorkerFactory.ExecuteMethod();
                            result.TryAdd(current, presult);
                        }
                        catch (Exception e)
                        {
                            exceptions.Add(e);
                        }
                    }, item, options.CancellationToken, options.CreationOptions, options.Scheduler));
                    
                    workChunks--;
                    sorter++;
                    
                    if (workChunks == 0) { break; }
                }
                if (queue.Count == 0) { break; }
                await Task.WhenAll(queue).ConfigureAwait(false);
                if (workChunks > 1) { break; }
            }
            if (exceptions.Count > 0) { throw new AggregateException(exceptions); }
            return new ReadOnlyCollection<TResult>(result.Values.ToList());
        }
    }
}