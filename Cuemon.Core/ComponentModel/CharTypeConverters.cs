using System;
using System.ComponentModel;
using System.IO;
using Cuemon.ComponentModel.TypeConverters;
using Cuemon.Text;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:IConversion{System.Char}"/> interface.
    /// </summary>
    public static class CharTypeConverters
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="char"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="char"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <seealso cref="BaseConvertibleConverter"/>
        /// <seealso cref="EndianOptions"/>
        public static byte[] ToByteArray(this IConversion<char> _, char input, Action<EndianOptions> setup = null)
        {
            return Converter<char, byte[], EndianOptions>.UseConverter<BaseConvertibleConverter>(input, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:char[]"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="Stream"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="Stream"/> to be converted into a <see cref="T:char[]"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:char[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be read from.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input" /> length is greater than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <seealso cref="EncodedStreamConverter"/>
        /// <seealso cref="EncodingOptions"/>
        public static char[] ToCharArray(this IConversion<Stream> _, Stream input, Action<EncodingOptions> setup = null)
        {
            return Converter<Stream, char[], EncodingOptions>.UseConverter<EncodedStreamConverter>(input, setup);
        }
    }
}