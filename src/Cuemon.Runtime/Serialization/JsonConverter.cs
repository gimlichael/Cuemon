using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// This utility class is designed to make JSON related conversions easier to work with.
    /// </summary>
    public static class JsonConverter
    {
        private static Func<int, string> _jsonUnicodeConverter = UnicodeEscape;
        private static Func<int, string> _jsonAsciiConverter = JsonEscape;

        /// <summary>
        /// Represents the null literal as defined in RFC 4627.
        /// </summary>
        public static readonly string NullValue = "null";

        /// <summary>
        /// The function delegate that will handle JSON ASCII conversions.
        /// </summary>
        public static Func<int, string> JsonAsciiConverter
        {
            get { return _jsonAsciiConverter; }
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _jsonAsciiConverter = value;
            }
        }

        /// <summary>
        /// The function delegate that will handle JSON Unicode conversions.
        /// </summary>
        public static Func<int, string> JsonUnicodeConverter
        {
            get { return _jsonUnicodeConverter; }
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _jsonUnicodeConverter = value;
            }
        }

        /// <summary>
        /// JSON escapes the specified <paramref name="value"/> using the two function delegates; <see cref="JsonAsciiConverter"/> and <see cref="JsonUnicodeConverter"/>.
        /// </summary>
        /// <param name="value">The <see cref="String"/> to escape.</param>
        /// <returns>A JSON escaped <see cref="string"/> that is equivalent to <paramref name="value"/>.</returns>
        public static string Escape(string value)
        {
            if (value == null) { return NullValue; }
            StringBuilder builder = new StringBuilder(value.Length);
            foreach (char character in value)
            {
                if (ShouldEscape(character))
                {
                    builder.Append(character < byte.MaxValue ? JsonAsciiConverter(character) : JsonUnicodeConverter(character));
                }
                else
                {
                    builder.Append(character);
                }
            }
            return builder.ToString();
        }

        private static string JsonEscape(int c)
        {
            switch (c)
            {
                case 34:
                    return @"\""";
                case 92:
                    return @"\\";
                case 47:
                    return @"\/";
                case 8:
                    return @"\b";
                case 12:
                    return @"\f";
                case 10:
                    return @"\n";
                case 13:
                    return @"\r";
                case 9:
                    return @"\t";
                default:
                    return Convert.ToChar(c).ToString();
            }
        }

        private static string UnicodeEscape(int c)
        {
            return string.Format(CultureInfo.InvariantCulture, "%u{0:x4}", c);
        }

        private static bool ShouldEscape(int c)
        {
            return (c == 34 || c == 92 || c == 47 || c == 8 || c == 12 || c == 10 || c == 13 || c == 9) || (c > 126);
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return NullValue;
            }
            return string.Concat("\"", Escape(value), "\"");
        }

        /// <summary>
        /// Returns the boolean value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(bool value)
        {
            return value.ToString().ToLowerInvariant();
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(byte value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <remarks><paramref name="value"/> is converted to a Base64 encoded string.</remarks>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(byte[] value)
        {
            return Convert.ToBase64String(value);
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(char value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(DateTime value)
        {
            return StringFormatter.FromDateTime(value, StandardizedDateTimeFormatPattern.Iso8601CompleteDateTimeBasic, 2);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(decimal value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(float value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the string value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(Guid value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(long value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(sbyte value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(short value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(uint value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(ulong value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the numeric value of a JSON object as defined in RFC 4627.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
        public static string ToString(ushort value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Returns the value of a JSON object.
        /// </summary>
        /// <param name="value">The value of the JSON object.</param>
        /// <remarks><paramref name="value"/> is checked and written accordingly by the <see cref="IConvertible"/> interface.</remarks>
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
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
        /// <returns>A <see cref="string" /> representation of <paramref name="value"/>.</returns>
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

                if (valueType.IsEnumerable())
                {
                    var enumerableValue = (value as IEnumerable)?.Cast<object>();
                    if (enumerableValue != null)
                    {
                        StringBuilder jsonArray = new StringBuilder("[");
                        jsonArray.Append(enumerableValue.ToDelimitedString(",", ToString));
                        jsonArray.Append("]");
                        return jsonArray.ToString();
                    }
                }
            }
            return value == null ? NullValue : ToString(value.ToString());
        }
    }
}