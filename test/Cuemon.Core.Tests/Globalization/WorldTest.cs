using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon.Extensions.Xunit;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Globalization
{
    public class WorldTest : Test
    {
        public WorldTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Regions_ShouldReturnAllRegions()
        {
            var sut1 = World.Regions.ToList();

            TestOutput.WriteLine(sut1.Count.ToString());

            Assert.NotNull(sut1);
            Assert.True(sut1.Count > 220, "sut1.Count > 220");
        }

        [Fact]
        public void Regions_ShouldReturnAllCultures_FromRegions()
        {
            var sut1 = World.Regions.ToList();
            var sut2 = new List<CultureInfo>();

            foreach (var region in sut1)
            {
                foreach (var culture in World.GetCultures(region))
                {
                    sut2.Add(culture);
                    if (culture.IsNeutralCulture)
                    {
                        TestOutput.WriteLine(culture.Name);
                    }
                }
            }

            TestOutput.WriteLine(sut2.Count.ToString());

            Assert.NotNull(sut2);
            Assert.True(sut2.Count > 500, "sut1.Count > 500");
        }
    }
}
