using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using Cuemon.ComponentModel;
using Cuemon.Text;

namespace Cuemon.Extensions.IO
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> containing the result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        public static Stream ToStream(this string value, Action<EncodingOptions> setup = null)
        {
            return Decorator.Enclose(value).ToStream(setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to extend.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="Stream"/> containing the result of the specified <paramref name="value"/>.</returns>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        public static Task<Stream> ToStreamAsync(this string value, Action<EncodingOptions> setup = null)
        {
            return Decorator.Enclose(value).ToStreamAsync(setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="TextReader"/> object.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to extend.</param>
        /// <returns>A <see cref="TextReader"/> initialized with <paramref name="value"/>.</returns>
        public static TextReader ToTextReader(this string value)
        {
            Validator.ThrowIfNull(value, nameof(value));
            return new StringReader(value);
        }
    }
}