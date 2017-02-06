using System;
using System.Collections.Generic;
using System.Globalization;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make common struct related operations easier to work with.
    /// </summary>
    public static class StructUtility
    {
        /// <summary>
        /// A hash code representation for a null value.
        /// </summary>
        public const int HashCodeForNullValue = -1;

        private const int FnvPrime = 0x01000193;
        private const long FnvOffset = 0x811C9DC5;

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of structs implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 32-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="convertibles"/> is null.
        /// </exception>
        public static int GetHashCode32<T>(IEnumerable<T> convertibles) where T : struct, IConvertible
        {
            if (convertibles == null) { throw new ArgumentNullException(nameof(convertibles)); }
            long hash = GetHashCode64(convertibles);
            byte[] temp = ByteConverter.FromConvertibles(hash);
            byte[] result = new byte[4];
            for (int i = (result.Length - 1); i >= 0; i--)
            {
                result[i] = temp[temp.Length - (4 - i)];
            }
            return BitConverter.ToInt32(result, 0);
        }

        /// <summary>
        /// Computes a suitable hash code from the specified sequence of <paramref name="convertibles"/>.
        /// </summary>
        /// <param name="convertibles">A sequence of structs implementing the <see cref="IConvertible"/> interface.</param>
        /// <returns>A 64-bit signed integer that is the hash code of <paramref name="convertibles"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="convertibles"/> is null.
        /// </exception>
        public static long GetHashCode64<T>(IEnumerable<T> convertibles) where T : IConvertible
        {
            if (convertibles == null) { throw new ArgumentNullException(nameof(convertibles)); }
            TypeCode code = TypeCodeConverter.FromType(typeof(T));
            unchecked
            {
                bool skipFnvPrimeMultiplication = false;
                long hash = FnvOffset;
                foreach (T convertible in convertibles)
                {
                    switch (code)
                    {
                        case TypeCode.Boolean:
                            hash ^= convertible.ToByte(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Char:
                            hash ^= convertible.ToChar(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.DateTime:
                            hash ^= unchecked((long)convertible.ToDateTime(CultureInfo.InvariantCulture).Ticks);
                            break;
                        case TypeCode.Decimal:
                            hash ^= unchecked((long)convertible.ToDecimal(CultureInfo.InvariantCulture));
                            break;
                        case TypeCode.Double:
                            hash ^= unchecked((long)convertible.ToDouble(CultureInfo.InvariantCulture));
                            break;
                        case TypeCode.SByte:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                            hash ^= convertible.ToInt32(CultureInfo.InvariantCulture);
                            break;
                        case TypeCode.Int64:
                            hash ^= unchecked((long)convertible.ToInt64(CultureInfo.InvariantCulture));
                            break;
                        case TypeCode.Byte:
                        case TypeCode.UInt16:
                        case TypeCode.UInt32:
                            hash ^= unchecked((long)convertible.ToUInt32(CultureInfo.InvariantCulture));
                            break;
                        case TypeCode.UInt64:
                            hash ^= unchecked((long)convertible.ToUInt64(CultureInfo.InvariantCulture));
                            break;
                        case TypeCode.Single:
                            hash ^= unchecked((long)convertible.ToSingle(CultureInfo.InvariantCulture));
                            break;
                        case TypeCode.String:
                            string value = convertible as string;
                            if (value == null)
                            {
                                hash ^= HashCodeForNullValue;
                            }
                            else
                            {
                                for (int i = 0; i < value.Length; i++)
                                {
                                    hash ^= value[i];
                                    hash *= FnvPrime;
                                    skipFnvPrimeMultiplication = true;
                                }
                            }
                            break;
                    }
                    if (!skipFnvPrimeMultiplication) { hash *= FnvPrime; }
                }
                return hash;
            }
        }
    }
}