using System;
using System.IO;
using System.Linq;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Data
{
    public class DsvDataReaderTest : Test
    {
        public DsvDataReaderTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void DsvDataReader_ShouldReadEmbeddedResourceLineByLine()
        {
            var file = Decorator.Enclose(typeof(DsvDataReaderTest).Assembly).GetManifestResources("DsvDataReaderTest_Wiki.csv", ManifestResourceMatch.ContainsName).Values.Single();
            DsvDataReader reader = null;
            using (reader = new DsvDataReader(new StreamReader(file)))
            {
                while (reader.Read())
                {
                    TestOutput.WriteLine(reader.ToString());
                }
            }
            Assert.Equal(5, reader.FieldCount);
            Assert.Equal(4, reader.RowCount);
            Assert.True(reader.Disposed);
            Assert.Throws<ObjectDisposedException>(() => reader.Read());
        }
    }
}