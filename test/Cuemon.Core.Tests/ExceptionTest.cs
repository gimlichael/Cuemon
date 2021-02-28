using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon
{
    public class ExceptionTest : Test
    {
        public ExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void TypeArgumentException_ShouldBeSerializable()
        {
            var ex = new TypeArgumentException(Generate.RandomString(10));

            TestOutput.WriteLine(ex.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Position = 0;
                var desEx = bf.Deserialize(ms) as TypeArgumentException;
                Assert.Equal(ex.ParamName, desEx.ParamName);
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }

        [Fact]
        public void TypeArgumentOutOfRangeException_ShouldBeSerializable()
        {
            var ex = new TypeArgumentOutOfRangeException(Generate.RandomString(10), 42, Generate.RandomString(50));

            TestOutput.WriteLine(ex.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Position = 0;
                var desEx = bf.Deserialize(ms) as TypeArgumentOutOfRangeException;
                Assert.Equal(ex.ParamName, desEx.ParamName);
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.ActualValue, desEx.ActualValue);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }
    }
}