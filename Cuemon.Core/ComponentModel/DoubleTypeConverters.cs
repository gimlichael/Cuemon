using System;
using System.ComponentModel;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:IConversion{System.Double}"/> interface.
    /// </summary>
    public static class DoubleTypeConverters
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="T:byte[]"/> representation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="double"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="double"/> to be converted into a <see cref="T:byte[]"/>.</param>
        /// <param name="setup">The <see cref="EndianOptions"/> which may be configured.</param>
        /// <returns>A <see cref="T:byte[]"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <seealso cref="BaseConvertibleConverter"/>
        /// <seealso cref="EndianOptions"/>
        public static byte[] ToByteArray(this IConversion<double> _, double input, Action<EndianOptions> setup = null)
        {
            return Converter<double, byte[], EndianOptions>.UseConverter<BaseConvertibleConverter>(input, setup);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> of an UNIX Epoch time to its equivalent <see cref="DateTime"/> structure.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="double"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="double"/> to be converted into a <see cref="DateTime"/>.</param>
        /// <returns>A <see cref="DateTime"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <seealso cref="UnixEpochTimeDoubleConverter"/>
        public static DateTime ToDateTime(this IConversion<double> _, double input)
        {
            return Converter<double, DateTime>.UseConverter<UnixEpochTimeDoubleConverter>(input);
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> combined with <see cref="TimeUnit"/> to its equivalent <see cref="TimeSpan"/> structure.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="double"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="double"/> to be converted into a <see cref="TimeSpan"/>.</param>
        /// <param name="setup">The <see cref="CompositeDoubleOptions"/> which need to be configured.</param>
        /// <returns>A <see cref="TimeSpan"/> that is equivalent to <paramref name="input"/>.</returns>
        /// <exception cref="OverflowException">
        /// <paramref name="input" /> is either lower than <see cref="long.MinValue"/> or greater than <see cref="long.MaxValue"/>.
        /// </exception>
        /// <exception cref="InvalidEnumArgumentException">
        /// <paramref name="setup"/> was initialzied with an invalid <see cref="TimeUnit"/>.
        /// </exception>
        /// <seealso cref="CompositeDoubleConverter"/>
        /// <seealso cref="CompositeDoubleOptions"/>
        public static TimeSpan ToTimeSpan(this IConversion<double> _, double input, Action<CompositeDoubleOptions> setup)
        {
            return Converter<double, TimeSpan, CompositeDoubleOptions>.UseConverter<CompositeDoubleConverter>(input, setup);
        }
    }
}