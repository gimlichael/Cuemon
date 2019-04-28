using System;
using System.IO;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Configuration;
using Cuemon.Extensions;
using Cuemon.Extensions.Threading.Tasks;
using Cuemon.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// A filter that computes the response body and applies an appropriate HTTP Etag header.
    /// </summary>
    /// <seealso cref="HttpCacheableFilter" />
    public class HttpEntityTagHeader : IConfigurable<HttpEntityTagHeaderOptions>, ICacheableAsyncResultFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpEntityTagHeader"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="HttpEntityTagHeaderOptions" /> which need to be configured.</param>
        public HttpEntityTagHeader(Action<HttpEntityTagHeaderOptions> setup)
        {
            Options = setup.Configure();
        }

        /// <summary>
        /// Called asynchronously before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ResultExecutingContext" />.</param>
        /// <param name="next">The <see cref="ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var useFallbackToEntityTagResponseParser = true;
            if (Options.HasEntityTagProvider &&
                context.HttpContext.Request.IsGetOrHeadMethod() &&
                (context.HttpContext.Response.IsSuccessStatusCode() || context.HttpContext.Response.IsNotModifiedStatusCode()))
            {
                if (context.Result is ObjectResult result && result.Value is ICacheableIntegrity integrity && integrity.HasChecksum)
                {
                    useFallbackToEntityTagResponseParser = false;
                    Options.EntityTagProvider.Invoke(integrity, context.HttpContext);
                }
            }

            if (useFallbackToEntityTagResponseParser &&
                Options.HasEntityTagResponseParser &&
                Options.UseEntityTagResponseParser &&
                context.HttpContext.Request.IsGetOrHeadMethod() &&
                (context.HttpContext.Response.IsSuccessStatusCode() || context.HttpContext.Response.IsNotModifiedStatusCode()))
            {
                var statusCodeBeforeBodyRead = context.HttpContext.Response.StatusCode;
                object originalValue = null;
                if (context.Result is ObjectResult result)
                {
                    if (result.Value is ICacheableObjectResult cacheableObjectResult)
                    {
                        originalValue = result.Value;
                        result.Value = cacheableObjectResult.Value;
                    }
                    await InvokeEntityTagResponseParser(context, next, statusCodeBeforeBodyRead);
                    result.Value = originalValue;
                }
                else
                {
                    await InvokeEntityTagResponseParser(context, next, statusCodeBeforeBodyRead);
                }
            }
        }

        private async Task InvokeEntityTagResponseParser(ResultExecutingContext context, ResultExecutionDelegate next, int statusCodeBeforeBodyRead)
        {
            using (var ms = new MemoryStream())
            {
                var body = context.HttpContext.Response.Body;
                context.HttpContext.Response.Body = ms;
                await next().ContinueWithSuppressedContext(); 
                ms.Seek(0, SeekOrigin.Begin);

                if (statusCodeBeforeBodyRead == StatusCodes.Status304NotModified) { context.HttpContext.Response.StatusCode = statusCodeBeforeBodyRead; }

                Options.EntityTagResponseParser.Invoke(ms, context.HttpContext.Request, context.HttpContext.Response);
                if (context.HttpContext.Response.IsSuccessStatusCode()) { await ms.CopyToAsync(body).ContinueWithSuppressedContext(); }
            }
        }

        /// <summary>
        /// Gets the configured options of this instance.
        /// </summary>
        /// <value>The configured options of this instance.</value>
        public HttpEntityTagHeaderOptions Options { get; }
    }
}