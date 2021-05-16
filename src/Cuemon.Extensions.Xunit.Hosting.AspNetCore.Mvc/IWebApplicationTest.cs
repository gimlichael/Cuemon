namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core MVC, Razor and related testing.
    /// </summary>
    /// <seealso cref="IMiddlewareTest" />
    /// <seealso cref="IHostTest" />
    public interface IWebApplicationTest : IMiddlewareTest, IHostTest
    {
    }
}