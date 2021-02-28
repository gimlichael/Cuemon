using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Data
{
    public class ExceptionTest : Test
    {
        public ExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DataAdapterException_ShouldBeSerializable()
        {
            var ex = new DataAdapterException(Generate.RandomString(10));

            TestOutput.WriteLine(ex.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Position = 0;
                var desEx = bf.Deserialize(ms) as DataAdapterException;
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }

        [Fact]
        public void UniqueIndexViolationException_ShouldBeSerializable()
        {
            var ex = new UniqueIndexViolationException(Generate.RandomString(10));

            TestOutput.WriteLine(ex.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Position = 0;
                var desEx = bf.Deserialize(ms) as UniqueIndexViolationException;
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }
    }
}