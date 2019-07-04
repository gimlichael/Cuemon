using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cuemon.Collections.Generic;
using Cuemon.ComponentModel.Codecs;
using Cuemon.Configuration;
using Cuemon.Text;

namespace Cuemon.Integrity
{
    public class Convertible : Configurable<ConvertibleOptions>
    {
        private static readonly Lazy<Dictionary<Type, Func<IConvertible, Action<EndianOptions>, byte[]>>> LazyEndianSensitiveByteArrayConverters = new Lazy<Dictionary<Type, Func<IConvertible, Action<EndianOptions>, byte[]>>>(() =>
        {
            return new Dictionary<Type, Func<IConvertible, Action<EndianOptions>, byte[]>>()
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
        });

        private static readonly Lazy<Dictionary<Type, Func<IConvertible, byte[]>>> LazyByteArrayConverters = new Lazy<Dictionary<Type, Func<IConvertible, byte[]>>>(() =>
        {
            return new Dictionary<Type, Func<IConvertible, byte[]>>()
            {
                { typeof(string), input => input is string x ? GetBytes(x) : null },
                { typeof(DateTime), input => input is DateTime x ? GetBytes(x) : null },
                { typeof(decimal), input => input is decimal x ? GetBytes(x) : null },
                { typeof(DBNull), input => input is DBNull x ? GetBytes(x) : null }
            };
        });

        private static readonly Dictionary<Type, Func<IConvertible, Action<EndianOptions>, byte[]>> EndianSensitiveByteArrayConverters = LazyEndianSensitiveByteArrayConverters.Value;

        private static readonly Dictionary<Type, Func<IConvertible, byte[]>> ByteArrayConverters = LazyByteArrayConverters.Value;

        /// <summary>
        /// A representation for a null value when converting to a byte array.
        /// </summary>
        public const int NullValue = 0;

        public static void RegisterConvertible<T>(Func<T, byte[]> converter) where T : IConvertible
        {
            ByteArrayConverters.Add(typeof(T), convertible => converter((T)convertible));
        }

        public Convertible(Action<ConvertibleOptions> setup = null) : base(Patterns.Configure(setup))
        {
        }

        public byte[] GetBytes(params IConvertible[] input)
        {
            return GetBytes(Arguments.ToEnumerable(input));
        }

        public byte[] GetBytes(IEnumerable<IConvertible> input)
        {
            var result = new List<byte>();
            foreach (var type in input)
            {
                var bytes = GetBytes(type);
                if (bytes != null) { result.AddRange(bytes); }
            }
            return result.ToArray();
        }

        public byte[] GetBytes(IConvertible input)
        {
            if (input == null) { return BitConverter.GetBytes(NullValue); }
            if (input.GetType().IsPrimitive || input is Enum)
            {
                foreach (var item in EndianSensitiveByteArrayConverters)
                {
                    var bytes = item.Value(input, o => o.ByteOrder = Options.ByteOrder);
                    if (bytes != null) { return bytes; }
                }
            }

            foreach (var item in ByteArrayConverters)
            {
                var converter = Options.Converters[item.Key];
                var bytes = converter == null ? item.Value(input) : converter(input);
                if (bytes != null) { return bytes; }
            }
            throw new ArgumentOutOfRangeException(nameof(input), input, "Unknown implementation of IConvertible; please use RegisterConvertible to make a custom implementation known.");
        }

        public static byte ReverseBits8(byte input)
        {
            return (byte)ReverseBits(input, sizeof(byte));
        }

        public static ushort ReverseBits16(ushort input)
        {
            return (ushort)ReverseBits(input, sizeof(ushort));
        }

        public static uint ReverseBits32(uint input)
        {
            return (uint)ReverseBits(input, sizeof(uint));
        }

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

        public static byte[] GetBytes(bool input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(byte input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, x => new[] { x }, setup);
        }

        public static byte[] GetBytes(char input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(DateTime input)
        {
            return GetBytes(input.ToString("u", CultureInfo.InvariantCulture), o => o.Encoding = Encoding.ASCII);
        }

        public static byte[] GetBytes(DBNull input)
        {
            return GetBytesCore(input, x => BitConverter.GetBytes(NullValue), null);
        }

        public static byte[] GetBytes(decimal input)
        {
            return GetBytes(input.ToString(CultureInfo.InvariantCulture), o => o.Encoding = Encoding.ASCII);
        }

        public static byte[] GetBytes(double input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(short input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(int input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(long input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(sbyte input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, x => BitConverter.GetBytes(x), setup);
        }

        public static byte[] GetBytes(float input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(ushort input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(uint input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(ulong input, Action<EndianOptions> setup = null)
        {
            return GetBytesCore(input, BitConverter.GetBytes, setup);
        }

        public static byte[] GetBytes(string input, Action<EncodingOptions> setup = null)
        {
            return GetBytesCore(input, x => ConvertFactory.UseCodec<StringToByteArrayCodec>().Encode(input, setup), null);
        }

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