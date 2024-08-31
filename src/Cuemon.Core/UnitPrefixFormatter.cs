using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon
{
    /// <summary>
    /// Defines the string formatting of objects having an implementation of either <see cref="IPrefixUnit"/> or <see cref="IUnit"/>.
    /// Implements the <see cref="IFormatProvider" />
    /// Implements the <see cref="ICustomFormatter" />
    /// </summary>
    /// <seealso cref="IFormatProvider" />
    /// <seealso cref="ICustomFormatter" />
    /// <seealso cref="IUnit"/>
    /// <seealso cref="IPrefixUnit"/>
    public class UnitPrefixFormatter : IFormatProvider, ICustomFormatter
    {
        private static readonly string[] BaseFormats = { BitUnit.Name, ByteUnit.Name, BitUnit.Symbol, ByteUnit.Symbol };
        private static readonly IEnumerable<string> MultipleFormats = DecimalPrefix.MetricPrefixes.Where(dp => dp.Multiplier >= 1000).Select(dp => $"{dp.Symbol}{BitUnit.Symbol}")
            .Concat(DecimalPrefix.MetricPrefixes.Where(dp => dp.Multiplier >= 1000).Select(dp => $"{dp.Symbol}{ByteUnit.Symbol}"))
            .Concat(BinaryPrefix.BinaryPrefixes.Select(dp => $"{dp.Symbol}{BitUnit.Symbol}"))
            .Concat(BinaryPrefix.BinaryPrefixes.Select(dp => $"{dp.Symbol}{BitUnit.Name}"))
            .Concat(BinaryPrefix.BinaryPrefixes.Select(dp => $"{dp.Symbol}{ByteUnit.Symbol}"));

        /// <summary>
        /// Returns an object that provides formatting services for the specified type.
        /// </summary>
        /// <param name="formatType">An object that specifies the type of format object to return.</param>
        /// <returns>An instance of the object specified by <paramref name="formatType"/>, if the <see cref="IFormatProvider"/> implementation can supply that type of object; otherwise, null.</returns>
        public object GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) { return this; }
            return null;
        }

        /// <summary>
        /// Converts the value of a specified <paramref name="arg"/> to an equivalent string representation using specified <paramref name="format"/> and culture-specific format <paramref name="formatProvider"/>.
        /// </summary>
        /// <param name="format">A format string containing formatting specifications.</param>
        /// <param name="arg">An object that implements either interface <see cref="IPrefixUnit"/>, <see cref="IUnit"/> or both.</param>
        /// <param name="formatProvider">An object that supplies format information about <paramref name="arg"/>.</param>
        /// <returns>The string representation of the value of <paramref name="arg"/>, formatted as specified by <paramref name="format"/> and <paramref name="formatProvider"/>.</returns>
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            var formats = format.Split(' ');
            if (arg is IPrefixUnit unit && formats.Intersect(MultipleFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, formatProvider); }
            if (arg is IUnit baseUnit && formats.Intersect(BaseFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, baseUnit, formatProvider); }
            return null;
        }

        private static string FormatInterpreter(string[] formats, IPrefixUnit unit, IFormatProvider provider)
        {
            var numberFormat = formats[0].Trim();
            var unitFormat = formats[1].Trim();
            var useCompoundFormat = formats.Last().Trim() == "X";
            return string.Format(provider, "{0} {1}", unit.PrefixValue.ToString(numberFormat, provider), useCompoundFormat ? $"{unit.Prefix.Name}{unit.UnitName}" : unitFormat);
        }

        private static string FormatInterpreter(string[] formats, IUnit unit, IFormatProvider provider)
        {
            var numberFormat = formats[0].Trim();
            var unitFormat = formats[1].Trim();
            var useCompoundFormat = formats.Last().Trim() == "X";
            return string.Format(provider, "{0} {1}", unit.UnitValue.ToString(numberFormat, provider), useCompoundFormat ? $"{unit.UnitName}" : unitFormat);
        }
    }
}