using System;
using Cuemon.Extensions.Xunit;
using Cuemon.Integrity;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.AspNetCore.Mvc
{
    public class CacheableObjectTest : Test
    {
        public CacheableObjectTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateCacheableObject_ShouldHaveICacheableTimestampImplementation_WhenCreatedWithTimestampRelatedArguments()
        {
            var or = Generate.RandomString(2048);
            var cor = CacheableObjectFactory.CreateCacheableObjectResult(or, () => DateTime.MinValue, () => DateTime.MaxValue);
            Assert.IsAssignableFrom<ICacheableTimestamp>(cor);
            if (cor.Value is ICacheableTimestamp timestamp)
            {
                Assert.True(timestamp.Created == DateTime.MinValue);
                Assert.True(timestamp.Modified == DateTime.MinValue);
            }
            Assert.Equal(cor.Value, or);
        }

        [Fact]
        public void CreateCacheableObject_ShouldHaveICacheableIntegrityImplementation_WhenCreatedWithIntegrityRelatedArguments()
        {
            var or = Generate.RandomString(2048);
            var orBytes = Convertible.GetBytes(or);
            var cor = CacheableObjectFactory.CreateCacheableObjectResult(or, () => orBytes, () => false);
            Assert.IsAssignableFrom<ICacheableIntegrity>(cor);
            if (cor.Value is ICacheableIntegrity integrity)
            {
                Assert.True(integrity.Validation == ChecksumStrength.Strong);
                Assert.True(integrity.Checksum.GetBytes() == orBytes);
            }
            Assert.Equal(cor.Value, or);
        }

        [Fact]
        public void CreateCacheableObject_ShouldHaveICacheableEntityImplementation_WhenCreatedWithTimestampRelatedArguments_And_IntegrityRelatedArguments()
        {
            var or = Generate.RandomString(2048);
            var orBytes = Convertible.GetBytes(or);
            var cor = CacheableObjectFactory.CreateCacheableObjectResult(or, () => DateTime.MinValue, () => orBytes, () => DateTime.MaxValue, () => false);
            Assert.IsAssignableFrom<ICacheableEntity>(cor);
            if (cor.Value is ICacheableEntity entity)
            {
                Assert.True(entity.Created == DateTime.MinValue);
                Assert.True(entity.Modified == DateTime.MinValue);
                Assert.True(entity.Validation == ChecksumStrength.Strong);
                Assert.True(entity.Checksum.GetBytes() == orBytes);
            }
            Assert.Equal(cor.Value, or);
        }
    }
}