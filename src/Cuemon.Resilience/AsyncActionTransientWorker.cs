using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Resilience
{
    internal sealed class AsyncActionTransientWorker : AsyncTransientWorker
    {
        internal AsyncActionTransientWorker(MethodInfo delegateInfo, object[] runtimeArguments, Action<TransientOperationOptions> setup) : base(delegateInfo, runtimeArguments, setup)
        {
        }

        public async Task ResilientActionAsync(Func<CancellationToken, Task> operation, CancellationToken ct)
        {
            for (var attempts = 0;;)
            {
                var waitTime = Options.RetryStrategy(attempts);
                try
                {
                    ResilientTry();
                    await operation(ct).ConfigureAwait(false);
                    break;
                }
                catch (Exception ex)
                {
                    var sleep = waitTime;
                    if (await ResilientCatchAsync(attempts, waitTime, ex, () => Task.Delay(sleep, ct)).ConfigureAwait(false))
                    {
                        break;
                    }
                    attempts++;
                }
            }
            ResilientThrower();
        }
    }
}