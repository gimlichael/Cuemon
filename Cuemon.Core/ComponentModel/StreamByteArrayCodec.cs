using System;
using System.IO;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Provides a translator that converts a <see cref="Stream" /> to its equivalent <see cref="T:byte[]" /> and vice versa.
    /// Implements the <see cref="ICodec" />
    /// Implements the <see cref="IEncoder{TInput, TOutput, TOptions}" />
    /// Implements the <see cref="IDecoder{TInput, TOutput}" />
    /// </summary>
    /// <seealso cref="ICodec" />
    /// <seealso cref="IEncoder{TInput, TOutput, TOptions}" />
    /// <seealso cref="IDecoder{TInput, TOutput}" />
    public class StreamByteArrayCodec : ICodec, IEncoder<Stream, byte[], DisposableOptions>, IDecoder<byte[], Stream>
    {
        /// <summary>
        /// Encodes the specified <paramref name="value"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="value">The <see cref="Stream"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="DisposableOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="value"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be read from.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value" /> length is greater than <see cref="int.MaxValue"/>.
        /// </exception>
        public byte[] Encode(Stream value, Action<DisposableOptions> setup = null)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfFalse(value.CanRead, nameof(value), "Stream cannot be read from.");
            Validator.ThrowIfGreaterThan(value.Length, int.MaxValue, nameof(value), FormattableString.Invariant($"Stream length is greater than {int.MaxValue}."));
            var options = Patterns.Configure(setup);
            try
            {
                if (value is MemoryStream s)
                {
                    return s.ToArray();
                }
                using (var memoryStream = new MemoryStream(new byte[value.Length]))
                {
                    var oldPosition = value.Position;
                    if (value.CanSeek) { value.Position = 0; }
                    value.CopyTo(memoryStream);
                    if (value.CanSeek) { value.Position = oldPosition; }
                    return memoryStream.ToArray();
                }
            }
            finally
            {
                if (!options.LeaveOpen)
                {
                    value.Dispose();
                }
            }
        }

        /// <summary>
        /// Decodes the specified <paramref name="encoded"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="encoded">The <see cref="T:byte[]"/> to be converted into a <see cref="Stream"/>.</param>
        /// <returns>A <see cref="Stream"/> that is equivalent to <paramref name="encoded"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="encoded"/> cannot be null.
        /// </exception>
        public Stream Decode(byte[] encoded)
        {
            Validator.ThrowIfNull(encoded, nameof(encoded));
            return Disposable.SafeInvoke(() => new MemoryStream(encoded.Length), ms =>
            {
                ms.Write(encoded, 0, encoded.Length);
                ms.Position = 0;
                return ms;
            });
        }
    }
}