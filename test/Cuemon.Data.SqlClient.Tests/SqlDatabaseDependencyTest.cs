using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.Data.SqlClient.Assets;
using Cuemon.Extensions.Xunit;
using Cuemon.Extensions.Xunit.Hosting;
using Cuemon.Runtime;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Data.SqlClient
{
    public class SqlDatabaseDependencyTest : HostTest<UserSecretsHostFixture>
    {
        private readonly IDbConnection _connection;

        public SqlDatabaseDependencyTest(UserSecretsHostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
        {
            _connection = hostFixture.ServiceProvider.GetRequiredService<SqlConnection>();
        }

        [Fact]
        public async Task StartAsync_ShouldReceiveTwoSignalsFromDatabaseWatcher()
        {
            var ce = new CountdownEvent(2);
            var sut1 = _connection;
            var sut2 = new Lazy<DatabaseWatcher>(() => new DatabaseWatcher(sut1, connection =>
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM [Person].[ContactType]";
                return command.ExecuteReader();

            }, o => o.Period = TimeSpan.FromMilliseconds(500)));
            var sut3 = new DatabaseDependency(sut2);
            var sut4 = DateTime.UtcNow;
            var sut5 = new List<DateTime>();
            var sut6 = new EventHandler<DependencyEventArgs>((s, e) =>
            {
                sut5.Add(e.UtcLastModified);
                ce.Signal();
            });

            sut3.DependencyChanged += sut6;

            await sut3.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(1));

            new SqlDataManager(_connection.ConnectionString).Execute(new DataCommand(@"INSERT INTO [Person].[ContactType] ([Name]) VALUES (@name)"), new SqlParameter("@name", $"Fleet Admiral - {Generate.RandomString(5)}")); // should trigger last modified

            await Task.Delay(TimeSpan.FromSeconds(1));

            new SqlDataManager(_connection.ConnectionString).Execute(new DataCommand(@"INSERT INTO [Person].[ContactType] ([Name]) VALUES (@name)"), new SqlParameter("@name", $"Lieutenant Commander - {Generate.RandomString(5)}")); // should trigger last modified

            var signaled = ce.Wait(TimeSpan.FromSeconds(15));

            TestOutput.WriteLines(sut5);

            sut3.DependencyChanged -= sut6;

            Assert.True(signaled);
            Assert.True(sut2.IsValueCreated);
            Assert.True(sut3.HasChanged);
            Assert.NotNull(sut3.UtcLastModified);
            Assert.InRange(sut3.UtcLastModified.Value, sut4, sut4.AddSeconds(5));
            Assert.Equal(2, sut5.Count);
        }

        [Fact]
        public async Task StartAsync_ShouldReceiveOnlyOneSignalFromDatabaseWatcher()
        {
            var are = new AutoResetEvent(false);
            var sut1 = _connection;
            var sut2 = new Lazy<DatabaseWatcher>(() => new DatabaseWatcher(sut1, connection =>
            {
                var command = connection.CreateCommand();
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM [Person].[ContactType]";
                return command.ExecuteReader();

            }, o => o.Period = TimeSpan.FromMilliseconds(500)));
            var sut3 = new DatabaseDependency(sut2, true);
            var sut4 = DateTime.UtcNow;
            var sut5 = new List<DateTime>();
            var sut6 = new EventHandler<DependencyEventArgs>((s, e) =>
            {
                sut5.Add(e.UtcLastModified);
                are.Set();
            });

            sut3.DependencyChanged += sut6;

            await sut3.StartAsync();

            await Task.Delay(TimeSpan.FromSeconds(1));

            new SqlDataManager(_connection.ConnectionString).Execute(new DataCommand(@"INSERT INTO [Person].[ContactType] ([Name]) VALUES (@name)"), new SqlParameter("@name", $"Fleet Admiral - {Generate.RandomString(5)}")); // should trigger last modified

            await Task.Delay(TimeSpan.FromSeconds(1));

            new SqlDataManager(_connection.ConnectionString).Execute(new DataCommand(@"INSERT INTO [Person].[ContactType] ([Name]) VALUES (@name)"), new SqlParameter("@name", $"Lieutenant Commander - {Generate.RandomString(5)}")); // should trigger last modified

            var signaled = are.WaitOne(TimeSpan.FromSeconds(15));

            TestOutput.WriteLines(sut5);

            sut3.DependencyChanged -= sut6;

            Assert.True(signaled);
            Assert.True(sut2.IsValueCreated);
            Assert.True(sut3.HasChanged);
            Assert.NotNull(sut3.UtcLastModified);
            Assert.InRange(sut3.UtcLastModified.Value, sut4, sut4.AddSeconds(5));
            Assert.Equal(1, sut5.Count);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            var cnn = $"Persist Security Info=True;{Configuration.GetConnectionString("AdventureWorks")}";
            services.AddSingleton(new SqlConnection(cnn));
        }
    }
}