using System;

namespace Cuemon.Web.Security
{
    /// <summary>
    /// This utility class is designed to make JSON Web Token related conversions easier to work with.
    /// </summary>
    public static class JsonWebTokenConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to its JWT <see cref="string"/> equivalent.
        /// </summary>
        /// <param name="value">The JSON Web Token hash algorithm to be converted.</param>
        /// <returns>A JWT <see cref="string"/> that is equivalent to <paramref name="value"/>.</returns>
        public static string FromJsonWebTokenHashAlgorithm(JsonWebTokenHashAlgorithm value)
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
    }
}