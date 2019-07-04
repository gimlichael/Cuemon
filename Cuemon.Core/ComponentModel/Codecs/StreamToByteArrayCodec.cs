using System;
using System.IO;
using Cuemon.ComponentModel.Decoders;
using Cuemon.ComponentModel.Encoders;

namespace Cuemon.ComponentModel.Codecs
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
    public class StreamToByteArrayCodec : ICodec, IEncoder<Stream, byte[], DisposableOptions>, IDecoder<Stream, byte[]>
    {
        /// <summary>
        /// Encodes the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Stream"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="DisposableOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="input"/> cannot be read from.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input" /> length is greater than <see cref="int.MaxValue"/>.
        /// </exception>
        public byte[] Encode(Stream input, Action<DisposableOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            Validator.ThrowIfFalse(input.CanRead, nameof(input), "Stream cannot be read from.");
            Validator.ThrowIfGreaterThan(input.Length, int.MaxValue, nameof(input), FormattableString.Invariant($"Stream length is greater than {int.MaxValue}."));
            var options = Patterns.Configure(setup);
            try
            {
                if (input is MemoryStream s)
                {
                    return s.ToArray();
                }
                using (var memoryStream = new MemoryStream(new byte[input.Length]))
                {
                    var oldPosition = input.Position;
                    if (input.CanSeek) { input.Position = 0; }
                    input.CopyTo(memoryStream);
                    if (input.CanSeek) { input.Position = oldPosition; }
                    return memoryStream.ToArray();
                }
            }
            finally
            {
                if (!options.LeaveOpen)
                {
                    input.Dispose();
                }
            }
        }

        /// <summary>
        /// Decodes the specified <paramref name="input"/> to its equivalent <see cref="Stream"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="Stream"/>.</param>
        /// <returns>A <see cref="Stream"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        public Stream Decode(byte[] input)
        {
            Validator.ThrowIfNull(input, nameof(input));
            return Disposable.SafeInvoke(() => new MemoryStream(input.Length), ms =>
            {
                ms.Write(input, 0, input.Length);
                ms.Position = 0;
                return ms;
            });
        }
    }
}