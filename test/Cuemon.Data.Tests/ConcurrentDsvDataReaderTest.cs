using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Data
{
    public class ConcurrentDsvDataReaderTest : Test
    {
        public ConcurrentDsvDataReaderTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public async Task DsvDataReader_ShouldReadEmbeddedResourceLineByLine()
        {
            var file = Decorator.Enclose(typeof(DsvDataReaderTest).Assembly).GetManifestResources("DsvDataReaderTest_Wiki.csv", ManifestResourceMatch.ContainsName).Values.Single();
            ConcurrentDsvDataReader reader = null;
            using (reader = new ConcurrentDsvDataReader(new StreamReader(file)))
            {
                while (await reader.ReadAsync())
                {
                    TestOutput.WriteLine(reader.ToString());
                }
            }
            Assert.Equal(5, reader.FieldCount);
            Assert.Equal(4, reader.RowCount);
            Assert.True(reader.Disposed);
            await Assert.ThrowsAsync<ObjectDisposedException>(() => reader.ReadAsync());
        }
    }
}