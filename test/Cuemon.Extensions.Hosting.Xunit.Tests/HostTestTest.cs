using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Cuemon.Extensions.Xunit.Hosting.Assets;
using Cuemon.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace Cuemon.Extensions.Xunit.Hosting
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class HostTestTest : HostTest<HostFixture>
    {
        private readonly Func<IList<ICorrelation>> _correlationsFactory;
        private static readonly ConcurrentBag<ICorrelation> ScopedCorrelations = new ConcurrentBag<ICorrelation>();

        public HostTestTest(HostFixture hostFixture, ITestOutputHelper output) : base(hostFixture, output)
        {
            _correlationsFactory = () => hostFixture.ServiceProvider.GetServices<ICorrelation>().ToList();
        }

        [Fact, Priority(1)]
        public void Test_SingletonShouldBeSame()
        {
            ScopedCorrelations.Add(_correlationsFactory().Single(c => c is ScopedCorrelation));
            var c1 = _correlationsFactory().Single(c => c is SingletonCorrelation);
            var c2 = _correlationsFactory().Single(c => c is SingletonCorrelation);
            Assert.Equal(c1.CorrelationId, c2.CorrelationId);
        }

        [Fact, Priority(2)]
        public void Test_TransientShouldBeDifferent()
        {
            ScopedCorrelations.Add(_correlationsFactory().Single(c => c is ScopedCorrelation));
            var c1 = _correlationsFactory().Single(c => c is TransientCorrelation);
            var c2 = _correlationsFactory().Single(c => c is TransientCorrelation);
            Assert.NotEqual(c1.CorrelationId, c2.CorrelationId);
        }

        [Fact, Priority(3)]
        public void Test_ScopedShouldBeSame()
        {
            ScopedCorrelations.Add(_correlationsFactory().Single(c => c is ScopedCorrelation));
            var c1 = _correlationsFactory().Single(c => c is ScopedCorrelation);
            var c2 = _correlationsFactory().Single(c => c is ScopedCorrelation);
            Assert.Equal(c1.CorrelationId, c2.CorrelationId);
        }

        [Fact]
        public void Test_ScopedShouldBeSameInLastTestRun()
        {
            var c1 = _correlationsFactory().Single(c => c is ScopedCorrelation);
            if (ScopedCorrelations.IsEmpty) { return; }
            Assert.Equal(3, ScopedCorrelations.Count);
            Assert.All(ScopedCorrelations, c => Assert.Equal(c1.CorrelationId, c.CorrelationId));
        }

        [Fact]
        public void Test_ShouldHaveConfigurationEntry()
        {
            Assert.Equal("xUnit", Configuration.GetSection("unitTestTool").Value);
        }

        [Fact]
        public void Test_ShouldHaveEnvironmentOfroduction()
        {
            Assert.Equal("Production", HostingEnvironment.EnvironmentName);
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ICorrelation, SingletonCorrelation>();
            services.AddTransient<ICorrelation, TransientCorrelation>();
            services.AddScoped<ICorrelation, ScopedCorrelation>();
        }
    }
}