using System;

namespace Cuemon
{
    /// <summary>
    /// Represents a unit of measurement for bits and is used with measurement of data.
    /// </summary>
    /// <seealso cref="PrefixUnit" />
    /// <seealso cref="ByteUnit"/>
    /// <seealso cref="BinaryPrefix"/>
    public sealed class BitUnit : PrefixUnit
    {
        /// <summary>
        /// Defines the name of a bit unit.
        /// </summary>
        public const string Name = "bit";

        /// <summary>
        /// Defines the symbol of a bit unit.
        /// </summary>
        public const string Symbol = "b";

        /// <summary>
        /// Performs an implicit conversion from <see cref="BitUnit"/> to <see cref="double"/>.
        /// </summary>
        /// <param name="unit">The <see cref="ByteUnit"/> to convert.</param>
        /// <returns>A <see cref="double"/> that is equivalent to <paramref name="unit"/>.</returns>
        public static implicit operator double(BitUnit unit)
        {
            return unit.UnitValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitUnit"/> struct.
        /// </summary>
        /// <param name="value">The value of this unit expressed as bits.</param>
        /// <param name="prefix">The prefix to associate with this unit.</param>
        /// <param name="setup">The <see cref="UnitFormatOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value" /> is lower than 0 -or-
        /// <paramref name="prefix"/> is less than a metric-multiple <see cref="DecimalPrefix.Kilo"/>.
        /// </exception>
        public BitUnit(double value, IPrefixMultiple prefix, Action<UnitFormatOptions> setup = null) : base(Name, Symbol, value, Validator.CheckParameter(prefix, () =>
        {
            if (prefix != null && prefix != PrefixMultiple.None) { Validator.ThrowIfLowerThan(prefix.Multiplier, DecimalPrefix.Kilo.Multiplier, nameof(prefix.Multiplier), "Prefix multiplier must be greater or equal to 1000."); }
        }), setup)
        {
        }
    }
}