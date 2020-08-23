using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Resilience
{
    internal sealed class AsyncFuncTransientWorker<TResult> : AsyncTransientWorker
    {
        internal AsyncFuncTransientWorker(MethodInfo delegateInfo, object[] runtimeArguments, Action<TransientOperationOptions> setup) : base(delegateInfo, runtimeArguments, setup)
        {
        }

        public async Task<TResult> ResilientFuncAsync(Func<CancellationToken, Task<TResult>> operation, CancellationToken ct)
        {
            var result = default(TResult);
            for (var attempts = 0;;)
            {
                var waitTime = Options.RetryStrategy(attempts);
                try
                {
                    ResilientTry();
                    result = await operation(ct).ConfigureAwait(false);
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
                finally
                {
                    ResilientFinally(result);
                }
            }
            ResilientThrower();
            return result;
        }
    }
}