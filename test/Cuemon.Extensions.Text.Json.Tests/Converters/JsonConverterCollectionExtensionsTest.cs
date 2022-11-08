using System;
using System.Linq;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Text.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Text.Json.Converters
{
    public class JsonConverterCollectionExtensionsTest : Test
    {
        public JsonConverterCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddStringEnumConverter_ShouldAddStringEnumConverterToConverterCollection_WithPascalCase()
        {
            var sut1 = HorizontalDirection.Left;
            var sut2 = new JsonFormatterOptions();
            sut2.Settings.PropertyNamingPolicy = null; // set PascalCase
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddStringEnumConverter();

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())), jc =>
            {
                var jf = new JsonFormatter(sut2);

                var result = jf.Serialize(sut1);

                var json = result.ToEncodedString();

                Assert.True(jc.CanConvert(typeof(HorizontalDirection)));
                Assert.Equal("\"Left\"", json);

                TestOutput.WriteLine(json);
            });
        }

        [Fact]
        public void AddStringFlagsEnumConverter_ShouldAddStringFlagsEnumConverterToConverterCollection_WithPascalCase()
        {
            var sut1 = GuidFormats.N | GuidFormats.X;
            var sut2 = new JsonFormatterOptions();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddStringFlagsEnumConverter();
            sut2.Settings.PropertyNamingPolicy = null;

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())), jc =>
            {
                var jf = new JsonFormatter(sut2);

                var result = jf.Serialize(sut1);

                var json = result.ToEncodedString(o => o.LeaveOpen = true);

                Assert.True(jc.CanConvert(typeof(GuidFormats)));
                Assert.Contains("[", json);
                Assert.Contains("\"N\",", json);
                Assert.Contains("\"X\"", json);
                Assert.Contains("]", json);

                var sut3 = jf.Deserialize<GuidFormats>(result);

                Assert.Equal(sut1, sut3);

                TestOutput.WriteLine(json);
            });
        }

        [Fact]
        public void AddStringEnumConverter_ShouldAddStringEnumConverterToConverterCollection()
        {
            var sut1 = HorizontalDirection.Left;
            var sut2 = new JsonFormatterOptions();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddStringEnumConverter();

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())), jc =>
            {
                var jf = new JsonFormatter(sut2);

                var result = jf.Serialize(sut1);

                var json = result.ToEncodedString();

                Assert.True(jc.CanConvert(typeof(HorizontalDirection)));
                Assert.Equal("\"left\"", json);

                TestOutput.WriteLine(json);
            });
        }

        [Fact]
        public void AddStringFlagsEnumConverter_ShouldAddStringFlagsEnumConverterToConverterCollection()
        {
            var sut1 = GuidFormats.N | GuidFormats.X;
            var sut2 = new JsonFormatterOptions();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddStringFlagsEnumConverter();

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(sut1.GetType())), jc =>
            {
                var jf = new JsonFormatter(sut2);

                var result = jf.Serialize(sut1);

                var json = result.ToEncodedString(o => o.LeaveOpen = true);

                Assert.True(jc.CanConvert(typeof(GuidFormats)));
                Assert.Contains("[", json);
                Assert.Contains("\"n\",", json);
                Assert.Contains("\"x\"", json);
                Assert.Contains("]", json);

                var sut3 = jf.Deserialize<GuidFormats>(result);

                Assert.Equal(sut1, sut3);

                TestOutput.WriteLine(json);
            });
        }

        [Theory]
        [InlineData(FaultSensitivityDetails.All)]
        [InlineData(FaultSensitivityDetails.None)]
        public void AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection_AndMakeUseOfIncludeOptions(FaultSensitivityDetails sensitivityDetails)
        {
            InsufficientMemoryException ime = null;
            try
            {
                throw new InsufficientMemoryException();
            }
            catch (InsufficientMemoryException e)
            {
                ime = e;
            }

            var sut1 = new ExceptionDescriptor(ime, "NoMemory", "System has exhausted memory.", new Uri("https://docs.microsoft.com/en-us/dotnet/api/system.insufficientmemoryexception"));
            sut1.AddEvidence("CorrelationId", Guid.Empty, correlationId => correlationId.ToString("N"));

            var sut2 = new JsonFormatterOptions()
            {
                SensitivityDetails = sensitivityDetails
            };

            sut2.Settings.Converters.AddExceptionDescriptorConverterOf<ExceptionDescriptor>(o =>
            {
                o.SensitivityDetails = sensitivityDetails;
            });

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(ExceptionDescriptor))), jc =>
            {
                var jf = new JsonFormatter(sut2);

                var result = jf.Serialize(sut1);

                var json = result.ToEncodedString();

                Assert.True(jc.CanConvert(typeof(ExceptionDescriptor)));
                Assert.Contains("\"error\":", json);
                Assert.Contains("\"code\": \"NoMemory\"", json);
                Assert.Contains("\"message\": \"System has exhausted memory.\"", json);
                Assert.Contains("\"helpLink\": \"https://docs.microsoft.com/en-us/dotnet/api/system.insufficientmemoryexception\"", json);

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.Failure), () =>
                {
                    Assert.Contains("\"failure\":", json);
                    Assert.Contains("\"type\": \"System.InsufficientMemoryException\"", json);
                    Assert.Contains("\"source\": \"Cuemon.Extensions.Text.Json.Tests\"", json);
                    Assert.Contains("\"message\": \"Insufficient memory to continue the execution of the program.\"", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"failure\":", json);
                    Assert.DoesNotContain("\"type\": \"System.InsufficientMemoryException\"", json);
                    Assert.DoesNotContain("\"source\": \"Cuemon.Extensions.Text.Json.Tests\"", json);
                    Assert.DoesNotContain("\"message\": \"Insufficient memory to continue the execution of the program.\"", json);
                });

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.StackTrace), () =>
                {
                    Assert.Contains("\"stack\":", json);
                    Assert.Contains("\"at Cuemon.Extensions.Text.Json.Converters.JsonConverterCollectionExtensionsTest.AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"stack\":", json);
                    Assert.DoesNotContain("\"at Cuemon.Extensions.Text.Json.Converters.JsonConverterCollectionExtensionsTest.AddExceptionDescriptorConverter_ShouldAddExceptionDescriptorConverterToConverterCollection", json);
                });

                Condition.FlipFlop(sensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence), () =>
                {
                    Assert.Contains("\"evidence\":", json);
                    Assert.Contains("\"correlationId\": \"00000000000000000000000000000000\"", json);
                }, () =>
                {
                    Assert.DoesNotContain("\"evidence\":", json);
                    Assert.DoesNotContain("\"correlationId\": \"00000000000000000000000000000000\"", json);
                });

                TestOutput.WriteLine(json);
            });
        }

        [Fact]
        public void AddDataPairConverter_ShouldAddDataPairConverterToConverterCollection()
        {
            var sut1 = new DataPair<int>("AnswerToEverything", 42);
            var sut2 = new JsonFormatterOptions();
            sut2.Settings.Converters.Clear();
            sut2.Settings.Converters.AddDataPairConverter();

            Assert.Collection(sut2.Settings.Converters.Where(jc => jc.CanConvert(typeof(DataPair))), jc =>
            {
                var jf = new JsonFormatter(sut2);

                var result = jf.Serialize(sut1);

                var json = result.ToEncodedString();

                Assert.True(jc.CanConvert(typeof(DataPair)));
                Assert.Contains("\"name\": \"AnswerToEverything\"", json);
                Assert.Contains("\"value\": 42", json);
                Assert.Contains("\"type\": \"Int32\"", json);

                TestOutput.WriteLine(json);
            });
        }
    }
}