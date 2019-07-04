using System;
using System.ComponentModel;
using System.IO;
using Cuemon.ComponentModel.Decoders;
using Cuemon.ComponentModel.Encoders;
using Cuemon.IO;
using Cuemon.Text;

namespace Cuemon.ComponentModel.Codecs
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
    public class StreamToStringCodec : ICodec, IEncoder<Stream, string, StreamEncodingOptions>, IDecoder<Stream, string, EncodingOptions>
    {
        /// <summary>
        /// Encodes the specified <paramref name="input"/> to its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to be converted into a <see cref="string"/>.</param>
        /// <param name="setup">The <see cref="StreamEncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="StreamEncodingOptions.Preamble"/>.
        /// </exception>
        /// <seealso cref="StreamEncodingOptions"/>
        /// <seealso cref="StreamToByteArrayCodec"/>
        /// <seealso cref="StringToByteArrayCodec"/>
        public string Encode(Stream input, Action<StreamEncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(input, options.Encoding); }
            if (options.Preamble < PreambleSequence.Keep || options.Preamble > PreambleSequence.Remove) { throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence)); }

            var bytes = ConvertFactory.UseCodec<StreamToByteArrayCodec>().Encode(input, o => o.LeaveOpen = options.LeaveOpen);
            return ConvertFactory.UseCodec<StringToByteArrayCodec>().Decode(bytes, o =>
            {
                o.Encoding = options.Encoding;
                o.Preamble = options.Preamble;
            });
        }

        /// <summary>
        /// Decodes the specified <paramref name="input"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to be converted into a <see cref="Stream"/>.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="Stream"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <seealso cref="StringToByteArrayCodec"/>
        public Stream Decode(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            return Disposable.SafeInvoke(() => new MemoryStream(), ms =>
            {
                var bytes = ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(input, setup);
                ms.Write(bytes, 0, bytes.Length);
                ms.Position = 0;
                return ms;
            });
        }
    }
}