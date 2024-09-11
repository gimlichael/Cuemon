using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Cuemon.Extensions.Xunit.Hosting
{
    internal class XunitTestLogger : InMemoryTestStore<XunitTestLoggerEntry>, ILogger, IDisposable
    {
        private readonly ITestOutputHelperAccessor _accessor;

        public XunitTestLogger(ITestOutputHelperAccessor accessor)
        {
            _accessor = accessor;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var builder = new StringBuilder($"{logLevel}: {formatter(state, exception)}");
            if (exception != null) { builder.AppendLine().Append(exception).AppendLine(); }

            var message = builder.ToString();
            _accessor.TestOutput.WriteLine(message);
            Add(new XunitTestLoggerEntry(logLevel, eventId, message));
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        public IDisposable BeginScope<TState>(TState state) where TState : notnull
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
