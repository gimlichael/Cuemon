using System;
using System.IO;
using System.Text;
using Cuemon.Text;

namespace Cuemon.IO
{
    /// <summary>
    /// This utility class is designed to make <see cref="Stream"/> related conversions easier to work with.
    /// </summary>
    public static class StreamConverter
    {
        /// <summary>
        /// Removes the preamble information (if present) from the specified <see cref="Stream"/>, and <paramref name="source"/> is being closed and disposed.
        /// </summary>
        /// <param name="source">The input <see cref="Stream"/> to process.</param>
        /// <param name="encoding">The encoding to use when determining the preamble to remove.</param>
        /// <returns>A <see cref="Stream"/> without preamble information.</returns>
        public static Stream RemovePreamble(Stream source, Encoding encoding)
        {
            Validator.ThrowIfNull(source, nameof(source));

            byte[] bytes = ByteConverter.FromStream(source);
            bytes = ByteUtility.RemovePreamble(bytes, encoding);

            MemoryStream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream(bytes);
                tempOutput.Position = 0;
                output = tempOutput;
                tempOutput = null;
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
                using (source) // because we return a stream, close the source
                {
                }
            }
            output.Position = 0;
            return output;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The byte array to be converted.</param>
        /// <returns>A <see cref="System.IO.Stream"/> object.</returns>
        public static Stream FromBytes(byte[] value)
        {
            MemoryStream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream(value);
                tempOutput.Position = 0;
                output = tempOutput;
                tempOutput = null;
            }
            finally
            {
                if (tempOutput != null) { tempOutput.Dispose(); }
            }
            output.Position = 0;
            return output;
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which need to be configured.</param>
        /// <returns>A <b><see cref="System.IO.Stream"/></b> object.</returns>
        /// <remarks><see cref="EncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static Stream FromString(string value, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            MemoryStream output = DelegateUtility.SafeInvokeDisposable(() =>
            {
                var m = new MemoryStream(ByteConverter.FromString(value, setup));
                m.Position = 0;
                return m;
            });
            return output;
        }
    }
}