using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    public class MiddlewareTestFactoryTest : Test
    {
        public MiddlewareTestFactoryTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateMiddlewareTest_CallerTypeShouldHaveDeclaringTypeOfMiddlewareTestFactoryTest()
        {
            Type sut1 = GetType();
            string sut2 = null;
            var middleware = MiddlewareTestFactory.Create(Assert.NotNull, Assert.NotNull, host =>
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
        public Task RunMiddlewareTest_ShouldHaveApplicationNameEqualToThisAssembly()
        {
            return MiddlewareTestFactory.Run(Assert.NotNull, Assert.NotNull, host =>
              {
                  host.ConfigureAppConfiguration((context, configuration) =>
                  {
                      TestOutput.WriteLine(context.HostingEnvironment.ApplicationName);
                      Assert.Equal(GetType().Assembly.GetName().Name, context.HostingEnvironment.ApplicationName);
                  });
              });
        }

        [Fact]
        public Task RunMiddlewareTest_ShouldHaveApplicationNameEqualToThisAssembly_WithHostBuilderContext()
        {
            return MiddlewareTestFactory.RunWithHostBuilderContext((context, app) =>
                {
                    Assert.NotNull(context);
                    Assert.NotNull(context.HostingEnvironment);
                    Assert.NotNull(context.Configuration);
                    Assert.NotNull(context.Properties);
                    Assert.NotNull(app);
                },
                (context, services) =>
                {
                    Assert.NotNull(context);
                    Assert.NotNull(context.HostingEnvironment);
                    Assert.NotNull(context.Configuration);
                    Assert.NotNull(context.Properties);
                    Assert.NotNull(services);
                },
                host =>
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
            var sut = MiddlewareTestFactory.Create(services => { });

            Assert.True(sut.GetType().Name == "MiddlewareTestDecorator", "sut.GetType().Name == 'MiddlewareTestDecorator'");
        }

        [Fact]
        public void CreateGenericHostTest_ShouldCreateWithoutClassFixture()
        {
            var sut = MiddlewareTestFactory.Create(false, services => { });

            Assert.True(sut.GetType().Name == "MiddlewareTest", "sut.GetType().Name == 'MiddlewareTest'");
        }
    }
}