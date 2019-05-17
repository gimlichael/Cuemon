using System;

namespace Cuemon.ComponentModel.TypeConverters
{
    /// <summary>
    /// Provides a converter that converts base data types of <see cref="IConvertible" /> to its equivalent <see cref="T:byte[]" />.
    /// </summary>
    public class BaseConvertibleConverter : 
        IConverter<bool, byte[], EndianOptions>,
        IConverter<char, byte[], EndianOptions>,
        IConverter<double, byte[], EndianOptions>,
        IConverter<short, byte[], EndianOptions>,
        IConverter<int, byte[], EndianOptions>,
        IConverter<long, byte[], EndianOptions>,
        IConverter<float, byte[], EndianOptions>,
        IConverter<ushort, byte[], EndianOptions>,
        IConverter<uint, byte[], EndianOptions>,
        IConverter<ulong, byte[], EndianOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="bool"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(bool input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="char"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(char input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(double input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="short"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(short input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="int"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(int input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="long"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(long input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="float"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(float input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="ushort"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(ushort input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="uint"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(uint input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="ulong"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public byte[] ChangeType(ulong input, Action<EndianOptions> setup = null)
        {
            return ReverseByteArrayWhenRequested(BitConverter.GetBytes(input), setup);
        }

        internal static byte[] ReverseByteArrayWhenRequested(byte[] bytes, Action<EndianOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            if (options.UseBigEndian && BitConverter.IsLittleEndian) { Array.Reverse(bytes); }
            return bytes;
        }
    }
}