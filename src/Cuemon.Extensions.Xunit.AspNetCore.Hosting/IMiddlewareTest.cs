namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core middleware testing.
    /// </summary>
    /// <seealso cref="IServiceTest" />
    /// <seealso cref="IPipelineTest" />
    public interface IMiddlewareTest : IServiceTest, IPipelineTest
    {
    }
}