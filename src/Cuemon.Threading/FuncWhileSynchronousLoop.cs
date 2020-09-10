using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cuemon.Threading
{
    internal sealed class FuncWhileSynchronousLoop<TReader, TSource, TResult> : WhileSynchronousLoop<TReader, TSource>
    {
        public FuncWhileSynchronousLoop(ForwardIterator<TReader, TSource> iterator, Action<AsyncTaskFactoryOptions> setup) : base(iterator, setup)
        {
        }

        private ConcurrentDictionary<long, TResult> Result { get; } = new ConcurrentDictionary<long, TResult>();

        protected override void FillWorkQueueWorkerFactory<TWorker>(TemplateFactory<TWorker> worker, long sorter)
        {
            if (worker is FuncFactory<TWorker, TResult> wf)
            {
                var presult = wf.ExecuteMethod();
                Result.TryAdd(sorter, presult);
            }
        }

        public IReadOnlyCollection<TResult> GetResult<TWorker>(TemplateFactory<TWorker> worker) where TWorker : Template<TSource>
        {
            PrepareExecution(worker);
            return new ReadOnlyCollection<TResult>(Result.Values.ToList());
        }
    }
}