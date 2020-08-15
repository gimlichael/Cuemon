using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Http.Throttling
{
    public class ExceptionTest : Test
    {
        public ExceptionTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void ThrottlingException_ShouldBeSerializable()
        {
            var ex = new ThrottlingException(429, "Throttling rate limit quota violation. Quota limit exceeded.", 100, TimeSpan.FromHours(1), DateTime.Today.AddDays(1));

            TestOutput.WriteLine(ex.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Position = 0;
                var desEx = bf.Deserialize(ms) as ThrottlingException;
                Assert.Equal(ex.StatusCode, desEx.StatusCode);
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.Delta, desEx.Delta);
                Assert.Equal(ex.Reset, desEx.Reset);
                Assert.Equal(ex.RateLimit, desEx.RateLimit);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }
    }
}