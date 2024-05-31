using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Net
{
    public class QueryStringCollectionTest : Test
    {
        public QueryStringCollectionTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Class_ShouldHaveKeysAndValuesRulesetAsQueryString()
        {
            var query = new QueryStringCollection("?a=1&b=2&c=3&a=2");
            var queryString = query.ToString();


            Assert.Contains("a=1", queryString);
            Assert.Contains("a=2", queryString);
            Assert.Contains("b=2", queryString);
            Assert.Contains("c=3", queryString);

            TestOutput.WriteLine(query.ToString());

            Assert.Throws<SerializationException>(() =>
            {
                var bf = new BinaryFormatter();
                using (var ms = new MemoryStream())
                {
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    bf.Serialize(ms, query);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                    ms.Position = 0;
#pragma warning disable SYSLIB0011 // Type or member is obsolete
                    bf.Deserialize(ms);
#pragma warning restore SYSLIB0011 // Type or member is obsolete
                }
            });

        }
    }
}