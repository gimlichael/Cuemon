using System;

namespace Cuemon.Messaging
{
    /// <summary>
    /// Provides a factory based way to create and wrap an <see cref="IRequest"/> implementation.
    /// </summary>
    public static class DynamicRequest
    {
        /// <summary>
        /// Creates a dynamic implementation of <seealso cref="IRequest"/>.
        /// </summary>
        /// <param name="requestIdProvider">The function delegate which provides a unique identifier of a request or message.</param>
        /// <returns>A dynamic <see cref="IRequest"/> implementation.</returns>
        public static IRequest Create(Func<string> requestIdProvider)
        {
            Validator.ThrowIfNull(requestIdProvider, nameof(requestIdProvider));
            return new DynamicRequestCore(requestIdProvider.Invoke());
        }

        /// <summary>
        /// Creates a dynamic implementation of <seealso cref="IRequest"/>.
        /// </summary>
        /// <param name="requestId">The unique identifier of a request or message.</param>
        /// <returns>A dynamic <see cref="IRequest"/> implementation.</returns>
        public static IRequest Create(string requestId)
        {
            Validator.ThrowIfNullOrWhitespace(requestId, nameof(requestId));
            return new DynamicRequestCore(requestId);
        }
    }

    internal class DynamicRequestCore : IRequest
    {
        public DynamicRequestCore(string requestId)
        {
            RequestId = requestId;
        }

        public string RequestId { get; }
    }
}