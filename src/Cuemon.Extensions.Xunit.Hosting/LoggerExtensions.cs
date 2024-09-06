using System;
using System.Collections;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Cuemon.Extensions.Xunit.Hosting
{
    /// <summary>
    /// Extension methods for the <see cref="ILogger{TCategoryName}"/> interface.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Returns the associated <see cref="ITestStore{T}"/> that is provided when settings up services from <see cref="ServiceCollectionExtensions.AddXunitTestLogging"/>.
        /// </summary>
        /// <param name="logger">The <see cref="ILogger{TCategoryName}"/> from which to retrieve the <see cref="ITestStore{T}"/>.</param>
        /// <returns>Returns an implementation of <see cref="ITestStore{T}"/> with all logged entries expressed as <see cref="XunitTestLoggerEntry"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="logger"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="logger"/> does not contain a test store.
        /// </exception>
        public static ITestStore<XunitTestLoggerEntry> GetTestStore<T>(this ILogger<T> logger)
        {
            Validator.ThrowIfNull(logger);
            var loggerType = logger.GetType();
            var internalLogger = Decorator.Enclose(loggerType).GetAllFields().SingleOrDefault(fi => fi.Name == "_logger")?.GetValue(logger);
            if (internalLogger != null)
            {
                var internalLoggerType = internalLogger.GetType();
                var internalLoggers = Decorator.Enclose(internalLoggerType).GetAllProperties().SingleOrDefault(pi => pi.Name == "Loggers")?.GetValue(internalLogger);
                if (internalLoggers != null)
                {
                    foreach (var loggerInformation in (IEnumerable)internalLoggers)
                    {
                        var loggerInformationType = loggerInformation.GetType();
                        var providerType = loggerInformationType.GetProperty("ProviderType")?.GetValue(loggerInformation) as Type;
                        if (providerType == typeof(XunitTestLoggerProvider))
                        {
                            return loggerInformationType.GetProperty("Logger")?.GetValue(loggerInformation) as InMemoryTestStore<XunitTestLoggerEntry>;
                        }
                    }
                }
            }
            throw new ArgumentException($"Logger does not contain a test store; did you remember to call {nameof(ServiceCollectionExtensions.AddXunitTestLogging)} before calling this method?", nameof(logger));
        }
    }
}
