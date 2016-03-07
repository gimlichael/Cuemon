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
        /// Returns an <see cref="IConvertible"/> primitive converted from the specified array <paramref name="value"/> of bytes.
        /// </summary>
        /// <typeparam name="T">The type of the expected return <paramref name="value"/> after conversion.</typeparam>
        /// <param name="value">The value to convert into an <see cref="IConvertible"/>.</param>
        /// <returns>An <see cref="IConvertible"/> primitive formed by n-bytes beginning at 0.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="T"/> is outside the range of allowed types.<br/>
        /// Allowed types are: <see cref="Boolean"/>, <see cref="Char"/>, <see cref="double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="ushort"/>, <see cref="UInt32"/> and <see cref="UInt64"/>.
        /// </exception>
        public static T FromBytes<T>(byte[] value) where T : struct, IConvertible
        {
            Validator.ThrowIfNull(value, nameof(value));
            if (BitConverter.IsLittleEndian) { Array.Reverse(value); }
            TypeCode code = TypeCodeConverter.FromType(typeof(T));

            object result;
            switch (code)
            {
                case TypeCode.Boolean:
                    result = BitConverter.ToBoolean(value, 0);
                    break;
                case TypeCode.Char:
                    result = BitConverter.ToChar(value, 0);
                    break;
                case TypeCode.Double:
                    result = BitConverter.ToDouble(value, 0);
                    break;
                case TypeCode.Int16:
                    result = BitConverter.ToInt16(value, 0);
                    break;
                case TypeCode.Int32:
                    result = BitConverter.ToInt32(value, 0);
                    break;
                case TypeCode.Int64:
                    result = BitConverter.ToInt64(value, 0);
                    break;
                case TypeCode.Single:
                    result = BitConverter.ToSingle(value, 0);
                    break;
                case TypeCode.UInt16:
                    result = BitConverter.ToUInt16(value, 0);
                    break;
                case TypeCode.UInt32:
                    result = BitConverter.ToUInt32(value, 0);
                    break;
                case TypeCode.UInt64:
                    result = BitConverter.ToUInt64(value, 0);
                    break;
                default:
                    throw new TypeArgumentException("T", string.Format(CultureInfo.InvariantCulture, "T appears to be of an invalid type. Expected type is one of the following: Boolean, Char, Double, Int16, Int32, Int64, UInt16, UInt32 or UInt64. Actually type was {0}.", code));
            }
            return (T)result;
        }
    }
}