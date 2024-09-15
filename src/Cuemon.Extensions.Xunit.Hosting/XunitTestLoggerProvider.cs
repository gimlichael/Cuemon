using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{
    internal sealed class XunitTestLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, XunitTestLogger> _loggers = new();
        private readonly ITestOutputHelperAccessor _accessor;
        private readonly ITestOutputHelper _output;

        public XunitTestLoggerProvider(ITestOutputHelper output)
        {
            _output = output;
        }

        public XunitTestLoggerProvider(ITestOutputHelperAccessor accessor)
        {
            _accessor = accessor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, _ => _accessor != null
                ? new XunitTestLogger(_accessor)
                : new XunitTestLogger(_output));
        }

        public void Dispose()
        {
        }
    }
}
