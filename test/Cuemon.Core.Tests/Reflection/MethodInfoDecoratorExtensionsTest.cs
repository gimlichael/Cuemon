using Cuemon.Core.Assets;
using Cuemon.Extensions.Xunit;
using Cuemon.Reflection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Core.Reflection
{
    public class MethodInfoDecoratorExtensionsTest : Test
    {
        public MethodInfoDecoratorExtensionsTest(ITestOutputHelper output = null) : base(output)
        {
        }

        [Fact]
        public void IsOverridden_ShouldBeTrueWhenClassHasOverridenMethod()
        {
            var b = new ClassBase();
            var bmi = b.GetType().GetMethod("GetSomeNumber");
            var d = new ClassDerived();
            var dmi = d.GetType().GetMethod("GetSomeNumber");

            Assert.Equal(int.MinValue, b.GetSomeNumber());
            Assert.False(Decorator.Enclose(bmi).IsOverridden());

            Assert.Equal(int.MaxValue, d.GetSomeNumber());
            Assert.True(Decorator.Enclose(dmi).IsOverridden());
        }
    }
}