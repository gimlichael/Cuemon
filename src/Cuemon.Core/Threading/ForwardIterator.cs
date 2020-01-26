using System;
using System.Threading.Tasks;

namespace Cuemon.Threading
{
    internal class ForwardIterator<TReader, TElement>
    {
        internal ForwardIterator(TReader reader, Func<Task<bool>> condition, Func<TReader, TElement> provider)
        {
            Reader = reader;
            Condition = condition;
            Provider = provider;
        }

        private TReader Reader { get; }

        private Func<Task<bool>> Condition { get; }

        private Func<TReader, TElement> Provider { get; }

        public TElement Current { get; private set; }

        public async Task<bool> ReadAsync()
        {
            if (await Condition())
            {
                Current = Provider(Reader);
                return true;
            }
            Current = default;
            return false;
        }
    }
}