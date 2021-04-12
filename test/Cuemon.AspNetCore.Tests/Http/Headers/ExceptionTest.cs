using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Http.Headers
{
    public class ExceptionTest : Test
    {
        public ExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void UserAgentException_ShouldBeSerializable()
        {
            var ex = new UserAgentException(400, "Bad Request.");

            TestOutput.WriteLine(ex.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                bf.Serialize(ms, ex);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as UserAgentException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                Assert.Equal(ex.StatusCode, desEx.StatusCode);
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }
    }
}