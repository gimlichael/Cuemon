using System;
using Cuemon.Security.Cryptography;

namespace Cuemon.Security.Web
{
    /// <summary>
    /// This utility class is designed to make <see cref="JsonWebTokenHashAlgorithm"/> related conversions easier to work with.
    /// </summary>
    public static class JsonWebTokenHashAlgorithmConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to its JWT <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="value">The JSON Web Token hash algorithm to be converted.</param>
        /// <returns>A JWT <see cref="string"/> that is equivalent to <paramref name="value"/>.</returns>
        public static string ToString(JsonWebTokenHashAlgorithm value)
        {
            switch (value)
            {
                case JsonWebTokenHashAlgorithm.None:
                    return "none";
                case JsonWebTokenHashAlgorithm.SHA256:
                    return "HS256";
            }
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to its <see cref="HmacAlgorithmType"/> equivalent.
        /// </summary>
        /// <param name="value">The JSON Web Token hash algorithm to be converted.</param>
        /// <returns>A <see cref="HmacAlgorithmType"/> that is equivalent to <paramref name="value"/>.</returns>
        public static HmacAlgorithmType ToHmacAlgorithm(JsonWebTokenHashAlgorithm value)
        {
            switch (value)
            {
                case JsonWebTokenHashAlgorithm.SHA256:
                    return HmacAlgorithmType.SHA256;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, "The specified value is not supported by this converter.");
            }
        }
    }
}