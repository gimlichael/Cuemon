using Cuemon.Integrity;
using Microsoft.Net.Http.Headers;

namespace Cuemon.AspNetCore.Integrity
{
    /// <summary>
    /// Extension methods for the <see cref="ChecksumBuilder"/> class.
    /// </summary>
    public static class ChecksumBuilderExtensions
    {
        /// <summary>
        /// Creates an <see cref="EntityTagHeaderValue"/> from the specified <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="ChecksumBuilder"/> to extend.</param>
        /// <param name="isWeak">A value that indicates if this entity-tag header is a weak validator.</param>
        /// <returns>An <see cref="EntityTagHeaderValue"/> that is initiated with a hexadecimal representation of <see cref="ChecksumBuilder.Checksum"/> and a value that indicates if the tag is weak.</returns>
        public static EntityTagHeaderValue ToEntityTag(this ChecksumBuilder builder, bool isWeak = false)
        {
            Validator.ThrowIfNull(builder, nameof(builder));
            return new EntityTagHeaderValue(string.Concat("\"", builder.Checksum.ToHexadecimal(), "\""), isWeak);
        }
    }
}