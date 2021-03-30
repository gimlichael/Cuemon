using System;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Newtonsoft.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Newtonsoft.Json.Serialization
{
    public class ContractResolverExtensionsTest : Test
    {
        public ContractResolverExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ResolveNamingStrategyOrDefault_ShouldResolveDefaultNamingStrategy()
        {
            var sut1 = TimeSpan.MaxValue;
            var sut2 = new JsonFormatter(o => o.Settings.ContractResolver = new DefaultContractResolver());
            var json = sut2.Serialize(sut1).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.IsType<DefaultNamingStrategy>(sut2.Options.Settings.ContractResolver.ResolveNamingStrategyOrDefault());
            Assert.Equal(@"{
  ""Ticks"": 9223372036854775807,
  ""Days"": 10675199,
  ""Hours"": 2,
  ""Minutes"": 48,
  ""Seconds"": 5,
  ""TotalDays"": 10675199.116730064,
  ""TotalHours"": 256204778.80152154,
  ""TotalMilliseconds"": 922337203685477.0,
  ""TotalMinutes"": 15372286728.091293,
  ""TotalSeconds"": 922337203685.4775
}", json);
        }

        [Fact]
        public void ResolveNamingStrategyOrDefault_ShouldResolveCamelCaseNamingStrategy()
        {
            var sut1 = TimeSpan.MaxValue;
            var sut2 = new JsonFormatter();
            var json = sut2.Serialize(sut1).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.IsType<CamelCaseNamingStrategy>(sut2.Options.Settings.ContractResolver.ResolveNamingStrategyOrDefault());

            Assert.Equal(@"{
  ""ticks"": 9223372036854775807,
  ""days"": 10675199,
  ""hours"": 2,
  ""minutes"": 48,
  ""seconds"": 5,
  ""totalDays"": 10675199.116730064,
  ""totalHours"": 256204778.80152154,
  ""totalMilliseconds"": 922337203685477.0,
  ""totalMinutes"": 15372286728.091293,
  ""totalSeconds"": 922337203685.4775
}", json);
        }

        [Fact]
        public void ResolveNamingStrategyOrDefault_ShouldResolveSnakeCaseNamingStrategy()
        {
            var sut1 = TimeSpan.MaxValue;
            var sut2 = new JsonFormatter(o => o.Settings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() });
            var json = sut2.Serialize(sut1).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.IsType<SnakeCaseNamingStrategy>(sut2.Options.Settings.ContractResolver.ResolveNamingStrategyOrDefault());
            Assert.Equal(@"{
  ""ticks"": 9223372036854775807,
  ""days"": 10675199,
  ""hours"": 2,
  ""minutes"": 48,
  ""seconds"": 5,
  ""total_days"": 10675199.116730064,
  ""total_hours"": 256204778.80152154,
  ""total_milliseconds"": 922337203685477.0,
  ""total_minutes"": 15372286728.091293,
  ""total_seconds"": 922337203685.4775
}", json);
        }

        [Fact]
        public void ResolveNamingStrategyOrDefault_ShouldResolveKebabCaseNamingStrategy()
        {
            var sut1 = TimeSpan.MaxValue;
            var sut2 = new JsonFormatter(o => o.Settings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new KebabCaseNamingStrategy() });
            var json = sut2.Serialize(sut1).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.IsType<KebabCaseNamingStrategy>(sut2.Options.Settings.ContractResolver.ResolveNamingStrategyOrDefault());
            Assert.Equal(@"{
  ""ticks"": 9223372036854775807,
  ""days"": 10675199,
  ""hours"": 2,
  ""minutes"": 48,
  ""seconds"": 5,
  ""total-days"": 10675199.116730064,
  ""total-hours"": 256204778.80152154,
  ""total-milliseconds"": 922337203685477.0,
  ""total-minutes"": 15372286728.091293,
  ""total-seconds"": 922337203685.4775
}", json);
        }
    }
}