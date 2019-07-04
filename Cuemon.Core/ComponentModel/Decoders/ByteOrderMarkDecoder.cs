using System;
using System.Text;

namespace Cuemon.ComponentModel.Decoders
{
    /// <summary>
    /// Provides a decoder that converts a <see cref="T:byte[]"/> to its equivalent <see cref="Encoding"/>.
    /// </summary>
    public class ByteOrderMarkDecoder : IDecoder<Encoding, byte[]>
    {
        /// <summary>
        /// Decodes a BOM-preamble of the specified <paramref name="input"/> to its equivalent <see cref="Encoding"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into an <see cref="Encoding"/>.</param>
        /// <returns>An <see cref="Encoding"/> that is equivalent to the BOM-preamble of <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> was without byte order mark information (BOM).
        /// </exception>
        public Encoding Decode(byte[] input)
        {
            Validator.ThrowIfNull(input, nameof(input));
            if (input.Length >= 4)
            {

                if (input[0] == 0xEF &&
                    input[1] == 0xBB &&
                    input[2] == 0xBF)
                {
                    return Encoding.GetEncoding("UTF-8");
                }

                if (input[0] == 0x00 &&
                    input[1] == 0x00 &&
                    input[2] == 0xFE &&
                    input[3] == 0xFF)
                {
                    return Encoding.GetEncoding("UTF-32BE");
                }

                if (input[0] == 0xFF &&
                    input[1] == 0xFE &&
                    input[2] == 0x00 &&
                    input[3] == 0x00)
                {
                    return Encoding.GetEncoding("UTF-32");
                }

                if (input[0] == 0xFE &&
                    input[1] == 0xFF)
                {
                    return Encoding.GetEncoding("UNICODEFFFE");
                }

                if (input[0] == 0xFF &&
                    input[1] == 0xFE)
                {
                    return Encoding.GetEncoding("UTF-16");
                }

                if (input[0] == 0x2B &&
                    input[1] == 0x2F &&
                    input[2] == 0x76 &&
                    (input[3] == 0x38 ||
                     input[3] == 0x39 ||
                     input[3] == 0x2B ||
                     input[3] == 0x2F))
                {
                    return Encoding.GetEncoding("UTF-7");
                }
            }

            throw new ArgumentException("Unable to locate and decode BOM.", nameof(input));
        }
    }
}