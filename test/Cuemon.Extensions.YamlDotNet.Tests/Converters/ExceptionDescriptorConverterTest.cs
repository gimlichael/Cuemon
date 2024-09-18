using System;
using System.Threading;
using Cuemon.Diagnostics;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Cuemon.Extensions.YamlDotNet.Formatters;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.YamlDotNet.Converters
{
    public class ExceptionDescriptorConverterTest : Test
    {
        public ExceptionDescriptorConverterTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void WriteYaml_ShouldSerializeToYamlFormat()
        {
            try
            {
                throw new AbandonedMutexException()
                {
                    Data = { { "MyKey", "MyValue" } }
                };
            }
            catch (Exception ex)
            {
                var sut = new ExceptionDescriptor(ex, "X900", "Critical Error.");
                var formatter = new YamlFormatter(o => o.Settings.Converters.Add(new ExceptionDescriptorConverter(io => io.SensitivityDetails = FaultSensitivityDetails.All)));
                var result = formatter.Serialize(sut).ToEncodedString();

                TestOutput.WriteLine(result);

                Assert.StartsWith("""
                             Error:
                               Code: X900
                               Message: Critical Error.
                               Failure:
                                 Type: System.Threading.AbandonedMutexException
                                 Source: Cuemon.Extensions.YamlDotNet.Tests
                                 Message: The wait completed due to an abandoned mutex.
                                 Stack:
                                   - at Cuemon.Extensions.YamlDotNet.Converters.ExceptionDescriptorConverterTest.WriteYaml_ShouldSerializeToYamlFormat()
                             """.ReplaceLineEndings(), result);
                Assert.EndsWith(@"    Data:
      MyKey: MyValue
    MutexIndex: -1
".ReplaceLineEndings(), result);
            }

        }
    }
}
