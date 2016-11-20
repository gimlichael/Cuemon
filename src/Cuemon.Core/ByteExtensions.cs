using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the <see cref="byte"/> structure using various methods already found in the Microsoft .NET Framework.
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// Converts an array of 8-bit unsigned integers to its equivalent string representation that is encoded with base-64 digits.
        /// </summary>
        /// <param name="value">An array of 8-bit unsigned integers.</param>
        /// <returns>The string representation, in base 64, of the contents of <paramref name="value"/>.</returns>
        public static string ToBase64(this byte[] value)
        {
            return Convert.ToBase64String(value);
        }
    }
}