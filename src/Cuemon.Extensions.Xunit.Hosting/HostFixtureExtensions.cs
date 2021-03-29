namespace Cuemon.Extensions.Xunit.Hosting
{
    internal static class HostFixtureExtensions
    {
        internal static bool HasValidState(this IHostFixture fixture)
        {
            return fixture.ConfigureServicesCallback != null && fixture.Host != null && fixture.ServiceProvider != null && fixture.ConfigureHostCallback != null;
        }
    }
}