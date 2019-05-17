using System;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:IConversion{byte[]}"/> interface.
    /// </summary>
    public static class ByteArrayTypeConverters
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="bool"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="bool"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="bool"/> that is equivalent to <paramref name="input"/>.</returns>
        public static bool ToBoolean(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToBoolean(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="char"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="char"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="char"/> that is equivalent to <paramref name="input"/>.</returns>
        public static char ToChar(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToChar(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="double"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="double"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="double"/> that is equivalent to <paramref name="input"/>.</returns>
        public static double ToDouble(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToDouble(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="short"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="short"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="short"/> that is equivalent to <paramref name="input"/>.</returns>
        public static short ToInt16(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToInt16(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="int"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="int"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="int"/> that is equivalent to <paramref name="input"/>.</returns>
        public static int ToInt32(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToInt32(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="long"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="long"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="long"/> that is equivalent to <paramref name="input"/>.</returns>
        public static long ToInt64(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToInt64(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="float"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="float"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="float"/> that is equivalent to <paramref name="input"/>.</returns>
        public static float ToSingle(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToSingle(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="ushort"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="ushort"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="ushort"/> that is equivalent to <paramref name="input"/>.</returns>
        public static ushort ToUInt16(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToUInt16(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="uint"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="uint"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="uint"/> that is equivalent to <paramref name="input"/>.</returns>
        public static uint ToUInt32(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToUInt32(input, 0);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="ulong"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="T:byte[]"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="T:byte[]"/> to be converted into a <see cref="ulong"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="ulong"/> that is equivalent to <paramref name="input"/>.</returns>
        public static ulong ToUInt64(this IConversion<byte[]> _, byte[] input, Action<EndianOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            input = BaseConvertibleConverter.ReverseByteArrayWhenRequested(input, setup);
            return BitConverter.ToUInt64(input, 0);
        }
    }
}