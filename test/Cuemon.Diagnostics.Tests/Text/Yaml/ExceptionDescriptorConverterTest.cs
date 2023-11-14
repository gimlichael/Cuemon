using System;
using System.Threading;
using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Text.Yaml.Formatters;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Diagnostics.Text.Yaml
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
                var formatter = new YamlFormatter(o => o.Settings.Converters.Add(new ExceptionDescriptorConverter(o => o.SensitivityDetails = FaultSensitivityDetails.All)));
                var result = formatter.Serialize(sut).ToEncodedString();
                
                TestOutput.WriteLine(result);

                Assert.StartsWith("""
                             Error: 
                               Code: X900
                               Message: Critical Error.
                               Failure: 
                                 Type: System.Threading.AbandonedMutexException
                                 Source: Cuemon.Diagnostics.Tests
                                 Message: The wait completed due to an abandoned mutex.
                                 Stack: 
                                   at Cuemon.Diagnostics.Text.Yaml.ExceptionDescriptorConverterTest.WriteYaml_ShouldSerializeToYamlFormat()
                             """.ReplaceLineEndings(), result);
                Assert.EndsWith(@"    Data: 
      MyKey: MyValue
    MutexIndex: -1".ReplaceLineEndings(), result);
            }

        }
    }
}
