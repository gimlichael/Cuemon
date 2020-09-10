using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Cuemon.Threading
{
    internal sealed class FuncForSynchronousLoop<TSource, TResult> : ForSynchronousLoop<TSource> where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
    {
        public FuncForSynchronousLoop(ForLoopRuleset<TSource> rules, Action<AsyncTaskFactoryOptions> setup) : base(rules, setup)
        {
        }

        private ConcurrentDictionary<TSource, TResult> Result { get; } = new ConcurrentDictionary<TSource, TResult>();


        protected override void FillWorkQueueWorkerFactory<TWorker>(TemplateFactory<TWorker> worker)
        {
            if (worker is FuncFactory<TWorker, TResult> wf)
            {
                var presult = wf.ExecuteMethod();
                Result.TryAdd(wf.GenericArguments.Arg1, presult);
            }
        }

        public IReadOnlyCollection<TResult> GetResult<TWorker>(TemplateFactory<TWorker> worker) where TWorker : Template<TSource>
        {
            PrepareExecution(worker);
            return new ReadOnlyCollection<TResult>(Result.Values.ToList());
        }
    }
}