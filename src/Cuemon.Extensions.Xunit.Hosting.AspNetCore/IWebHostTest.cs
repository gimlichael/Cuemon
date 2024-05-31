namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core (including but not limited to MVC, Razor and related) testing.
    /// </summary>
    /// <seealso cref="IGenericHostTest"/>
    /// <seealso cref="IPipelineTest" />
    /// <seealso cref="IHostTest" />
    public interface IWebHostTest : IGenericHostTest, IPipelineTest, IHostTest
    {
    }
}
