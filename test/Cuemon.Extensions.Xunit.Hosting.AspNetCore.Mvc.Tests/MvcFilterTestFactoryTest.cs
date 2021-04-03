using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    public class MvcFilterTestFactoryTest : Test
    {
        public MvcFilterTestFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateMvcFilterTest_CallerTypeShouldHaveDeclaringTypeOfMvcFilterTestFactoryTest()
        {
            Type sut1 = GetType();
            string sut2 = null;
            var middleware = MvcFilterTestFactory.CreateMvcFilterTest(hostSetup: host =>
            {
                host.ConfigureAppConfiguration((context, configuration) =>
                {
                    sut2 = context.HostingEnvironment.ApplicationName;
                });
            });

            Assert.True(sut1 == middleware.CallerType.DeclaringType);
            Assert.Equal(GetType().Assembly.GetName().Name, sut2);
        }

        [Fact]
        public Task RunMvcFilterTest_ShouldHaveApplicationNameEqualToThisAssembly()
        {
            return MvcFilterTestFactory.RunMvcFilterTest(hostSetup: host =>
            {
                host.ConfigureAppConfiguration((context, configuration) =>
                {
                    TestOutput.WriteLine(context.HostingEnvironment.ApplicationName);
                    Assert.Equal(GetType().Assembly.GetName().Name, context.HostingEnvironment.ApplicationName);
                });
            });
        }
    }
}