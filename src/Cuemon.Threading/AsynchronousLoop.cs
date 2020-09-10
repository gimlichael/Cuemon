using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    internal abstract class AsynchronousLoop : Loop<AsyncWorkloadOptions>
    {
        protected AsynchronousLoop(Action<AsyncWorkloadOptions> setup) : base(setup)
        {
        }

        protected Task WhileExecuting()
        {
            return null;
        }

        protected Task Process(IList<Task> queue)
        {
            return queue.Count == 0 ? Task.CompletedTask : Task.WhenAll(queue);
        }
    }
}