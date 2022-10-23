using System.Linq;
using Cuemon.Extensions.DependencyInjection.Assets;
using Cuemon.Extensions.Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;
using Xunit.Abstractions;

namespace Cuemon.Extensions.DependencyInjection
{
    public class ServiceCollectionExtensionsTest : Test
    {
        public ServiceCollectionExtensionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void AddWithSetup_ShouldAddServiceToServiceCollectionWithSpecifiedLifetime()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.Add<FakeService, FakeServiceScoped>(o => o.Lifetime = ServiceLifetime.Scoped)
                .Add<FakeService, FakeServiceSingleton>(o => o.Lifetime = ServiceLifetime.Singleton)
                .Add<FakeService, FakeServiceTransient>(o => o.Lifetime = ServiceLifetime.Transient);
            var sut3 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceScoped));
            var sut4 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
            var sut5 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceTransient));

            Assert.Equal(3, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.NotNull(sut4);
            Assert.NotNull(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
            Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
        }

        [Fact]
        public void AddWithSetup_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetime()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.Add<FakeService, FakeServiceScoped>(_ => new FakeServiceScoped(default), o => o.Lifetime = ServiceLifetime.Scoped)
                .Add<FakeService, FakeServiceSingleton>(_ => new FakeServiceSingleton(default), o => o.Lifetime = ServiceLifetime.Singleton)
                .Add<FakeService, FakeServiceTransient>(_ => new FakeServiceTransient(default), o => o.Lifetime = ServiceLifetime.Transient);
            var sut3 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
            var sut4 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
            var sut5 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));

            Assert.Equal(3, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.NotNull(sut4);
            Assert.NotNull(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
            Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
        }

        [Fact]
        public void TryAddWithSetup_ShouldAddServiceToServiceCollectionWithSpecifiedLifetime()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped>(o => o.Lifetime = ServiceLifetime.Scoped)
                .TryAdd<FakeService, FakeServiceSingleton>(o => o.Lifetime = ServiceLifetime.Singleton)
                .TryAdd<FakeService, FakeServiceTransient>(o => o.Lifetime = ServiceLifetime.Transient);
            var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceScoped));
            var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
            var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceTransient));

            Assert.Equal(1, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.Null(sut4);
            Assert.Null(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        }

        [Fact]
        public void TryAddWithSetup_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetime()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped>(_ => new FakeServiceScoped(default), o => o.Lifetime = ServiceLifetime.Scoped)
                .TryAdd<FakeService, FakeServiceSingleton>(_ => new FakeServiceSingleton(default), o => o.Lifetime = ServiceLifetime.Singleton)
                .TryAdd<FakeService, FakeServiceTransient>(_ => new FakeServiceTransient(default), o => o.Lifetime = ServiceLifetime.Transient);
            var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
            var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
            var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));

            Assert.Equal(1, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.Null(sut4);
            Assert.Null(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        }

        [Fact]
        public void AddWithSetup_ShouldAddServiceToServiceCollectionWithTypeForwarding()
        {
            var sut1 = new ServiceCollection()
                .Add<Foo>(o =>
                {
                    o.Lifetime = ServiceLifetime.Scoped;
                    o.UseNestedTypeForwarding = true;
                });
            
            var provider = sut1.BuildServiceProvider();

            var sut2 = provider.GetRequiredService<Foo>();
            var sut3 = provider.GetRequiredService<IBar>();
            var sut4 = provider.GetRequiredService<IFoo>();

            Assert.Same(sut2, sut3); 
            Assert.Same(sut2, sut4); 

            Assert.Collection(sut1, 
                sd => Assert.True(sd.ServiceType == typeof(Foo)),
                sd => Assert.True(sd.ServiceType == typeof(IFoo)),
                sd => Assert.True(sd.ServiceType == typeof(IBar)));
        }

        [Fact]
        public void AddWithSetup_ShouldAddServiceToServiceCollectionWithoutTypeForwarding()
        {
            var sut1 = new ServiceCollection()
                .Add<Foo>(o =>
                {
                    o.Lifetime = ServiceLifetime.Scoped;
                });
            
            var provider = sut1.BuildServiceProvider();

            var sut2 = provider.GetService<Foo>();
            var sut3 = provider.GetService<IBar>();
            var sut4 = provider.GetService<IFoo>();

            Assert.NotNull(sut2);
            Assert.Null(sut3);
            Assert.Null(sut4);

            Assert.Collection(sut1, 
                sd => Assert.True(sd.ServiceType == typeof(Foo)));
        }

        [Fact]
        public void AddWithSetup_ShouldAddServiceToServiceCollectionWithTypeForwardingForMarker()
        {
            var sut1 = new ServiceCollection()
                .Add<DefaultService<FakeService>>(o =>
                {
                    o.Lifetime = ServiceLifetime.Scoped;
                    o.UseNestedTypeForwarding = true;
                });
            
            var provider = sut1.BuildServiceProvider();

            var sut2 = provider.GetRequiredService<DefaultService<FakeService>>();
            var sut3 = provider.GetRequiredService<IService<FakeService>>();
            var sut4 = provider.GetRequiredService<IDependencyInjectionMarker<FakeService>>();

            Assert.Same(sut2, sut3); 
            Assert.Same(sut2, sut4); 

            Assert.Collection(sut1, 
                sd => Assert.True(sd.ServiceType == typeof(DefaultService<FakeService>)),
                sd => Assert.True(sd.ServiceType == typeof(IService<FakeService>)),
                sd => Assert.True(sd.ServiceType == typeof(IDependencyInjectionMarker<FakeService>)));
        }

        [Fact]
        public void AddWithSetup_ShouldAddServiceToServiceCollectionWithoutTypeForwardingForMarker()
        {
            var sut1 = new ServiceCollection()
                .Add<DefaultService<FakeService>>(o =>
                {
                    o.Lifetime = ServiceLifetime.Scoped;
                });
            
            var provider = sut1.BuildServiceProvider();

            var sut2 = provider.GetService<DefaultService<FakeService>>();
            var sut3 = provider.GetService<IService<FakeService>>();
            var sut4 = provider.GetService<IDependencyInjectionMarker<FakeService>>();

            Assert.NotNull(sut2);
            Assert.Null(sut3); 
            Assert.Null(sut4); 

            Assert.Collection(sut1, 
                sd => Assert.True(sd.ServiceType == typeof(DefaultService<FakeService>)));
        }

        [Fact]
        public void Add_ShouldAddServiceToServiceCollectionWithSpecifiedLifetime()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.Add<FakeService, FakeServiceScoped>(ServiceLifetime.Scoped)
                .Add<FakeService, FakeServiceSingleton>(ServiceLifetime.Singleton)
                .Add<FakeService, FakeServiceTransient>(ServiceLifetime.Transient);
            var sut3 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceScoped));
            var sut4 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
            var sut5 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceTransient));

            Assert.Equal(3, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.NotNull(sut4);
            Assert.NotNull(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
            Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
        }

        [Fact]
        public void Add_ShouldAddServiceToServiceCollectionWithSpecifiedLifetimeAndOptions()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.Add<FakeService, FakeServiceScoped, FakeServiceScopedOptions>(ServiceLifetime.Scoped, o => o.Greeting = "Hejsa!")
                .Add<FakeService, FakeServiceSingleton, FakeServiceSingletonOptions>(ServiceLifetime.Singleton, o => o.Greeting = "Hello!")
                .Add<FakeService, FakeServiceTransient, FakeServiceTransientOptions>(ServiceLifetime.Transient, o => o.Greeting = "Aloha!");
            var sut3 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceScoped));
            var sut4 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
            var sut5 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceTransient));
            var sut6 = sut2.BuildServiceProvider();
            var sut7 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceScoped));
            var sut8 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceSingleton));
            var sut9 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceTransient));

            Assert.Equal(11, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.NotNull(sut4);
            Assert.NotNull(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
            Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
            Assert.Equal("Hejsa!", sut7.Greeting);
            Assert.Equal("Hello!", sut8.Greeting);
            Assert.Equal("Aloha!", sut9.Greeting);
        }

        [Fact]
        public void Add_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetime()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.Add<FakeService, FakeServiceScoped>(_ => new FakeServiceScoped(default), ServiceLifetime.Scoped)
                .Add<FakeService, FakeServiceSingleton>(_ => new FakeServiceSingleton(default), ServiceLifetime.Singleton)
                .Add<FakeService, FakeServiceTransient>(_ => new FakeServiceTransient(default), ServiceLifetime.Transient);
            var sut3 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
            var sut4 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
            var sut5 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));

            Assert.Equal(3, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.NotNull(sut4);
            Assert.NotNull(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
            Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
        }

        [Fact]
        public void Add_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetimeAndOptions()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.Add<FakeService, FakeServiceScoped, FakeServiceScopedOptions>(sp => new FakeServiceScoped(sp.GetRequiredService<IOptions<FakeServiceScopedOptions>>()), ServiceLifetime.Scoped, o => o.Greeting = "Hejsa!")
                .Add<FakeService, FakeServiceSingleton, FakeServiceSingletonOptions>(sp => new FakeServiceSingleton(sp.GetRequiredService<IOptions<FakeServiceSingletonOptions>>()), ServiceLifetime.Singleton, o => o.Greeting = "Hello!")
                .Add<FakeService, FakeServiceTransient, FakeServiceTransientOptions>(sp => new FakeServiceTransient(sp.GetRequiredService<IOptions<FakeServiceTransientOptions>>()), ServiceLifetime.Transient, o => o.Greeting = "Aloha!");
            var sut3 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
            var sut4 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
            var sut5 = sut2.Single(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));
            var sut6 = sut2.BuildServiceProvider();
            var sut7 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceScoped));
            var sut8 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceSingleton));
            var sut9 = sut6.GetServices<FakeService>().Single(fs => fs.GetType() == typeof(FakeServiceTransient));

            Assert.Equal(11, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.NotNull(sut4);
            Assert.NotNull(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
            Assert.Equal(ServiceLifetime.Singleton, sut4.Lifetime);
            Assert.Equal(ServiceLifetime.Transient, sut5.Lifetime);
            Assert.Equal("Hejsa!", sut7.Greeting);
            Assert.Equal("Hello!", sut8.Greeting);
            Assert.Equal("Aloha!", sut9.Greeting);
        }

        [Fact]
        public void TryAdd_ShouldAddServiceToServiceCollectionWithSpecifiedLifetime()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped>(ServiceLifetime.Scoped)
                .TryAdd<FakeService, FakeServiceSingleton>(ServiceLifetime.Singleton)
                .TryAdd<FakeService, FakeServiceTransient>(ServiceLifetime.Transient);
            var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceScoped));
            var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
            var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceTransient));

            Assert.Equal(1, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.Null(sut4);
            Assert.Null(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        }

        [Fact]
        public void TryAdd_ShouldAddServiceToServiceCollectionWithSpecifiedLifetimeAndOptions()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped, FakeServiceScopedOptions>(ServiceLifetime.Scoped, o => o.Greeting = "Hejsa!")
                .TryAdd<FakeService, FakeServiceSingleton, FakeServiceSingletonOptions>(ServiceLifetime.Singleton, o => o.Greeting = "Hello!")
                .TryAdd<FakeService, FakeServiceTransient, FakeServiceTransientOptions>(ServiceLifetime.Transient, o => o.Greeting = "Aloha!");
            var sut3 = sut2.Single(sd => sd.ImplementationType == typeof(FakeServiceScoped));
            var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceSingleton));
            var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationType == typeof(FakeServiceTransient));
            var sut6 = sut2.BuildServiceProvider();
            var sut7 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceScoped));
            var sut8 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceSingleton));
            var sut9 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceTransient));

            Assert.Equal(9, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.Null(sut4);
            Assert.Null(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
            Assert.Equal("Hejsa!", sut7.Greeting);
            Assert.Null(sut8);
            Assert.Null(sut9);
        }

        [Fact]
        public void TryAdd_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetime()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped>(_ => new FakeServiceScoped(default), ServiceLifetime.Scoped)
                .TryAdd<FakeService, FakeServiceSingleton>(_ => new FakeServiceSingleton(default), ServiceLifetime.Singleton)
                .TryAdd<FakeService, FakeServiceTransient>(_ => new FakeServiceTransient(default), ServiceLifetime.Transient);
            var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
            var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
            var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));

            Assert.Equal(1, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.Null(sut4);
            Assert.Null(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
        }

        [Fact]
        public void TryAdd_ShouldAddServiceToServiceCollectionUsingFactoryWithSpecifiedLifetimeAndOptions()
        {
            var sut1 = new ServiceCollection();
            var sut2 = sut1.TryAdd<FakeService, FakeServiceScoped, FakeServiceScopedOptions>(sp => new FakeServiceScoped(sp.GetRequiredService<IOptions<FakeServiceScopedOptions>>()), ServiceLifetime.Scoped, o => o.Greeting = "Hejsa!")
                .TryAdd<FakeService, FakeServiceSingleton, FakeServiceSingletonOptions>(sp => new FakeServiceSingleton(sp.GetRequiredService<IOptions<FakeServiceSingletonOptions>>()), ServiceLifetime.Singleton, o => o.Greeting = "Hello!")
                .TryAdd<FakeService, FakeServiceTransient, FakeServiceTransientOptions>(sp => new FakeServiceTransient(sp.GetRequiredService<IOptions<FakeServiceTransientOptions>>()), ServiceLifetime.Transient, o => o.Greeting = "Aloha!");
            var sut3 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceScoped));
            var sut4 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceSingleton));
            var sut5 = sut2.SingleOrDefault(sd => sd.ImplementationFactory?.Method.ReturnType == typeof(FakeServiceTransient));
            var sut6 = sut2.BuildServiceProvider();
            var sut7 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceScoped));
            var sut8 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceSingleton));
            var sut9 = sut6.GetServices<FakeService>().SingleOrDefault(fs => fs.GetType() == typeof(FakeServiceTransient));

            Assert.Equal(9, sut1.Count);
            Assert.Equal(sut1, sut2);
            Assert.NotNull(sut3);
            Assert.Null(sut4);
            Assert.Null(sut5);
            Assert.Equal(ServiceLifetime.Scoped, sut3.Lifetime);
            Assert.Equal("Hejsa!", sut7.Greeting);
            Assert.Null(sut8);
            Assert.Null(sut9);
        }
    }
}