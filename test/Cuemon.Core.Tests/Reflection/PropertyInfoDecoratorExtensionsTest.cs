using System;
using Cuemon.Core.Assets;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Core.Reflection
{
    public class PropertyInfoDecoratorExtensionsTest : Test
    {
        public PropertyInfoDecoratorExtensionsTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void IsOverridden_ShouldBeTrueWhenClassHasOverridenMethod()
        {
            var b = new ClassBase();
            var bmi = b.GetType().GetProperty("Id");
            var d = new ClassDerived();
            var dmi = d.GetType().GetProperty("Id");

            Assert.Equal(Guid.Empty, b.Id);
            Assert.False(Decorator.Enclose(bmi).IsOverridden());

            Assert.NotEqual(Guid.Empty, d.Id);
            Assert.True(Decorator.Enclose(dmi).IsOverridden());
        }
    }
}