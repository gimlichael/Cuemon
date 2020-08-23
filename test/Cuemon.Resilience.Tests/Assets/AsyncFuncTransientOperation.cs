﻿using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Resilience.Assets
{
    public static class AsyncFuncTransientOperation
    {
        public static Task<string> MethodThatReturnsOkStringAsync(Guid id, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            retryTracker[id] += 1;
            return Task.FromResult("OK");
        }

        public static Task<string> FailUntilExpectedRetryAttemptsIsReachedAsync(Guid id, int expectedRetryAttempts, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            retryTracker[id] += 1;
            if (retryTracker[id] < expectedRetryAttempts) { throw new HttpRequestException(); }
            return Task.FromResult("OK");
        }

        public static Task<string> TriggerTransientFaultExceptionAsync(Guid id, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            retryTracker[id] += 1;
            throw new HttpRequestException();
        }

        public static Task<string> TriggerLatencyExceptionAsync(Guid id, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            Thread.Sleep(250);
            retryTracker[id] += 1;
            throw new HttpRequestException();
        }

        public static Task<string> FailWithNonTransientFaultExceptionAsync(Guid id, int expectedRetryAttempts, ConcurrentDictionary<Guid, int> retryTracker, CancellationToken ct)
        {
            retryTracker[id] += 1;
            if (retryTracker[id] < expectedRetryAttempts) { throw new HttpRequestException(); }
            throw new InvalidOperationException();
        }
    }
}