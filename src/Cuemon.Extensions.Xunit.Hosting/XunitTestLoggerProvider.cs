using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{
	internal class XunitTestLoggerProvider : ILoggerProvider
	{
		private readonly ConcurrentDictionary<string, XunitTestLogger> _loggers = new();
		private readonly ITestOutputHelper _output;

		public XunitTestLoggerProvider(ITestOutputHelper output)
		{
			_output = output;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return _loggers.GetOrAdd(categoryName, s => new XunitTestLogger(_output));
		}

		public void Dispose()
		{
		}
	}
}
