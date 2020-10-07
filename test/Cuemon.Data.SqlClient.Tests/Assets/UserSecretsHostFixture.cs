using System.IO;
using System.Linq;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Cuemon.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cuemon.Data.SqlClient.Assets
{
    public sealed class UserSecretsHostFixture : HostFixture
    {
        public override void ConfigureHost(Test hostTest)
        {
            var hostTestType = hostTest?.GetType();
            Validator.ThrowIfNull(hostTest, nameof(hostTest));
            Validator.ThrowIfNotContainsType(hostTestType, nameof(hostTestType), $"{nameof(hostTest)} is not assignable from HostTest<T>.", typeof(HostTest<>));

            Host = new HostBuilder()
                .ConfigureHostConfiguration(config => config.AddEnvironmentVariables("DOTNET_"))
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables()
                        .AddUserSecrets<UserSecretsHostFixture>();
                })
                .ConfigureServices((context, services) =>
                {
                    var flags = new MemberReflection(excludeStatic: true, excludePublic: true).Flags;
                    var hostTestTypeBase = Decorator.Enclose(hostTestType).GetInheritedTypes().Single(t => t.BaseType == typeof(Test));
                    hostTestTypeBase.GetField("_configuration", flags).SetValue(hostTest, context.Configuration);
                    hostTestTypeBase.GetField("_hostingEnvironment", flags).SetValue(hostTest, context.HostingEnvironment);
                    ConfigureServicesCallback(services);
                    ServiceProvider = services.BuildServiceProvider();
                }).Build();
        }
    }
}