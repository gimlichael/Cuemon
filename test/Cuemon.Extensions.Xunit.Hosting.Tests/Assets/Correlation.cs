using System;
using Cuemon.Messaging;

namespace Cuemon.Extensions.Xunit.Hosting.Assets
{
    public abstract class Correlation : ICorrelation
    {
        protected Correlation()
        {
            CorrelationId = Guid.NewGuid().ToString("N");
        }

        public string CorrelationId { get; }
    }
}