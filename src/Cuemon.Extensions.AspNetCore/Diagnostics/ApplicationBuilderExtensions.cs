using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Diagnostics;
using Cuemon.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

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
            return MiddlewareBuilderFactory.UseMiddleware<ServerTimingMiddleware>(builder);
        }

        /// <summary>
        /// Adds a middleware to the pipeline that will catch exceptions, log them, and re-execute the request in an alternate pipeline. The request will not be re-executed if the response has already started.
        /// </summary>
        /// <param name="builder">The type that provides the mechanisms to configure an application’s request pipeline.</param>
        /// <param name="setup">The <see cref="FaultDescriptorOptions"/> which may be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>Extends the existing <see cref="ExceptionHandlerMiddleware"/> to include features similar to those provided by Cuemon.AspNetCore.Mvc.Filters.Diagnostics.FaultDescriptorFilter, except:<br/>
        /// 1. Unable to interact with controller applied attributes (outside scope; part of MVC context)<br/>
        /// 2. Unable to mark an exception as handled (outside scope; part of MVC context)
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <see cref="FaultDescriptorOptions"/> in a valid state.
        /// </exception>
        public static IApplicationBuilder UseFaultDescriptorExceptionHandler(this IApplicationBuilder builder, Action<FaultDescriptorOptions> setup = null)
        {
            Validator.ThrowIfInvalidConfigurator(setup, out var options);
            var handlerOptions = new ExceptionHandlerOptions()
            {
                ExceptionHandler = async context =>
                {
                    var ehf = context.Features.Get<IExceptionHandlerFeature>();
                    if (ehf != null)
                    {
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

                        var fallback = new List<HttpExceptionDescriptorResponseHandler>().AddYamlResponseHandler(Patterns.ConfigureExchange<FaultDescriptorOptions, ExceptionDescriptorOptions>(setup));
                        var handlers = options.NonMvcResponseHandlers ?? fallback;
                        if (handlers.Count > 0)
                        {
                            var accepts = context.Request.Headers[HttpHeaderNames.Accept]
                                .OrderByDescending(accept =>
                                {
                                    var values = accept.Split(';').Select(raw => raw.Trim());
                                    return values.FirstOrDefault(quality => quality.StartsWith("q=")) ?? "q=0.0";
                                })
                                .Select(accept => accept.Split(';').First())
                                .ToList();

                            foreach (var accept in accepts)
                            {
                                var handler = handlers.FirstOrDefault(rh => rh.ContentType.MediaType != null && rh.ContentType.MediaType.Equals(accept, StringComparison.OrdinalIgnoreCase));
                                if (handler != null)
                                {
                                    await WriteResponseAsync(context, handler, exceptionDescriptor, options.CancellationToken).ConfigureAwait(false);
                                    return;
                                }
                            }
                        }
                        await WriteResponseAsync(context, fallback.First(), exceptionDescriptor, options.CancellationToken).ConfigureAwait(false);
                    }
                }
            };
            return builder.UseExceptionHandler(handlerOptions);
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpExceptionDescriptorResponseHandler handler, HttpExceptionDescriptor exceptionDescriptor, CancellationToken ct)
        {
            var message = handler.ToHttpResponseMessage(exceptionDescriptor);
            var buffer = await message.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);
            context.Response.ContentType = message.Content.Headers.ContentType!.ToString();
            context.Response.ContentLength = buffer.Length;
            context.Response.StatusCode = (int)message.StatusCode;
            await context.Response.Body.WriteAsync(buffer, 0, buffer.Length, ct).ConfigureAwait(false);
        }
    }
}
