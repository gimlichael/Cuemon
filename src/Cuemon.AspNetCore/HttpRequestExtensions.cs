using System;
using System.Linq;
using Cuemon.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;

namespace Cuemon.AspNetCore
{
    /// <summary>
    /// Extension methods for the <see cref="HttpRequest"/> class.
    /// </summary>
    public static class HttpRequestExtensions
    {
        /// <summary>
        /// Determines whether a cached version of the requested resource is found client-side given the specified <paramref name="validator"/>.
        /// </summary>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <param name="validator">A <see cref="CacheValidator"/> object that represents the content validation of the resource.</param>
        /// <returns>
        ///     <c>true</c> if a cached version of the requested content is found client-side given the specified <paramref name="validator"/>; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsClientSideResourceCached(this HttpRequest request, CacheValidator validator)
        {
            Validator.ThrowIfNull(request, nameof(request));
            Validator.ThrowIfNull(validator, nameof(validator));
            var headers = new RequestHeaders(request.Headers);
            var validatorLastModified = validator.GetMostSignificant().Adjust(o => new DateTime(o.Year, o.Month, o.Day, o.Hour, o.Minute, o.Second, DateTimeKind.Utc)); // make sure, that modified has the same format as the if-modified-since header
            var ifModifiedSince = headers.IfModifiedSince;
            var isClientSideContentCached = (validatorLastModified != DateTime.MinValue) && (ifModifiedSince.HasValue && ifModifiedSince.Value.ToUniversalTime() >= validatorLastModified);
            if (isClientSideContentCached && validator.Strength != ChecksumStrength.None && headers.IfNoneMatch != null)
            {
                var clientSideEntityTagHeader = headers.IfNoneMatch.FirstOrDefault();
                var clientSideEntityTag = clientSideEntityTagHeader == null ? "" : clientSideEntityTagHeader.Tag;
                int indexOfStartQuote = clientSideEntityTag.IndexOf('"');
                int indexOfEndQuote = clientSideEntityTag.LastIndexOf('"');
                if (indexOfStartQuote == 0 &&
                    (indexOfEndQuote > 2 && indexOfEndQuote == clientSideEntityTag.Length - 1))
                {
                    clientSideEntityTag = clientSideEntityTag.Remove(indexOfEndQuote, 1);
                    clientSideEntityTag = clientSideEntityTag.Remove(indexOfStartQuote, 1);
                }
                isClientSideContentCached = validator.Checksum.Equals(clientSideEntityTag);
            }
            return isClientSideContentCached;
        }
    }
}