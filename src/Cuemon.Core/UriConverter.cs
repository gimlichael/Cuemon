using System;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="Uri"/> related conversions easier to work with.
    /// </summary>
    public static class UriConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> of a protocol-relative <see cref="string"/> to its equivalent <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to be converted.</param>
        /// <param name="protocol">The desired protocol of the <paramref name="value"/> to be converted.</param>
        /// <returns>An <see cref="Uri"/> that is equivalent to <paramref name="value"/> with the specified <paramref name="protocol"/> as <see cref="Uri.Scheme"/>.</returns>
        public static Uri FromProtocolRelativeUri(string value, UriScheme protocol)
        {
            return FromProtocolRelativeUri(value, protocol, StringUtility.NetworkPathReference);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> of a protocol-relative <see cref="string"/> to its equivalent <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to be converted.</param>
        /// <param name="protocol">The desired protocol of the <paramref name="value"/> to be converted.</param>
        /// <param name="relativeReference">The relative reference that <paramref name="value"/> must begin with. Default is <see cref="StringUtility.NetworkPathReference"/>.</param>
        /// <returns>An <see cref="Uri"/> that is equivalent to <paramref name="value"/> with the specified <paramref name="protocol"/> as <see cref="Uri.Scheme"/>.</returns>
        public static Uri FromProtocolRelativeUri(string value, UriScheme protocol, string relativeReference)
        {
            Validator.ThrowIfNullOrEmpty(value, nameof(value));
            Validator.ThrowIfNullOrEmpty(relativeReference, nameof(relativeReference));
            Validator.ThrowIfFalse(value.StartsWith(relativeReference, StringComparison.OrdinalIgnoreCase), nameof(value), string.Format(CultureInfo.InvariantCulture, "The specified value did not start with the the expected value of: {0}.", relativeReference));
            int relativeReferenceLength = relativeReference.Length;
            return new Uri(value.Remove(0, relativeReferenceLength).Insert(0, string.Format(CultureInfo.InvariantCulture, "{0}://", StringConverter.FromUriScheme(protocol))));
        }
    }
}