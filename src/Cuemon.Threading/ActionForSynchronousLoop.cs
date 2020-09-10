using System;

namespace Cuemon.Threading
{
    internal sealed class ActionForSynchronousLoop<TSource> : ForSynchronousLoop<TSource> where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
    {
        public ActionForSynchronousLoop(ForLoopRuleset<TSource> rules, Action<AsyncTaskFactoryOptions> setup) : base(rules, setup)
        {
        }

        protected override void FillWorkQueueWorkerFactory<TWorker>(TemplateFactory<TWorker> worker)
        {
            if (worker is ActionFactory<TWorker> wf) { wf.ExecuteMethod(); }
        }
    }
}