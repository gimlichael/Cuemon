using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{
	public class ServiceCollectionExtensions : Test
	{
		public ServiceCollectionExtensions(ITestOutputHelper output) : base(output)
		{
		}

		[Fact]
		public void AddTestOutputLogging_ShouldAddTestOutputLogging()
		{
			var services = new ServiceCollection();
			services.AddXunitTestLogging(TestOutput);

			var provider = services.BuildServiceProvider();

			var logger = provider.GetRequiredService<ILogger<ServiceCollectionExtensions>>();
			var loggerStore = logger.GetTestStore();

			logger.LogCritical("SUT");
			logger.LogTrace("SUT");
			logger.LogDebug("SUT");
			logger.LogError("SUT");
			logger.LogInformation("SUT");
			logger.LogWarning("SUT");

			Assert.NotNull(logger);
			Assert.NotNull(loggerStore);
			Assert.Equal(6, loggerStore.Query().Count());
			Assert.Collection(loggerStore.Query(),
				entry => Assert.Equal("Critical: SUT", entry.ToString()),
				entry => Assert.Equal("Trace: SUT", entry.ToString()),
				entry => Assert.Equal("Debug: SUT", entry.ToString()),
				entry => Assert.Equal("Error: SUT", entry.ToString()),
				entry => Assert.Equal("Information: SUT", entry.ToString()),
				entry => Assert.Equal("Warning: SUT", entry.ToString()));
		}
	}
}
