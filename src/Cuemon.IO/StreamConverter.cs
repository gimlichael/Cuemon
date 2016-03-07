using System;
using System.IO;
using System.Text;

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
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/> using UTF-16 for the encoding preserving any preamble sequences.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <returns>A <b><see cref="System.IO.Stream"/></b> object.</returns>
        public static Stream FromString(string value)
        {
            return FromString(value, PreambleSequence.Keep);
        }

        /// <summary>
        /// Converts specified <paramref name="value"/> to a <see cref="Stream"/> using UTF-16 for the encoding.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <returns>A <b><see cref="System.IO.Stream"/></b> object.</returns>
        public static Stream FromString(string value, PreambleSequence sequence)
        {
            return FromString(value, sequence, Encoding.Unicode);
        }

        /// <summary>
        /// Converts the specified <paramref name="value"/> to a <see cref="Stream"/>.
        /// </summary>
        /// <param name="value">The string to be converted.</param>
        /// <param name="sequence">Determines whether too keep or remove any preamble sequences.</param>
        /// <param name="encoding">The preferred encoding to apply to the result.</param>
        /// <returns>A <b><see cref="System.IO.Stream"/></b> object.</returns>
        public static Stream FromString(string value, PreambleSequence sequence, Encoding encoding)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(encoding, nameof(encoding));

            MemoryStream output;
            MemoryStream tempOutput = null;
            try
            {
                tempOutput = new MemoryStream(ByteConverter.FromString(value, sequence, encoding));
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
    }
}