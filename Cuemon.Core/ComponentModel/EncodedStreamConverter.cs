using System;
using System.ComponentModel;
using System.IO;
using Cuemon.Text;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a converter that converts a <see cref="Stream"/> to its equivalent <see cref="T:char[]"/>.
    /// </summary>
    public class EncodedStreamConverter : IConverter<Stream, char[], EncodingOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:char[]"/> representation.
        /// </summary>
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
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public char[] ChangeType(Stream input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(input, options.Encoding); }
            var valueInBytes = ConvertFactory.UseCodec<StreamToByteArrayCodec>().Encode(input);
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = ByteOrderMark.Remove(valueInBytes, options.Encoding);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(options.Preamble), (int)options.Preamble, typeof(PreambleSequence));
            }
            return options.Encoding.GetChars(valueInBytes);
        }
    }
}