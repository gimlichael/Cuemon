using System;
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
        private static readonly string[] BaseFormats = { "bit", "byte", "b", "B" };
        private static readonly string[] KibiFormats = { "Kibit", "Kib", "KiB" };
        private static readonly string[] MebiFormats = { "Mibit", "Mib", "MiB" };
        private static readonly string[] GibiFormats = { "Gibit", "Gib", "GiB" };
        private static readonly string[] TebiFormats = { "Tibit", "Tib", "TiB" };
        private static readonly string[] PebiFormats = { "Pibit", "Pib", "PiB" };
        private static readonly string[] ExbiFormats = { "Exbit", "Eib", "EiB" };
        private static readonly string[] ZebiFormats = { "Zebit", "Zib", "ZiB" };
        private static readonly string[] YobiFormats = { "Yobit", "Yib", "YiB" };
        private static readonly string[] KiloFormats = { "kbit", "kb", "kB" };
        private static readonly string[] MegaFormats = { "Mbit", "Mb", "MB" };
        private static readonly string[] GigaFormats = { "Gbit", "Gb", "GB" };
        private static readonly string[] TeraFormats = { "Tbit", "Tb", "TB" };
        private static readonly string[] PetaFormats = { "Pbit", "Pb", "PB" };
        private static readonly string[] ExaFormats = { "Ebit", "Eb", "EB" };
        private static readonly string[] ZettaFormats = { "Zbit", "Zb", "ZB" };
        private static readonly string[] YottaFormats = { "Ybit", "Yb", "YB" };

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
        /// Converts the value of a specified <paramref name="arg"/> to an equivalent string representation using specified <paramref name="format"/> and culture-specific format <paramref name="provider"/>.
        /// </summary>
        /// <param name="format">A format string containing formatting specifications.</param>
        /// <param name="arg">An object that implements either interface <see cref="IPrefixUnit"/>, <see cref="IUnit"/> or both.</param>
        /// <param name="provider">An object that supplies format information about <paramref name="arg"/>.</param>
        /// <returns>The string representation of the value of <paramref name="arg"/>, formatted as specified by <paramref name="format"/> and <paramref name="provider"/>.</returns>
        public string Format(string format, object arg, IFormatProvider provider)
        {
            var formats = format.Split(' ');
            if (arg is IPrefixUnit unit)
            {
                if (formats.Intersect(KibiFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(MebiFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(GibiFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(TebiFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(PebiFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(ExbiFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(ZebiFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(YobiFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(KiloFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(MegaFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(GigaFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(TeraFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(PetaFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(ExaFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(ZettaFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
                if (formats.Intersect(YottaFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, unit, provider); }
            }
            if (arg is IUnit baseUnit)
            {
                if (formats.Intersect(BaseFormats, StringComparer.Ordinal).Any()) { return FormatInterpreter(formats, baseUnit, provider); }
            }
            return null;
        }

        private string FormatInterpreter(string[] formats, IPrefixUnit unit, IFormatProvider provider)
        {
            var numberFormat = formats[0].Trim();
            var unitFormat = formats[1].Trim();
            var useCompoundFormat = formats.Last().Trim() == "X";
            return string.Format(provider, "{0} {1}", unit.PrefixValue.ToString(numberFormat, provider), useCompoundFormat ? $"{unit.Prefix.Name}{unit.Name}" : unitFormat);
        }

        private string FormatInterpreter(string[] formats, IUnit unit, IFormatProvider provider)
        {
            var numberFormat = formats[0].Trim();
            var unitFormat = formats[1].Trim();
            var useCompoundFormat = formats.Last().Trim() == "X";
            return string.Format(provider, "{0} {1}", unit.Value.ToString(numberFormat, provider), useCompoundFormat ? $"{unit.Name}" : unitFormat);
        }
    }
}