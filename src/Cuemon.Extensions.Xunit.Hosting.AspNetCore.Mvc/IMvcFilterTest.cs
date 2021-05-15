using System;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core MVC filter testing.
    /// </summary>
    /// <seealso cref="IMiddlewareTest" />
    /// <seealso cref="IHostTest" />
    [Obsolete("This interface is deprecated and will be removed soon. Please use IWebApplicationTest instead.")]
    public interface IMvcFilterTest : IWebApplicationTest
    {
    }
}
