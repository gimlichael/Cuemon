using System;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Newtonsoft.Json.Serialization
{
    public class ContractResolverExtensionsTest : Test
    {
        private readonly JsonConverter _tsConverter = DynamicJsonConverter.Create<TimeSpan>((writer, value, serializer) =>
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Ticks", serializer);
            writer.WriteValue(value.Ticks);
            writer.WritePropertyName("Days", serializer);
            writer.WriteValue(value.Days);
            writer.WritePropertyName("Hours", serializer);
            writer.WriteValue(value.Hours);
            writer.WritePropertyName("Minutes", serializer);
            writer.WriteValue(value.Minutes);
            writer.WritePropertyName("Seconds", serializer);
            writer.WriteValue(value.Seconds);
            writer.WritePropertyName("TotalDays", serializer);
            writer.WriteValue(value.TotalDays);
            writer.WritePropertyName("TotalHours", serializer);
            writer.WriteValue(value.TotalHours);
            writer.WritePropertyName("TotalMilliseconds", serializer);
            writer.WriteValue(value.TotalMilliseconds);
            writer.WritePropertyName("TotalMinutes", serializer);
            writer.WriteValue(value.TotalMinutes);
            writer.WritePropertyName("TotalSeconds", serializer);
            writer.WriteValue(value.TotalSeconds);
            writer.WriteEndObject();
        });

        public ContractResolverExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void ResolveNamingStrategyOrDefault_ShouldResolveDefaultNamingStrategy()
        {
            var sut1 = TimeSpan.MaxValue;
            var sut2 = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.Converters.Add(_tsConverter);
                o.Settings.ContractResolver = new DefaultContractResolver();
            });
            var json = sut2.Serialize(sut1).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.IsType<DefaultNamingStrategy>(sut2.Options.Settings.ContractResolver.ResolveNamingStrategyOrDefault());
#if NET48_OR_GREATER
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
  ""TotalSeconds"": 922337203685.47754
}".ReplaceLineEndings(), json);
#else
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
}".ReplaceLineEndings(), json);
#endif
        }

        [Fact]
        public void ResolveNamingStrategyOrDefault_ShouldResolveCamelCaseNamingStrategy()
        {
            var sut1 = TimeSpan.MaxValue;
            var sut2 = new NewtonsoftJsonFormatter(o => o.Settings.Converters.Add(_tsConverter));
            var json = sut2.Serialize(sut1).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.IsType<CamelCaseNamingStrategy>(sut2.Options.Settings.ContractResolver.ResolveNamingStrategyOrDefault());

#if NET48_OR_GREATER
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
  ""totalSeconds"": 922337203685.47754
}".ReplaceLineEndings(), json);
#else
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
}".ReplaceLineEndings(), json);
#endif
        }

        [Fact]
        public void ResolveNamingStrategyOrDefault_ShouldResolveSnakeCaseNamingStrategy()
        {
            var sut1 = TimeSpan.MaxValue;
            var sut2 = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.Converters.Add(_tsConverter);
                o.Settings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new SnakeCaseNamingStrategy() };
            });
            var json = sut2.Serialize(sut1).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.IsType<SnakeCaseNamingStrategy>(sut2.Options.Settings.ContractResolver.ResolveNamingStrategyOrDefault());
#if NET48_OR_GREATER
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
  ""total_seconds"": 922337203685.47754
}".ReplaceLineEndings(), json);
#else
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
}".ReplaceLineEndings(), json);
#endif
        }

        [Fact]
        public void ResolveNamingStrategyOrDefault_ShouldResolveKebabCaseNamingStrategy()
        {
            var sut1 = TimeSpan.MaxValue;
            var sut2 = new NewtonsoftJsonFormatter(o =>
            {
                o.Settings.Converters.Add(_tsConverter);
                o.Settings.ContractResolver = new DefaultContractResolver() { NamingStrategy = new KebabCaseNamingStrategy() };
            });
            var json = sut2.Serialize(sut1).ToEncodedString();

            TestOutput.WriteLine(json);

            Assert.IsType<KebabCaseNamingStrategy>(sut2.Options.Settings.ContractResolver.ResolveNamingStrategyOrDefault());
#if NET48_OR_GREATER
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
  ""total-seconds"": 922337203685.47754
}".ReplaceLineEndings(), json);
#else
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
}".ReplaceLineEndings(), json);
#endif
        }
    }
}