using System;
using System.IO;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:IConversion{System.IO.Stream}"/> interface.
    /// </summary>
    public static class StreamTypeConverters
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="Stream"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="Stream"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="DisposableOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be read from.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input" /> length is greater than <see cref="int.MaxValue"/>.
        /// </exception>
        /// <seealso cref="StreamConverter"/>
        /// <seealso cref="DisposableOptions"/>
        public static byte[] ToByteArray(this IConversion<Stream> _, Stream input, Action<DisposableOptions> setup = null)
        {
            return Converter<Stream, byte[], DisposableOptions>.UseConverter<StreamConverter>(input, setup);
        }
    }
}