using System.IO;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Microsoft.Extensions.Configuration;
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

                    ConfigureCallback(config.Build(), context.HostingEnvironment);
                })
                .ConfigureServices((context, services) =>
                {
                    Configuration = context.Configuration;
                    HostingEnvironment = context.HostingEnvironment;
                    ConfigureServicesCallback(services);
                }).Build();
        }
    }
}