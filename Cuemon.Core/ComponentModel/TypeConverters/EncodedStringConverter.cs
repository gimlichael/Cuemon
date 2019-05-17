using System;
using System.ComponentModel;
using Cuemon.Text;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="string"/> to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class EncodedStringConverter : IConverter<string, byte[], EncodingOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public byte[] ChangeType(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            byte[] valueInBytes;
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    valueInBytes = ByteArrayUtility.CombineByteArrays(options.Encoding.GetPreamble(), options.Encoding.GetBytes(input));
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = options.Encoding.GetBytes(input);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(options.Preamble), (int)options.Preamble, typeof(PreambleSequence));
            }
            return valueInBytes;
        }
    }
}