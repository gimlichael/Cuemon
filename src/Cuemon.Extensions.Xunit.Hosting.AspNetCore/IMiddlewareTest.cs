using System;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core middleware testing.
    /// </summary>
    /// <seealso cref="IServiceTest" />
    /// <seealso cref="IPipelineTest" />
    /// <seealso cref="IDisposable" />
    public interface IMiddlewareTest : IServiceTest, IPipelineTest, IDisposable
    {
    }
}