using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods for eradicating different types of values or sequences of values.
    /// </summary>
    /// <seealso cref="Generate"/>
    public static class Eradicate
    {
        /// <summary>
        /// Eradicates trailing zero information (if any) from the specified set of <paramref name="bytes"/>.
        /// </summary>
        /// <param name="bytes">The <see cref="T:byte[]"/> to process.</param>
        /// <returns>A <see cref="T:byte[]"/> without trailing zeros.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="bytes" /> must have a length larger than 1.
        /// </exception>
        public static byte[] TrailingZeros(byte[] bytes)
        {
            Validator.ThrowIfNull(bytes);
            Validator.ThrowIfLowerThanOrEqual(bytes.Length, 1, nameof(bytes), "The byte array must have a length larger than 1.");
            var hasTrailingZeros = false;
            var marker = bytes.Length - 1;
            while (marker > 0 && bytes[marker] == 0)
            {
                if (!hasTrailingZeros) { hasTrailingZeros = true; }  
                marker--;
            }
            if (!hasTrailingZeros) { return bytes; }
            marker++;
            var output = new byte[marker];
            Array.Copy(bytes, output, marker);
            return output;
        }
    }
}