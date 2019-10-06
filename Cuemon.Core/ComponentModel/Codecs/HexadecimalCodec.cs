using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Cuemon.Integrity;
using Cuemon.Text;

namespace Cuemon.ComponentModel.Codecs
{
    /// <summary>
    /// Provides an encoder that converts a <see cref="string"/> to its equivalent hexadecimal <see cref="string"/> and vice versa.
    /// </summary>
    public class HexadecimalCodec : ICodec<string, string, EncodingOptions>
    {
        /// <summary>
        /// Encodes all the characters in the specified <paramref name="input"/> to its equivalent hexadecimal <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a hexadecimal <see cref="string"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A hexadecimal <see cref="string"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <seealso cref="HexadecimalConverter"/>
        public string Encode(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var encodedString = Convertible.GetBytes(input, setup);
            return ConvertFactory.FromHexadecimal().ChangeType(encodedString);
        }

        /// <summary>
        /// Decodes the specified hexadecimal <paramref name="input"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The hexadecimal <see cref="string"/> to be converted into a text <see cref="string"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A text <see cref="string"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> needs to have a length that is an even number.
        /// </exception>
        /// <exception cref="FormatException">
        /// <paramref name="input"/> is not an hexadecimal encoded.
        /// </exception>
        public string Decode(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            if (!Condition.IsEven(input.Length)) { throw new ArgumentException("The character length of a hexadecimal string must be an even number.", nameof(input)); }

            var converted = new List<byte>();
            var stringLength = input.Length / 2;
            using (var reader = new StringReader(input))
            {
                for (var i = 0; i < stringLength; i++)
                {
                    var firstChar = (char)reader.Read();
                    var secondChar = (char)reader.Read();
                    if (!Condition.IsHex(firstChar) || !Condition.IsHex(secondChar)) { throw new FormatException("One or more characters is not a valid hexadecimal value."); }
                    converted.Add(Convert.ToByte(new string(new[] { firstChar, secondChar }), 16));
                }
            }
            return Convertible.ToString(converted.ToArray(), setup);
        }
    }
}