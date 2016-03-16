using System;
using System.Globalization;

namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// This utility class is designed to make JSON related conversions easier to work with.
    /// </summary>
    public static class JsonConverter
    {
        /// <summary>
        /// Represents the null literal as defined in RFC 4627.
        /// </summary>
        public static readonly string NullValue = "null";

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return NullValue;
            }
            return string.Concat("\"", StringUtility.Escape(value), "\"");
        }

        /// <summary>
        /// Returns the boolean value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(bool value)
        {
            return value.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(byte value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <remarks><paramref name="value"/> is converted to a Base64 encoded string.</remarks>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(char value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(DateTime value)
        {
            return StringFormatter.FromDateTime(value, StandardizedDateTimeFormatPattern.Iso8601CompleteDateTimeBasic, 2);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(Guid value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(long value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(sbyte value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(short value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(uint value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(ulong value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(ushort value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the value of a JSON object.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <remarks><paramref name="value"/> is checked and written accordingly by the <see cref="IConvertible"/> interface.</remarks>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(object value)
        {
            return ToString(value, value == null ? null : value.GetType());
        }

        /// <summary>
        /// Returns the value of a JSON object.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <param name="valueType">The type of the <paramref name="value"/>.</param>
        /// <remarks><paramref name="value"/> is checked and written accordingly by the <see cref="IConvertible"/> interface.</remarks>
        /// <returns>A <see cref="string" /> respresentation of <paramref name="value"/>.</returns>
        public static string ToString(object value, Type valueType)
        {
            if (valueType != null)
            {
                switch (valueType.AsCode())
                {
                    case TypeCode.Byte:
                        return ToString(Convert.ToByte(value));
                    case TypeCode.SByte:
                        return ToString(Convert.ToSByte(value));
                    case TypeCode.Int16:
                        return ToString(Convert.ToInt16(value));
                    case TypeCode.Int32:
                        return ToString(Convert.ToInt32(value));
                    case TypeCode.UInt16:
                        return ToString(Convert.ToUInt16(value));
                    case TypeCode.Empty:
                        return NullValue;
                    case TypeCode.Char:
                        return ToString(Convert.ToChar(value));
                    case TypeCode.Boolean:
                        return ToString(Convert.ToBoolean(value));
                    case TypeCode.UInt32:
                        return ToString(Convert.ToUInt32(value));
                    case TypeCode.Double:
                        return ToString(Convert.ToDouble(value));
                    case TypeCode.Single:
                        return ToString(Convert.ToSingle(value));
                    case TypeCode.Decimal:
                        return ToString(Convert.ToDecimal(value));
                    case TypeCode.String:
                        return ToString(Convert.ToString(value));
                    case TypeCode.DateTime:
                        return ToString(Convert.ToDateTime(value));
                    case TypeCode.Int64:
                        return ToString(Convert.ToInt64(value));
                    case TypeCode.UInt64:
                        return ToString(Convert.ToUInt64(value));
                }
            }
            return value == null ? NullValue : ToString(value.ToString());
        }
    }
}