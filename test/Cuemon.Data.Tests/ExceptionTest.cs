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
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                bf.Serialize(ms, ex);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as DataAdapterException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete
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
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                bf.Serialize(ms, ex);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as UniqueIndexViolationException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }
    }
}