using System;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Configuration;
using Cuemon.Integrity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// A filter that applies a HTTP Last-Modified header.
    /// </summary>
    /// <seealso cref="IAsyncResultFilter" />
    public class HttpLastModifiedHeader : IConfigurable<HttpLastModifiedHeaderOptions>, ICacheableAsyncResultFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpLastModifiedHeader"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="HttpLastModifiedHeaderOptions"/> which need to be configured.</param>
        public HttpLastModifiedHeader(Action<HttpLastModifiedHeaderOptions> setup)
        {
            Options = setup.ConfigureOptions();
        }

        /// <summary>
        /// Called asynchronously before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ResultExecutingContext" />.</param>
        /// <param name="next">The <see cref="ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (Options.HasLastModifiedProvider && 
                context.HttpContext.Request.IsGetOrHeadMethod() && 
                context.HttpContext.Response.IsSuccessStatusCode())
            {
                if (context.Result is ObjectResult result && result.Value is ICacheableTimestamp timestamp) { Options.LastModifiedProvider.Invoke(timestamp, context.HttpContext); }
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Gets the configured options of this instance.
        /// </summary>
        /// <value>The configured options of this instance.</value>
        public HttpLastModifiedHeaderOptions Options { get; }
    }
}