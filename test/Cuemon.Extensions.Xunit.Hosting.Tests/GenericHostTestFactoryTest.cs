using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting
{
    public class GenericHostTestFactoryTest : Test
    {
        public GenericHostTestFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateGenericHostTest_ShouldCreateWithClassFixture()
        {
            var sut = GenericHostTestFactory.CreateGenericHostTest(services => { });

            Assert.True(sut.GetType().Name == "GenericHostTestDecorator", "sut.GetType().Name == 'GenericHostTestDecorator'");
        }

        [Fact]
        public void CreateGenericHostTest_ShouldCreateWithoutClassFixture()
        {
            var sut = GenericHostTestFactory.CreateGenericHostTest(false, services => { });

            Assert.True(sut.GetType().Name == "GenericHostTest", "sut.GetType().Name == 'GenericHostTest'");
        }
    }
}
