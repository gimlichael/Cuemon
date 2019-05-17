using System;
using Cuemon.ComponentModel.TypeConverters;

namespace Cuemon.ComponentModel
{
    /// <summary>
    /// Extension methods for the <see cref="T:IConversion{System.DateTime}"/> interface.
    /// </summary>
    public static class DateTimeTypeConverter
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to its equivalent <see cref="double"/> UNIX Epoch time represenation.
        /// </summary>
        /// <param name="_">The marker interface of a converter having <see cref="DateTime"/> as <paramref name="input"/>.</param>
        /// <param name="input">The <see cref="DateTime"/> to be converted into a <see cref="double"/>.</param>
        /// <returns>A <see cref="double"/> UNIX Epoch time represenation that is equivalent to <paramref name="input"/>.</returns>
        /// <seealso cref="UnixEpochTimeDateTimeConverter"/>
        public static double ToDouble(this IConversion<DateTime> _, DateTime input)
        {
            return Converter<DateTime, double>.UseConverter<UnixEpochTimeDateTimeConverter>(input);
        }
    }
}