using System;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    internal class ForwardIterator<TReader, TResult, TProvider>
        where TProvider : Template<TReader>
    {
        internal ForwardIterator(Func<Task<bool>> condition, FuncFactory<TProvider, TResult> providerFactory)
        {
            Condition = condition;
            ProviderFactory = providerFactory;
        }

        private Func<Task<bool>> Condition { get; }

        private FuncFactory<TProvider, TResult> ProviderFactory { get; }

        public TResult Current { get; private set; }

        public async Task<bool> ReadAsync()
        {
            if (await Condition())
            {
                Current = ProviderFactory.ExecuteMethod();
                return true;
            }
            Current = default;
            return false;
        }
    }
}