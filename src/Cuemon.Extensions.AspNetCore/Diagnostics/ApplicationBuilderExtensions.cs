using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Builder;
using Cuemon.AspNetCore.Diagnostics;
using Cuemon.AspNetCore.Http;
using Cuemon.AspNetCore.Http.Headers;
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
        /// <param name="setup">The <see cref="FaultDescriptorExceptionHandlerOptions"/> which may be configured.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        /// <remarks>Extends the existing <see cref="ExceptionHandlerMiddleware"/> to include features provided by Cuemon.</remarks>
        public static IApplicationBuilder UseFaultDescriptorExceptionHandler(this IApplicationBuilder builder, Action<FaultDescriptorExceptionHandlerOptions> setup = null)
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

                        var sb = new StringBuilder($"{exceptionDescriptor.Code}: {exceptionDescriptor.Message}").AppendLine();

                        if (options.HasRootHelpLink && exceptionDescriptor.HelpLink == null) { exceptionDescriptor.HelpLink = options.RootHelpLink; }

                        if (exceptionDescriptor.HelpLink != null)
                        {
                            sb.AppendLine($"{nameof(exceptionDescriptor.HelpLink)}: {exceptionDescriptor.HelpLink}");
                        }

                        if (context.Items.TryGetValue(RequestIdentifierMiddleware.HttpContextItemsKey, out var requestId))
                        {
                            sb.AppendLine($"{nameof(exceptionDescriptor.RequestId)}: {requestId}");
                            exceptionDescriptor.RequestId = requestId.ToString();
                        }

                        if (context.Items.TryGetValue(CorrelationIdentifierMiddleware.HttpContextItemsKey, out var correlationId))
                        {
                            sb.AppendLine($"{nameof(exceptionDescriptor.CorrelationId)}: {correlationId}");
                            exceptionDescriptor.CorrelationId = correlationId.ToString();
                        }

                        options.ExceptionCallback?.Invoke(context, ehf.Error, exceptionDescriptor);

                        await WriteResponseAsync(sb, context, exceptionDescriptor, ehf, options.CancellationToken).ConfigureAwait(false);
                    }
                }
            };
            return builder.UseExceptionHandler(handlerOptions);
        }

        private static Task WriteResponseAsync(StringBuilder sb, HttpContext context, HttpExceptionDescriptor exceptionDescriptor, IExceptionHandlerFeature ehf, CancellationToken ct)
        {
            var buffer = Decorator.Enclose(sb.ToString().TrimEnd()).ToByteArray();
            context.Response.ContentType = "text/plain";
            context.Response.ContentLength = buffer.Length;
            context.Response.StatusCode = exceptionDescriptor.StatusCode;
            if (ehf.Error is HttpStatusCodeException httpFault)
            {
                Decorator.Enclose(context.Response.Headers).AddRange(httpFault.Headers);
            }
            return context.Response.Body.WriteAsync(buffer, 0, buffer.Length, ct);
        }
    }
}
