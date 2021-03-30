using System;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core MVC filter testing.
    /// </summary>
    /// <seealso cref="IMiddlewareTest" />
    /// <seealso cref="IHostTest" />
    public interface IMvcFilterTest : IMiddlewareTest, IHostTest
    {
    }
}