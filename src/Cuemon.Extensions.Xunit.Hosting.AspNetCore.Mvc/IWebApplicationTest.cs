using System;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core MVC, Razor and related testing.
    /// </summary>
    /// <seealso cref="IMiddlewareTest" />
    /// <seealso cref="IHostTest" />
    [Obsolete("This interface is obsolete and will be removed in a future version. Please use IWebHostTest instead (located in Cuemon.Extensions.Xunit.Hosting.AspNetCore).")]
    public interface IWebApplicationTest : IMiddlewareTest, IHostTest
    {
    }
}
