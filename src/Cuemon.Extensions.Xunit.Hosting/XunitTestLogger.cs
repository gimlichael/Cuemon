using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{
    internal sealed class XunitTestLogger : InMemoryTestStore<XunitTestLoggerEntry>, ILogger, IDisposable
    {
        private readonly ITestOutputHelperAccessor _accessor;
        private readonly ITestOutputHelper _output;

        public XunitTestLogger(ITestOutputHelper output)
        {
            _output = output;
        }

        public XunitTestLogger(ITestOutputHelperAccessor accessor)
        {
            _accessor = accessor;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var builder = new StringBuilder($"{logLevel}: {formatter(state, exception)}");
            if (exception != null) { builder.AppendLine().Append(exception).AppendLine(); }

            var message = builder.ToString();
            if (_accessor != null)
            {
                _accessor.TestOutput.WriteLine(message);
            }
            else
            {
                _output.WriteLine(message);
            }
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
