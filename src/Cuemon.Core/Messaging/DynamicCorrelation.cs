using System;

namespace Cuemon.Messaging
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="ICorrelation"/> implementation.
    /// </summary>
    public static class DynamicCorrelation
    {
        /// <summary>
        /// Creates a dynamic implementation of <seealso cref="ICorrelation"/>.
        /// </summary>
        /// <param name="correlationIdProvider">The function delegate which provides a unique identifier of a request or message.</param>
        /// <returns>A dynamic <see cref="ICorrelation"/> implementation.</returns>
        public static ICorrelation Create(Func<string> correlationIdProvider)
        {
            Validator.ThrowIfNull(correlationIdProvider);
            return new DynamicCorrelationCore(correlationIdProvider.Invoke());
        }

        /// <summary>
        /// Creates a dynamic implementation of <seealso cref="ICorrelation"/>.
        /// </summary>
        /// <param name="correlationId">The unique identifier of a request or message.</param>
        /// <returns>A dynamic <see cref="ICorrelation"/> implementation.</returns>
        public static ICorrelation Create(string correlationId)
        {
            Validator.ThrowIfNullOrWhitespace(correlationId);
            return new DynamicCorrelationCore(correlationId);
        }
    }

    internal class DynamicCorrelationCore : ICorrelation
    {
        public DynamicCorrelationCore(string correlationId)
        {
            CorrelationId = correlationId;
        }

        public string CorrelationId { get; }
    }
}