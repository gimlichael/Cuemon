using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Resilience
{
    public class ExceptionTest : Test
    {
        public ExceptionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void LatencyException_ShouldBeSerializable()
        {
            var ex = new LatencyException(Generate.RandomString(10));

            TestOutput.WriteLine(ex.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as LatencyException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }

        [Fact]
        public void TransientFaultException_ShouldBeSerializable()
        {
            var ex = new TransientFaultException(Generate.RandomString(25), new TransientFaultEvidence(10, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(2), MethodDescriptor.Create(MethodBase.GetCurrentMethod())));

            TestOutput.WriteLine(ex.ToString());

            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, ex);
                ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                var desEx = bf.Deserialize(ms) as TransientFaultException;
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                TestOutput.WriteLine(ex.Evidence.GetHashCode().ToString());
                TestOutput.WriteLine(desEx.Evidence.GetHashCode().ToString());
                Assert.Equal(ex.Evidence, desEx.Evidence);
                Assert.Equal(ex.Message, desEx.Message);
                Assert.Equal(ex.ToString(), desEx.ToString());
            }
        }
    }
}