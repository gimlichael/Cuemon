using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    internal abstract class ForSynchronousLoop<TSource> : SynchronousLoop<TSource> where TSource : struct, IComparable<TSource>, IEquatable<TSource>, IConvertible
    {
        protected ForSynchronousLoop(ForLoopRuleset<TSource> rules, Action<AsyncTaskFactoryOptions> setup) : base(setup)
        {
            Rules = rules;
            From = rules.From;
            WhileCondition = () => true;
        }

        protected TSource From { get; set; }

        protected ForLoopRuleset<TSource> Rules { get; }

        protected TSource Processed { get; set; }

        protected int WorkChunks { get; set; }

        protected sealed override void FillWorkQueue<TWorker>(TemplateFactory<TWorker> worker, IList<Func<Task>> queue)
        {
            for (var i = From; Rules.Condition(i, Rules.Relation, Rules.To); i = Rules.Iterator(i, Rules.Assignment, Rules.Step))
            {
                var shallowWorkerFactory = worker.Clone();
                shallowWorkerFactory.GenericArguments.Arg1 = i;
                queue.Add(() => Task.Factory.StartNew(swf =>
                {
                    try
                    {
                        FillWorkQueueWorkerFactory(swf as TemplateFactory<TWorker>);
                    }
                    catch (Exception e)
                    {
                        Exceptions.Add(e);
                    }
                }, shallowWorkerFactory, Options.CancellationToken, Options.CreationOptions, Options.Scheduler));

                Processed = i;
                WorkChunks--;

                if (WorkChunks == 0) { break; }
            }
            From = Calculator.Calculate(Processed, Rules.Assignment, Rules.Step);
        }

        protected abstract void FillWorkQueueWorkerFactory<TWorker>(TemplateFactory<TWorker> worker) where TWorker : Template<TSource>;

        protected sealed override void OnWhileExecutingBeforeFillWorkQueue()
        {
            WorkChunks = Options.PartitionSize;
        }
    }
}