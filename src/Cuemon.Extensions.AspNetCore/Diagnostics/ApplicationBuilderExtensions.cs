using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Diagnostics;
using Cuemon.Extensions.AspNetCore.Text.Yaml.Converters;
using Cuemon.Net.Http;
using Cuemon.Text.Yaml.Formatters;
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
        /// <remarks>Extends the existing <see cref="ExceptionHandlerMiddleware"/> to include features provided by Cuemon.</remarks>
        public static IApplicationBuilder UseFaultDescriptorExceptionHandler(this IApplicationBuilder builder, Action<FaultDescriptorOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            var handlerOptions = new ExceptionHandlerOptions()
            {
                ExceptionHandler = async context =>
                {
                    var ehf = context.Features.Get<IExceptionHandlerFeature>();
                    if (ehf != null)
                    {
                        var exceptionDescriptor = options.ExceptionDescriptorResolver?.Invoke(options.UseBaseException ? ehf.Error.GetBaseException() : ehf.Error);
                        if (exceptionDescriptor == null) { return; }
                        //exceptionDescriptor.PostInitializeWith(actionDescriptor.MethodInfo.GetCustomAttributes<ExceptionDescriptorAttribute>());
                        if (options.HasRootHelpLink && exceptionDescriptor.HelpLink == null) { exceptionDescriptor.HelpLink = options.RootHelpLink; }
                        if (context.Items.TryGetValue(RequestIdentifierMiddleware.HttpContextItemsKey, out var requestId))
                        {
                            if (requestId != null) exceptionDescriptor.RequestId = requestId.ToString();
                        }
                        if (context.Items.TryGetValue(CorrelationIdentifierMiddleware.HttpContextItemsKey, out var correlationId))
                        {
                            if (correlationId != null) exceptionDescriptor.CorrelationId = correlationId.ToString();
                        }
                        options.ExceptionCallback?.Invoke(context, ehf.Error, exceptionDescriptor);
                        //if (Options.MarkExceptionHandled) { context.ExceptionHandled = true; }
                        if (options.IncludeRequest) { exceptionDescriptor.AddEvidence("Request", context.Request, request => new HttpRequestEvidence(request, options.RequestBodyParser)); }
                        // Options.ExceptionDescriptorHandler?.Invoke(context, exceptionDescriptor);
                        if (ehf.Error is HttpStatusCodeException httpFault)
                        {
                            Decorator.Enclose(context.Response.Headers).AddRange(httpFault.Headers);
                        }
                        var handlers = (options.NonMvcResponseHandler?.Invoke(context, exceptionDescriptor) ?? Enumerable.Empty<HttpExceptionDescriptorResponseHandler>()).ToList();
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
                                    await WriteResponseAsync(context, handler, options.CancellationToken).ConfigureAwait(false);
                                    return;
                                }
                            }
                        }

                        await WriteResponseAsync(context, GetFallbackHandler(exceptionDescriptor, Patterns.ConfigureExchange<FaultDescriptorOptions, ExceptionDescriptorOptions>(setup)), options.CancellationToken).ConfigureAwait(false);
                    }
                }
            };
            return builder.UseExceptionHandler(handlerOptions);
        }

        private static HttpExceptionDescriptorResponseHandler GetFallbackHandler(HttpExceptionDescriptor descriptor, Action<ExceptionDescriptorOptions> setup)
        {
            return new HttpExceptionDescriptorResponseHandler(descriptor, MediaTypeHeaderValue.Parse("text/plain"), (ed, mt) =>
            {
                return new HttpResponseMessage()
                {
                    Content = new StreamContent(YamlFormatter.SerializeObject(ed, setup: yfo =>
                    {
                        yfo.Settings.Converters.AddHttpExceptionDescriptorConverter(setup);
                    }))
                    {
                        Headers = { { HttpHeaderNames.ContentType, mt.MediaType } }
                    },
                    StatusCode = (HttpStatusCode)descriptor.StatusCode
                };
            });
        }

        private static async Task WriteResponseAsync(HttpContext context, HttpExceptionDescriptorResponseHandler handler, CancellationToken ct)
        {
            var message = handler.ToHttpResponseMessage();
#if NET6_0_OR_GREATER
            var buffer = await message.Content.ReadAsByteArrayAsync(ct).ConfigureAwait(false);
#else
            var buffer = await message.Content.ReadAsByteArrayAsync().ConfigureAwait(false);
#endif
            context.Response.ContentType = message.Content.Headers.ContentType!.ToString();
            context.Response.ContentLength = buffer.Length;
            context.Response.StatusCode = (int)message.StatusCode;
            await context.Response.Body.WriteAsync(buffer, 0, buffer.Length, ct).ConfigureAwait(false);
        }
    }
}
