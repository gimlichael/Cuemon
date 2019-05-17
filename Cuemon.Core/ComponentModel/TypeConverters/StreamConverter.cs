using System;
using System.IO;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="Stream"/> to its equivalent <see cref="T:byte[]"/>.
    /// </summary>
    public class StreamConverter : IConverter<Stream, byte[], DisposableOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
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
        public byte[] ChangeType(Stream input, Action<DisposableOptions> setup = null)
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
    }
}