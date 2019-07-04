using System;
using System.IO;
using Cuemon.ComponentModel.Codecs;
using Cuemon.IO;
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
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <b><see cref="Stream"/></b> object.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static Stream ToStream(this string value, Action<EncodingOptions> setup = null)
        {
            return ConvertFactory.UseCodec<StreamToStringCodec>().Decode(value, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="TextReader"/> object.
        /// </summary>
        /// <param name="value">The <see cref="string"/> to extend.</param>
        /// <returns>A <see cref="TextReader"/> initialized with <paramref name="value"/>.</returns>
        public static TextReader ToTextReader(this string value)
        {
            return TextReaderConverter.FromString(value);
        }
    }
}