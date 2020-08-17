using System;
using System.Linq;
using Cuemon.AspNetCore.Http;
using Cuemon.Data.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace Cuemon.Extensions.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for the <see cref="HttpRequest"/> class.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="request"/> is served by either a GET or a HEAD method.
        /// </summary>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <returns><c>true</c> if the specified <paramref name="request"/> is served by either a GET or a HEAD method; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="request"/> cannot be null.
        /// </exception>
        public static bool IsGetOrHeadMethod(this HttpRequest request)
        {
            Validator.ThrowIfNull(request, nameof(request));
            return Decorator.Enclose(request).IsGetOrHeadMethod();
        }

        /// <summary>
        /// Determines whether a cached version of the requested resource is found client-side using the If-None-Match HTTP header.
        /// </summary>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <param name="builder">A <see cref="ChecksumBuilder"/> that represents the integrity of the client.</param>
        /// <returns>
        ///     <c>true</c> if a cached version of the requested content is found client-side; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsClientSideResourceCached(this HttpRequest request, ChecksumBuilder builder)
        {
            Validator.ThrowIfNull(request, nameof(request));
            var headers = new RequestHeaders(request.Headers);
            if (headers.IfNoneMatch != null)
            {
                var clientSideEntityTagHeader = headers.IfNoneMatch.FirstOrDefault();
                var clientSideEntityTag = clientSideEntityTagHeader == null ? "" : clientSideEntityTagHeader.Tag.Value;
                int indexOfStartQuote = clientSideEntityTag.IndexOf('"');
                int indexOfEndQuote = clientSideEntityTag.LastIndexOf('"');
                if (indexOfStartQuote == 0 &&
                    (indexOfEndQuote > 2 && indexOfEndQuote == clientSideEntityTag.Length - 1))
                {
                    clientSideEntityTag = clientSideEntityTag.Remove(indexOfEndQuote, 1);
                    clientSideEntityTag = clientSideEntityTag.Remove(indexOfStartQuote, 1);
                }
                return builder.Checksum.ToHexadecimalString().Equals(clientSideEntityTag);
            }
            return false;
        }

        /// <summary>
        /// Determines whether a cached version of the requested resource is found client-side using the If-Modified-Since HTTP header.
        /// </summary>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <param name="lastModified">A <see cref="DateTime"/> value that represents the modification date of the content.</param>
        /// <returns>
        ///     <c>true</c> if a cached version of the requested content is found client-side; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsClientSideResourceCached(this HttpRequest request, DateTime lastModified)
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(lastModified, nameof(lastModified));
            var headers = new RequestHeaders(request.Headers);
            var adjustedLastModified = Decorator.Enclose(lastModified).Adjust(o => new DateTime(o.Year, o.Month, o.Day, o.Hour, o.Minute, o.Second, DateTimeKind.Utc)); // make sure, that modified has the same format as the if-modified-since header
            var ifModifiedSince = headers.IfModifiedSince?.UtcDateTime;
            return (adjustedLastModified != DateTime.MinValue) && (ifModifiedSince.HasValue && ifModifiedSince.Value.ToUniversalTime() >= adjustedLastModified);
        }
    }
}