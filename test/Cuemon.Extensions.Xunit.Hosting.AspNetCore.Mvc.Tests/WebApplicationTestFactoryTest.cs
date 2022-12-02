using System;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    public class WebApplicationTestFactoryTest : Test
    {
        public WebApplicationTestFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void Create_CallerTypeShouldHaveDeclaringTypeOfMvcFilterTestFactoryTest()
        {
            Type sut1 = GetType();
            string sut2 = null;
            var middleware = WebApplicationTestFactory.Create(Assert.NotNull, Assert.NotNull, host =>
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
        public Task Run_ShouldHaveApplicationNameEqualToThisAssembly()
        {
            return WebApplicationTestFactory.Run(Assert.NotNull, Assert.NotNull, host =>
            {
                host.ConfigureAppConfiguration((context, configuration) =>
                {
                    TestOutput.WriteLine(context.HostingEnvironment.ApplicationName);
                    Assert.Equal(GetType().Assembly.GetName().Name, context.HostingEnvironment.ApplicationName);
                });
            });
        }

        [Fact]
        public Task RunWithHostBuilderContext_ShouldHaveApplicationNameEqualToThisAssembly_WithHostBuilderContext()
        {
            return WebApplicationTestFactory.RunWithHostBuilderContext((context, app) =>
            {
                Assert.NotNull(context);
                Assert.NotNull(context.HostingEnvironment);
                Assert.NotNull(context.Configuration);
                Assert.NotNull(context.Properties);
                Assert.NotNull(app);
            }, (context, services) =>
            {
                Assert.NotNull(context);
                Assert.NotNull(context.HostingEnvironment);
                Assert.NotNull(context.Configuration);
                Assert.NotNull(context.Properties);
                Assert.NotNull(services);
            }, host =>
            {
                host.ConfigureAppConfiguration((context, configuration) =>
                {
                    TestOutput.WriteLine(context.HostingEnvironment.ApplicationName);
                    Assert.Equal(GetType().Assembly.GetName().Name, context.HostingEnvironment.ApplicationName);
                });
            });
        }

        [Fact]
        public void Create_ShouldCreateWithClassFixture()
        {
            var sut = WebApplicationTestFactory.Create(services => { });
            var innerType = sut.GetType().GetField("_middlewareTest", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sut).GetType();

            Assert.NotNull(sut.Host);
            Assert.True(innerType.Name == "MiddlewareTestDecorator", "sut.GetType().Name == 'MiddlewareTestDecorator'");
        }

        [Fact]
        public void CreateGenericHostTest_ShouldCreateWithoutClassFixture()
        {
            var sut = WebApplicationTestFactory.Create(false, services => { });
            var innerType = sut.GetType().GetField("_middlewareTest", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(sut).GetType();

            Assert.NotNull(sut.Host);
            Assert.True(innerType.Name == "MiddlewareTest", "sut.GetType().Name == 'MiddlewareTest'");
        }
    }
}
