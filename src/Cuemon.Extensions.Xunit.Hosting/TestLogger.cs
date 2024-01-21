using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{
	internal class TestLogger : InMemoryTestStore<TestLoggerEntry>, ILogger, IDisposable
	{
		private readonly ITestOutputHelper _output;

		public TestLogger(ITestOutputHelper output)
		{
			_output = output;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var message = $"{logLevel}: {formatter(state, exception)}";
			_output.WriteLine(message);
			Add(new TestLoggerEntry(logLevel, eventId, message));
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
