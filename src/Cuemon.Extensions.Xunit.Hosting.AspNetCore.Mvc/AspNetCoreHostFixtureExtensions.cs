namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    internal static class AspNetCoreHostFixtureExtensions
    {
        internal static bool HasValidState(this IAspNetCoreHostFixture fixture)
        {
            return fixture.ConfigureServicesCallback != null && fixture.Host != null && fixture.ServiceProvider != null && fixture.Application != null && fixture.ConfigureHostCallback != null;
        }
    }
}