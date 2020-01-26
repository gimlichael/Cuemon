using System;
using System.ComponentModel;
using System.IO;
using Cuemon.Integrity;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a translator that converts a <see cref="Stream" /> to its equivalent <see cref="string" /> and vice versa.
    /// Implements the <see cref="ICodec" />
    /// Implements the <see cref="IEncoder{TInput, TOutput, TOptions}" />
    /// Implements the <see cref="IDecoder{TInput, TOutput, TOptions}" />
    /// </summary>
    /// <seealso cref="ICodec" />
    /// <seealso cref="IEncoder{TInput, TOutput, TOptions}" />
    /// <seealso cref="IDecoder{TInput, TOutput, TOptions}" />
    public class StreamStringCodec : ICodec, IEncoder<Stream, string, StreamEncodingOptions>, IDecoder<string, Stream, EncodingOptions>
    {
        /// <summary>
        /// Encodes the specified <paramref name="value"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to be converted into a <see cref="string"/>.</param>
        /// <param name="setup">The <see cref="StreamEncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="StreamEncodingOptions.Preamble"/>.
        /// </exception>
        /// <seealso cref="StreamEncodingOptions"/>
        /// <seealso cref="StreamByteArrayCodec"/>
        public string Encode(Stream value, Action<StreamEncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(value, options.Encoding); }
            if (options.Preamble < PreambleSequence.Keep || options.Preamble > PreambleSequence.Remove) { throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence)); }

            var bytes = ConvertFactory.UseCodec<StreamByteArrayCodec>().Encode(value, o => o.LeaveOpen = options.LeaveOpen);
            return Convertible.ToString(bytes, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            });
        }

        /// <summary>
        /// Decodes the specified <paramref name="encoded"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="encoded">The <see cref="string"/> to be converted into a <see cref="Stream"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> that is equivalent to <paramref name="encoded"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encoded"/> cannot be null.
        /// </exception>
        public Stream Decode(string encoded, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(encoded, nameof(encoded));
            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                var bytes = Convertible.GetBytes(encoded, setup);
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return ms;
            });
        }
    }
}