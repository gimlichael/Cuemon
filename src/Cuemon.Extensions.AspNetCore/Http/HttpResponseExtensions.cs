using System;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.Extensions.Collections.Generic;
using Cuemon.Extensions.Integrity;
using Cuemon.Extensions.Threading.Tasks;
using Cuemon.Integrity;
using Cuemon.Extensions.AspNetCore.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Cuemon.Extensions.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for the <see cref="HttpResponse"/> class.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// This method will either add or update the necessary HTTP response headers needed to provide entity tag header information.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/> to extend.</param>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <param name="builder">A <see cref="ChecksumBuilder"/> that represents the integrity of the client.</param>
        /// <param name="isWeak">A value that indicates if this entity-tag header is a weak validator.</param>
        public static void SetEntityTagHeaderInformation(this HttpResponse response, HttpRequest request, ChecksumBuilder builder, bool isWeak = false)
        {
            Validator.ThrowIfNull(response, nameof(response));
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(builder, nameof(builder));
            builder = builder.CombineWith(request.Headers[HeaderNames.Accept]);
            if (response.StatusCode.IsSuccessStatusCode() && request.IsClientSideResourceCached(builder)) { response.StatusCode = StatusCodes.Status304NotModified; }
            response.Headers.AddOrUpdate(HeaderNames.ETag, new StringValues(builder.ToEntityTag(isWeak).ToString()));
        }

        /// <summary>
        /// This method will either add or update the necessary HTTP response headers needed to provide last-modified information.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/> to extend.</param>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <param name="lastModified">A value that represents when the resource was either created or last modified.</param>
        public static void SetLastModifiedHeaderInformation(this HttpResponse response, HttpRequest request, DateTime lastModified)
        {
            Validator.ThrowIfNull(response, nameof(response));
            Validator.ThrowIfNull(request, nameof(request));
            if (response.StatusCode.IsSuccessStatusCode() && request.IsClientSideResourceCached(lastModified)) { response.StatusCode = StatusCodes.Status304NotModified; }
            response.Headers.AddOrUpdate(HeaderNames.LastModified, new StringValues(lastModified.ToUniversalTime().ToString("R", DateTimeFormatInfo.InvariantInfo)));
        }

        /// <summary>
        /// Asynchronously writes a sequence of bytes to the response body stream and advances the current position within this stream by the number of bytes written.    
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/> to extend.</param>
        /// <param name="body">The function delegate that resolves the bytes to write.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static async Task WriteBodyAsync(this HttpResponse response, Func<byte[]> body)
        {
            var bodyContent = body?.Invoke();
            if (bodyContent != null)
            {
                await response.Body.WriteAsync(bodyContent, 0, bodyContent.Length).ContinueWithSuppressedContext();
            }
        }

        /// <summary>
        /// Transfers the specified <paramref name="message"/> to the HTTP <paramref name="response"/> pipeline using the <paramref name="transformer"/> delegate.
        /// </summary>
        /// <param name="response">The <see cref="HttpResponse"/> to extend.</param>
        /// <param name="message">The <see cref="HttpResponseMessage"/> to convert into an HTTP equivalent <see cref="HttpResponse"/>.</param>
        /// <param name="transformer">The delegate that converts a <see cref="HttpResponseMessage"/> to an HTTP equivalent <see cref="HttpResponse"/>.</param>
        public static void OnStartingInvokeTransformer(this HttpResponse response, HttpResponseMessage message, Action<HttpResponseMessage, HttpResponse> transformer)
        {
            Validator.ThrowIfNull(message, nameof(message));
            Validator.ThrowIfNull(response, nameof(response));
            response.OnStarting(() =>
            {
                transformer?.Invoke(message, response);
                return Task.CompletedTask;
            });
        }
    }
}