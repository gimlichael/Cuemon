using System;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="IConvertible"/> related conversions easier to work with.
    /// </summary>
    public static class ConvertibleConverter
    {
        /// <summary>
        /// Returns an <see cref="IConvertible"/> primitive converted from the specified array of <paramref name="bytes"/>.
        /// </summary>
        /// <typeparam name="T">The type of the expected <see cref="IConvertible"/> after conversion.</typeparam>
        /// <param name="bytes">The <see cref="T:byte[]"/> to convert into an <see cref="IConvertible"/>.</param>
        /// <returns>An <see cref="IConvertible"/> primitive formed by n-bytes beginning at 0.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="bytes"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="bool"/>, <see cref="char"/>, <see cref="double"/>, <see cref="short"/>, <see cref="int"/>, <see cref="ushort"/>, <see cref="uint"/> and <see cref="ulong"/>.
        /// </exception>
        public static T FromBytes<T>(byte[] bytes) where T : struct, IConvertible
        {
            Validator.ThrowIfNull(bytes, nameof(bytes));
            if (BitConverter.IsLittleEndian) { Array.Reverse(bytes); }
            var code = TypeCodeConverter.FromType(typeof(T));

            object result;
            switch (code)
            {
                case TypeCode.Boolean:
                    result = BitConverter.ToBoolean(bytes, 0);
                    break;
                case TypeCode.Char:
                    result = BitConverter.ToChar(bytes, 0);
                    break;
                case TypeCode.Double:
                    result = BitConverter.ToDouble(bytes, 0);
                    break;
                case TypeCode.Int16:
                    result = BitConverter.ToInt16(bytes, 0);
                    break;
                case TypeCode.Int32:
                    result = BitConverter.ToInt32(bytes, 0);
                    break;
                case TypeCode.Int64:
                    result = BitConverter.ToInt64(bytes, 0);
                    break;
                case TypeCode.Single:
                    result = BitConverter.ToSingle(bytes, 0);
                    break;
                case TypeCode.UInt16:
                    result = BitConverter.ToUInt16(bytes, 0);
                    break;
                case TypeCode.UInt32:
                    result = BitConverter.ToUInt32(bytes, 0);
                    break;
                case TypeCode.UInt64:
                    result = BitConverter.ToUInt64(bytes, 0);
                    break;
                default:
                    throw new TypeArgumentException("T", string.Format(CultureInfo.InvariantCulture, "T appears to be of an invalid type. Expected type is one of the following: Boolean, Char, Double, Int16, Int32, Int64, UInt16, UInt32 or UInt64. Actually type was {0}.", code));
            }
            return (T)result;
        }
    }
}