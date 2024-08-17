using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cuemon.Extensions.AspNetCore.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="IApplicationBuilder"/> interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds a Server-Timing HTTP header to the <see cref="IApplicationBuilder"/> request execution pipeline.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IApplicationBuilder UseServerTiming(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ServerTimingMiddleware>();
        }

        /// <summary>
        /// Adds a middleware to the pipeline that will catch exceptions, log them, and re-execute the request in an alternate pipeline. The request will not be re-executed if the response has already started.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>Extends the existing <see cref="ExceptionHandlerMiddleware"/> to include features similar to those provided by Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter, except:<br/>
        /// 1. Unable to interact with controller applied attributes (outside scope; part of MVC context)<br/>
        /// 2. Unable to mark an exception as handled (outside scope; part of MVC context)
        /// </remarks>
        public static IApplicationBuilder UseFaultDescriptorExceptionHandler(this IApplicationBuilder builder)
        {
            var handlerOptions = new ExceptionHandlerOptions()
            {
                ExceptionHandler = async context =>
                {
                    var ehf = context.Features.Get<IExceptionHandlerFeature>();
                    if (ehf != null)
                    {
						var faultDescriptorOptions = context.RequestServices.GetRequiredService<IOptions<FaultDescriptorOptions>>();
                        var nonMvcResponseHandlers = context.RequestServices.GetExceptionResponseFormatters().SelectExceptionDescriptorHandlers();
                        var options = faultDescriptorOptions.Value;
						var exceptionDescriptor = options.ExceptionDescriptorResolver?.Invoke(options.UseBaseException ? ehf.Error.GetBaseException() : ehf.Error);
                        if (exceptionDescriptor == null) { return; }
                        if (options.HasRootHelpLink && exceptionDescriptor.HelpLink == null) { exceptionDescriptor.HelpLink = options.RootHelpLink; }
                        if (context.Items.TryGetValue(RequestIdentifierMiddleware.HttpContextItemsKey, out var requestId) && requestId != null) { exceptionDescriptor.RequestId = requestId.ToString(); }
                        if (context.Items.TryGetValue(CorrelationIdentifierMiddleware.HttpContextItemsKey, out var correlationId) && correlationId != null) { exceptionDescriptor.CorrelationId = correlationId.ToString(); }
                        options.ExceptionCallback?.Invoke(context, ehf.Error, exceptionDescriptor);
                        if (options.RequestEvidenceProvider != null && options.SensitivityDetails.HasFlag(FaultSensitivityDetails.Evidence)) { exceptionDescriptor.AddEvidence("Request", context.Request, options.RequestEvidenceProvider); }
                        if (ehf.Error is HttpStatusCodeException httpFault)
                        {
                            Decorator.Enclose(context.Response.Headers).AddRange(httpFault.Headers);
                        }

                        var handlers = new List<HttpExceptionDescriptorResponseHandler>();
                        handlers = handlers.Concat(nonMvcResponseHandlers).ToList();

                        var accepts = context.Request.AcceptMimeTypesOrderedByQuality();

                        foreach (var accept in accepts)
                        {
                            var handler = handlers.FirstOrDefault(rh => rh.ContentType.MediaType != null && rh.ContentType.MediaType.Equals(accept, StringComparison.OrdinalIgnoreCase));
                            if (handler != null)
                            {
                                await WriteResponseAsync(context, handler, exceptionDescriptor, options.CancellationToken).ConfigureAwait(false);
                                return;
                            }
                        }

                        var fallback = HttpExceptionDescriptorResponseHandler.CreateDefaultFallbackHandler(context.RequestServices.GetRequiredService<IOptions<ExceptionDescriptorOptions>>().Value.SensitivityDetails);
                        await WriteResponseAsync(context, fallback, exceptionDescriptor, options.CancellationToken).ConfigureAwait(false); // fallback in case no match from Accept header
                    }
                }
            };
            return builder.UseExceptionHandler(handlerOptions);
        }

        private static Task WriteResponseAsync(HttpContext context, HttpExceptionDescriptorResponseHandler handler, HttpExceptionDescriptor exceptionDescriptor, CancellationToken ct)
        {
            return Decorator.Enclose(context).WriteExceptionDescriptorResponseAsync(handler, exceptionDescriptor, ct);
        }
    }
}
