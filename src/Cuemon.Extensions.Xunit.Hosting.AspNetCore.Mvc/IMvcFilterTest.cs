using System;

namespace Cuemon.Extensions.Xunit.Hosting.AspNetCore.Mvc
{
    /// <summary>
    /// Represents the members needed for ASP.NET Core MVC filter testing.
    /// </summary>
    /// <seealso cref="IServiceTest" />
    /// <seealso cref="IPipelineTest" />
    /// <seealso cref="IHostTest" />
    /// <seealso cref="IDisposable" />
    public interface IMvcFilterTest : IServiceTest, IPipelineTest, IHostTest, IDisposable
    {
    }
}