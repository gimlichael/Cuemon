using System.Threading.Tasks;

namespace Cuemon.Threading
{
    internal class ForwardIterator<TReader, TResult, TCondition, TProvider> 
        where TCondition : Template
        where TProvider : Template<TReader>
    {
        internal ForwardIterator(FuncFactory<TCondition, Task<bool>> conditionFactory, FuncFactory<TProvider, TResult> providerFactory)
        {
            ConditionFactory = conditionFactory;
            ProviderFactory = providerFactory;
        }

        private FuncFactory<TCondition, Task<bool>> ConditionFactory { get; }

        private FuncFactory<TProvider, TResult> ProviderFactory { get; }

        public TResult Current { get; private set; }

        public async Task<bool> ReadAsync()
        {
            if (await ConditionFactory.ExecuteMethod())
            {
                Current = ProviderFactory.ExecuteMethod();
                return true;
            }
            Current = default;
            return false;
        }
    }
}