using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{
	internal class TestLoggerProvider : ILoggerProvider
	{
		private readonly ConcurrentDictionary<string, TestLogger> _loggers = new();
		private readonly ITestOutputHelper _output;

		public TestLoggerProvider(ITestOutputHelper output)
		{
			_output = output;
		}

		public ILogger CreateLogger(string categoryName)
		{
			return _loggers.GetOrAdd(categoryName, s => new TestLogger(_output));
		}

		public void Dispose()
		{
		}
	}
}
