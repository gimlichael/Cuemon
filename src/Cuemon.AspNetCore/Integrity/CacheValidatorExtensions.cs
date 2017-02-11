using System;
using System.Globalization;
using Cuemon.Collections.Generic;
using Cuemon.Integrity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Integrity
{
    /// <summary>
    /// Extension methods for the <see cref="CacheValidator"/> class.
    /// </summary>
    public static class CacheValidatorExtensions
    {
        /// <summary>
        /// Creates an <see cref="EntityTagHeaderValue"/> from the specified <paramref name="validator"/>.
        /// </summary>
        /// <param name="validator">The validator to extend.</param>
        /// <returns>An <see cref="EntityTagHeaderValue"/> that is initiated with a hexadecimal representation of <see cref="CacheValidator.Checksum"/> and a value that indicates if the tag is weak.</returns>
        public static EntityTagHeaderValue ToEntityTag(this CacheValidator validator)
        {
            Validator.ThrowIfNull(validator, nameof(validator));
            return new EntityTagHeaderValue(string.Concat("\"", validator.Checksum.ToHexadecimal(), "\""), validator.Strength != ChecksumStrength.Strong);
        }

        /// <summary>
        /// This method will either add or update the necessary HTTP response headers needed to provide entity tag header information.
        /// </summary>
        /// <param name="validator">The validator to extend.</param>
        /// <param name="request">An instance of the <see cref="HttpRequest"/> object.</param>
        /// <param name="response">An instance of the <see cref="HttpResponse"/> object.</param>
        public static void SetEntityTagHeaderInformation(this CacheValidator validator, HttpRequest request, HttpResponse response)
        {
            Validator.ThrowIfNull(validator, nameof(validator));
            Validator.ThrowIfNull(request, nameof(request));
            var accept = request.Headers["Accept"];
            validator = validator.CombineWith(accept);
            if (validator.Strength != ChecksumStrength.None)
            {
                DateTime utcNow = DateTime.UtcNow;
                if (request.IsClientSideResourceCached(validator))
                {
                    response.StatusCode = StatusCodes.Status304NotModified;
                }
                else
                {
                    response.Headers.AddOrUpdate(HeaderNames.LastModified, new StringValues(utcNow.ToString("R", DateTimeFormatInfo.InvariantInfo)));
                }
                response.Headers.AddOrUpdate(HeaderNames.ETag, new StringValues(validator.ToEntityTag().ToString()));
            }
        }
    }
}