using System;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core middleware testing.
    /// </summary>
    /// <seealso cref="IGenericHostTest" />
    /// <seealso cref="IPipelineTest" />
    [Obsolete("This interface is obsolete and will be removed in a future version. Please use IWebHostTest instead.")]
    public interface IMiddlewareTest : IGenericHostTest, IPipelineTest
    {
    }
}
