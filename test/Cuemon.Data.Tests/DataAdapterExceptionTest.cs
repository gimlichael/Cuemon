using Cuemon.Extensions;
using Cuemon.Extensions.IO;
using Cuemon.Extensions.Newtonsoft.Json.Formatters;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Data
{
    public class DataAdapterExceptionTest : Test
    {
        public DataAdapterExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DataAdapterException_ShouldBeSerializable_Json()
        {
            var random = Generate.RandomString(10);
            var sut1 =new DataAdapterException(random);
            var sut2 = new JsonFormatter();
            var sut3 = sut2.Serialize(sut1);
            var sut4 = sut3.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(sut4);

            var original = sut2.Deserialize<DataAdapterException>(sut3);

            sut3.Dispose();

            Assert.Equal(sut1.Message, original.Message);
            Assert.Equal(sut1.ToString(), original.ToString());

            Assert.Equal($$"""
                           {
                             "type": "Cuemon.Data.DataAdapterException",
                             "message": "{{random}}"
                           }
                           """.ReplaceLineEndings(), sut4);
        }
    }
}
