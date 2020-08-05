using System;
using Cuemon.Extensions.Xunit;
using Cuemon.Integrity;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Integrity
{
    public class DateTimeExtensionsTest : Test
    {
        public DateTimeExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void GetCacheValidator_ShouldHaveNoneIntegrityChecksum()
        {
            var dt = DateTime.UnixEpoch.GetCacheValidator();
            var expected = "d41d8cd98f00b204e9800998ecf8427e";
            Assert.Equal(expected, dt.ToString());
            Assert.Equal(ChecksumStrength.None, dt.Strength);
            TestOutput.WriteLine(dt.ToString());
        }

        [Fact]
        public void GetCacheValidator_ShouldHaveWeakIntegrityChecksum()
        {
            var dt = DateTime.UnixEpoch.GetCacheValidator(DateTime.UnixEpoch.AddDays(7), o => o.Method = ChecksumMethod.Timestamp);
            var expected = "15f5959e2cf903cf2bf3dc93d4c89cb3";
            Assert.Equal(expected, dt.ToString());
            Assert.Equal(ChecksumStrength.Weak, dt.Strength);
            TestOutput.WriteLine(dt.ToString());
        }

        [Fact]
        public void GetCacheValidator_ShouldHaveStrongIntegrityChecksum()
        {
            var dt = DateTime.UnixEpoch.GetCacheValidator(DateTime.UnixEpoch.AddDays(7), 1234567890);
            var expected = "bbb1f04aceb3b6903f7f364a25501220";
            Assert.Equal(expected, dt.ToString());
            Assert.Equal(ChecksumStrength.Strong, dt.Strength);
            TestOutput.WriteLine(dt.ToString());
        }
    }
}