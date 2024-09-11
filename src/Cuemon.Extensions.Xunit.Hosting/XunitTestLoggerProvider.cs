using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Cuemon.Extensions.Xunit.Hosting
{
    internal class XunitTestLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, XunitTestLogger> _loggers = new();
        private readonly ITestOutputHelperAccessor _accessor;

        public XunitTestLoggerProvider(ITestOutputHelperAccessor accessor)
        {
            _accessor = accessor;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, s => new XunitTestLogger(_accessor));
        }

        public void Dispose()
        {
        }
    }
}
