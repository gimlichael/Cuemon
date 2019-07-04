using System;
using System.ComponentModel;
using System.Linq;
using Cuemon.Text;

namespace Cuemon.ComponentModel.Codecs
{
    /// <summary>
    /// Provides a translator that converts a <see cref="string"/> to its equivalent <see cref="T:byte[]"/> and vice versa.
    /// </summary>
    public class StringToByteArrayCodec : ICodec<string, byte[], EncodingOptions>
    {
        /// <summary>
        /// Encodes all the characters in the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public virtual byte[] Encode(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            byte[] valueInBytes;
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    valueInBytes = options.Encoding.GetPreamble().Concat(options.Encoding.GetBytes(input)).ToArray();
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = options.Encoding.GetBytes(input);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(options.Preamble), (int)options.Preamble, typeof(PreambleSequence));
            }
            return valueInBytes;
        }

        /// <summary>
        /// Decodes a range of bytes from the specified <paramref name="input"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="string"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public virtual string Decode(byte[] input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(input, options.Encoding); }
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    input = ByteOrderMark.Remove(input, options.Encoding);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(options.Preamble), (int)options.Preamble, typeof(PreambleSequence));
            }
            return options.Encoding.GetString(input, 0, input.Length);
        }
    }
}