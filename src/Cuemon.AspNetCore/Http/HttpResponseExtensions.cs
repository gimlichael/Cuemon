using System;
using System.Globalization;
using Cuemon.AspNetCore.Integrity;
using Cuemon.Collections.Generic;
using Cuemon.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for the <see cref="HttpResponse"/> class.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Determines whether the HTTP response was successful.
        /// </summary>
        /// <param name="response">An instance of the <see cref="HttpResponse"/> object.</param>
        /// <returns><c>true</c> if <see cref="HttpResponse.StatusCode"/> was in the <b>Successful</b> range (200-299); otherwise, <c>false</c>.</returns>
        public static bool IsSuccessStatusCode(this HttpResponse response)
        {
            return (response.StatusCode >= StatusCodes.Status200OK && response.StatusCode <= 299);
        }

        /// <summary>
        /// Determines whether the HTTP response equals a 304 Not Modified.
        /// </summary>
        /// <param name="response">An instance of the <see cref="HttpResponse"/> object.</param>
        /// <returns><c>true</c> if <see cref="HttpResponse.StatusCode"/> is <b>NotModified</b> (304); otherwise, <c>false</c>.</returns>
        public static bool IsNotModifiedStatusCode(this HttpResponse response)
        {
            return (response.StatusCode == StatusCodes.Status304NotModified);
        }

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
            if (response.IsSuccessStatusCode() && request.IsClientSideResourceCached(builder)) { response.StatusCode = StatusCodes.Status304NotModified; }
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
            if (response.IsSuccessStatusCode() && request.IsClientSideResourceCached(lastModified)) { response.StatusCode = StatusCodes.Status304NotModified; }
            response.Headers.AddOrUpdate(HeaderNames.LastModified, new StringValues(lastModified.ToUniversalTime().ToString("R", DateTimeFormatInfo.InvariantInfo)));
        }
    }
}