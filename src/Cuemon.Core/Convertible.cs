using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods, suitable for verifying integrity of data, that convert <see cref="IConvertible"/> implementations to and from a sequence of bytes.
    /// </summary>
    public static class Convertible
    {
        private static readonly Dictionary<Type, Func<IConvertible, Action<EndianOptions>, byte[]>> EndianSensitiveByteArrayConverters = new()
        {
            { typeof(bool), (i, o) => i is bool x ? GetBytes(x, o) : null },
            { typeof(byte), (i, o) => i is byte x ? GetBytes(x, o) : null },
            { typeof(char), (i, o) => i is char x ? GetBytes(x, o) : null },
            { typeof(double), (i, o) => i is double x ? GetBytes(x, o) : null },
            { typeof(short), (i, o) => i is short x ? GetBytes(x, o) : null },
            { typeof(int), (i, o) => i is int x ? GetBytes(x, o) : null },
            { typeof(long), (i, o) => i is long x ? GetBytes(x, o) : null },
            { typeof(sbyte), (i, o) => i is sbyte x ? GetBytes(x, o) : null },
            { typeof(float), (i, o) => i is float x ? GetBytes(x, o) : null },
            { typeof(ushort), (i, o) => i is ushort x ? GetBytes(x, o) : null },
            { typeof(uint), (i, o) => i is uint x ? GetBytes(x, o) : null },
            { typeof(ulong), (i, o) => i is ulong x ? GetBytes(x, o) : null },
            { typeof(Enum), (i, o) => i is Enum x ? GetBytes(x, o) : null }
        };

        private static readonly Dictionary<Type, Func<IConvertible, byte[]>> ByteArrayConverters =  new()
        {
            { typeof(string), input => input is string x ? GetBytes(x) : null },
            { typeof(DateTime), input => input is DateTime x ? GetBytes(x) : null },
            { typeof(decimal), input => input is decimal x ? GetBytes(x) : null },
            { typeof(DBNull), input => input is DBNull x ? GetBytes(x) : null }
        };

        /// <summary>
        /// A representation for a null value when converting to a <see cref="T:byte[]"/>.
        /// </summary>
        public const int NullValue = 0;

        /// <summary>
        /// Registers the specified <see cref="IConvertible"/> implementation of <typeparamref name="T"/> to make it globally known.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="IConvertible"/> implementation to use.</typeparam>
        /// <param name="converter">The function delegate that converts an <see cref="IConvertible"/> implementation to its equivalent <see cref="T:byte[]"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="converter"/> cannot be null.
        /// </exception>
        public static void RegisterConvertible<T>(Func<T, byte[]> converter) where T : IConvertible
        {
            Validator.ThrowIfNull(converter, nameof(converter));
            ByteArrayConverters.Add(typeof(T), convertible => converter((T)convertible));
        }

        /// <summary>
        /// Reverse the bits of the specified <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The unsigned 8-bit integer to reverse bits on.</param>
        /// <returns>A <see cref="byte"/> with the bits reversed.</returns>
        public static byte ReverseBits8(byte input)
        {
            return (byte)ReverseBits(input, sizeof(byte));
        }

        /// <summary>
        /// Reverse the bits of the specified <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The unsigned 16-bit integer to reverse bits on.</param>
        /// <returns>A <see cref="ushort"/> with the bits reversed.</returns>
        public static ushort ReverseBits16(ushort input)
        {
            return (ushort)ReverseBits(input, sizeof(ushort));
        }

        /// <summary>
        /// Reverse the bits of the specified <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The unsigned 32-bit integer to reverse bits on.</param>
        /// <returns>A <see cref="uint"/> with the bits reversed.</returns>
        public static uint ReverseBits32(uint input)
        {
            return (uint)ReverseBits(input, sizeof(uint));
        }

        /// <summary>
        /// Reverse the bits of the specified <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The unsigned 64-bit integer to reverse bits on.</param>
        /// <returns>A <see cref="ulong"/> with the bits reversed.</returns>
        public static ulong ReverseBits64(ulong input)
        {
            return ReverseBits(input, sizeof(ulong));
        }

        private static ulong ReverseBits(ulong input, byte byteSize)
        {
            var bitSize = byteSize * ByteUnit.BitsPerByte;
            ulong output = 0;
            for (var i = 0; i < bitSize; i++)
            {
                if ((input & ((ulong)1 << i)) != 0) { output |= (ulong)1 << ((bitSize - 1) - i); }
            }
            return output;
        }

        /// <summary>
        /// Returns the specified <see cref="IConvertible"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="IConvertible"/> implementation to convert.</param>
        /// <param name="setup">The <see cref="ConvertibleOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="input"/> is an unknown implementation of <see cref="IConvertible"/>; please use RegisterConvertible to make a custom implementation globally known -or- use setup to add a custom implementation using ConvertibleOptions.Converters..Add.
        /// </exception>
        public static byte[] GetBytes(IConvertible input, Action<ConvertibleOptions> setup = null)
        {
            if (input == null) { return BitConverter.GetBytes(NullValue); }
            var options = Patterns.Configure(setup);

            if (options.Converters.Count > 0)
            {
                var localConverter = options.Converters[input.GetType()];
                if (localConverter != null) { return localConverter(input); }
            }

            if (input.GetType().IsPrimitive || input is Enum)
            {
                foreach (var item in EndianSensitiveByteArrayConverters)
                {
                    var bytes = item.Value(input, o => o.ByteOrder = options.ByteOrder);
                    if (bytes != null) { return bytes; }
                }
            }

            foreach (var item in ByteArrayConverters)
            {
                var bytes = item.Value(input);
                if (bytes != null) { return bytes; }
            }

            throw new ArgumentOutOfRangeException(nameof(input), input, $"Unknown implementation of IConvertible; please use {nameof(RegisterConvertible)} to make a custom implementation globally known -or- use {nameof(setup)} to add a custom implementation using {nameof(ConvertibleOptions)}.{nameof(ConvertibleOptions.Converters)}.{nameof(ConvertibleOptions.Converters.Add)}.");
        }

        /// <summary>
        /// Returns the specified sequence of <see cref="IConvertible"/> as an aggregated <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="IEnumerable{IConvertible}"/> sequence to convert.</param>
        /// <param name="setup">The <see cref="ConvertibleOptions"/> which may be configured.</param>
        /// <returns>An aggregated <see cref="T:byte[]"/> that is otherwise equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(IEnumerable<IConvertible> input, Action<ConvertibleOptions> setup = null)
        {
            var result = new List<byte>();
            foreach (var type in input)
            {
                var bytes = GetBytes(type, setup);
                if (bytes != null) { result.AddRange(bytes); }
            }
            return result.ToArray();
        }

        /// <summary>
        /// Returns the specified <see cref="bool"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="bool"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(bool input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="byte"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="byte"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(byte input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, x => new[] { x }, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="char"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="char"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(char input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="DateTime"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="DateTime"/> to convert.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(DateTime input)
        {
            return GetBytes(input.ToString("u", CultureInfo.InvariantCulture), o => o.Encoding = Encoding.ASCII);
        }

        /// <summary>
        /// Returns the specified <see cref="DBNull"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="DBNull"/> to convert.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(DBNull input)
        {
            return GetBytesCore(input, x => BitConverter.GetBytes(NullValue), null);
        }

        /// <summary>
        /// Returns the specified <see cref="decimal"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="decimal"/> to convert.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(decimal input)
        {
            return GetBytes(input.ToString(CultureInfo.InvariantCulture), o => o.Encoding = Encoding.ASCII);
        }

        /// <summary>
        /// Returns the specified <see cref="double"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="double"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(double input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }
        
        /// <summary>
        /// Returns the specified <see cref="short"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="short"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(short input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="int"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="int"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(int input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="long"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="long"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(long input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="sbyte"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="sbyte"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(sbyte input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, x => BitConverter.GetBytes(x), setup);
        }

        /// <summary>
        /// Returns the specified <see cref="float"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="float"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(float input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="ushort"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="ushort"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(ushort input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="uint"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="uint"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(uint input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="ulong"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="ulong"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(ulong input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        /// <summary>
        /// Returns the specified <see cref="string"/> as its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="string"/> to convert.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static byte[] GetBytes(string input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            byte[] valueInBytes;
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    valueInBytes = options.Encoding.GetPreamble().Concat(options.Encoding.GetBytes(input)).ToArray();
                    break;
                case PreambleSequence.Remove:
                    valueInBytes = options.Encoding.GetBytes(input);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence));
            }
            return valueInBytes;
        }

        /// <summary>
        /// Returns the specified <see cref="Enum"/> as a <see cref="T:byte[]"/>.
        /// </summary>
        /// <param name="input">The <see cref="Enum"/> to convert.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        public static byte[] GetBytes(Enum input, Action<EndianOptions> setup = null)
        {
            var tc = input.GetTypeCode();
            var c = input as IConvertible;
            switch (tc)
            {
                case TypeCode.Byte:
                {
                    return GetBytes(c.ToByte(CultureInfo.InvariantCulture), setup);
                }
                case TypeCode.Int16:
                {
                    return GetBytes(c.ToInt16(CultureInfo.InvariantCulture), setup);
                }
                case TypeCode.Int64:
                {
                    return GetBytes(c.ToInt64(CultureInfo.InvariantCulture), setup);
                }
                case TypeCode.UInt16:
                {
                    return GetBytes(c.ToUInt16(CultureInfo.InvariantCulture), setup);
                }
                case TypeCode.UInt32:
                {
                    return GetBytes(c.ToUInt32(CultureInfo.InvariantCulture), setup);
                }
                case TypeCode.UInt64:
                {
                    return GetBytes(c.ToUInt64(CultureInfo.InvariantCulture), setup);
                }
                case TypeCode.SByte:
                {
                    return GetBytes(c.ToSByte(CultureInfo.InvariantCulture), setup);
                }
                default:
                    return GetBytes(c.ToInt32(CultureInfo.InvariantCulture), setup);
            }
        }

        /// <summary>
        /// Returns the specified <see cref="T:byte[]"/> as its equivalent <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to convert.</param>
        /// <param name="setup">The <see cref="EncodingOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialized with an invalid <see cref="EncodingOptions.Preamble"/>.
        /// </exception>
        /// <remarks><see cref="IEncodingOptions"/> will be initialized with <see cref="EncodingOptions.DefaultPreambleSequence"/> and <see cref="EncodingOptions.DefaultEncoding"/>.</remarks>
        public static string ToString(byte[] input, Action<EncodingOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            if (options.Encoding.Equals(EncodingOptions.DefaultEncoding)) { options.Encoding = ByteOrderMark.DetectEncodingOrDefault(input, options.Encoding); }
            switch (options.Preamble)
            {
                case PreambleSequence.Keep:
                    break;
                case PreambleSequence.Remove:
                    input = ByteOrderMark.Remove(input, options.Encoding);
                    break;
                default:
                    throw new InvalidEnumArgumentException(nameof(setup), (int)options.Preamble, typeof(PreambleSequence));
            }
            return options.Encoding.GetString(input, 0, input.Length);
        }

        /// <summary>
        /// Reverse the endianness of the specified <paramref name="input"/>.
        /// </summary>
        /// <param name="input">The <see cref="T:byte[]"/> to reverse.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that, depending on the <paramref name="setup"/>, is either equal or a reversed value of <paramref name="input"/>.</returns>
        public static byte[] ReverseEndianness(byte[] input, Action<EndianOptions> setup = null)
        {
            var options = Patterns.Configure(setup);
            switch (options.ByteOrder)
            {
                case Endianness.BigEndian:
                    if (BitConverter.IsLittleEndian) { Array.Reverse(input); }
                    break;
                default:
                    if (!BitConverter.IsLittleEndian) { Array.Reverse(input); }
                    break;
            }
            return input;
        }

        private static byte[] GetBytesCore<T>(T input, Func<T, byte[]> converter, Action<EndianOptions> setup) where T : IConvertible
        {
            if (TryCast<T>(input, out var result)) { return setup == null ? converter(result) : ReverseEndianness(converter(result), setup); }
            return BitConverter.GetBytes(NullValue);
        }

        private static bool TryCast<T>(IConvertible convertible, out T concrete) where T : IConvertible
        {
            if (convertible is T ct)
            {
                concrete = ct;
                return true;
            }
            concrete = default;
            return false;
        }
    }
}