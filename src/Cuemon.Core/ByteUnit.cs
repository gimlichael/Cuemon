using System;

namespace Cuemon
{
    /// <summary>
    /// Represents a unit of measurement for bytes and is used with measurement of data.
    /// Implements the <see cref="IPrefixUnit" />
    /// </summary>
    /// <seealso cref="IPrefixUnit" />
    /// <seealso cref="BitUnit"/>
    /// <seealso cref="BinaryPrefix"/>
    public readonly struct ByteUnit : IPrefixUnit
    {
        private readonly Action<UnitFormatOptions> _setup;

        /// <summary>
        /// Defines how many bits is needed for one byte.
        /// </summary>
        public const int BitsPerByte = 8;

        /// <summary>
        /// Defines how many bits is needed for one nibble (one hexadecimal digit).
        /// </summary>
        public const int BitsPerNibble = BitsPerByte / 2;

        /// <summary>
        /// Defines the name of a byte unit.
        /// </summary>
        public const string Name = "byte";

        /// <summary>
        /// Defines the symbol of a byte unit.
        /// </summary>
        public const string Symbol = "B";

        /// <summary>
        /// Performs an implicit conversion from <see cref="ByteUnit"/> to <see cref="double"/>.
        /// </summary>
        /// <param name="unit">The <see cref="ByteUnit"/> to convert.</param>
        /// <returns>A <see cref="double"/> that is equivalent to <paramref name="unit"/>.</returns>
        public static implicit operator double(ByteUnit unit)
        {
            return unit.UnitValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteUnit"/> struct.
        /// </summary>
        /// <param name="value">The value of this unit expressed as bytes.</param>
        /// <param name="prefix">The prefix to associate with this unit.</param>
        /// <param name="setup">The <see cref="UnitFormatOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value" /> is lower than 0 -or-
        /// <paramref name="prefix"/> is less than a metric-multiple <see cref="DecimalPrefix.Kilo"/>.
        /// </exception>
        public ByteUnit(double value, IPrefixMultiple prefix = null, Action<UnitFormatOptions> setup = null) : this()
        {
            Validator.ThrowIfLowerThan(value, 0, nameof(value));
            if (prefix != null) { Validator.ThrowIfLowerThan(prefix.Multiplier, DecimalPrefix.Kilo.Multiplier, nameof(prefix.Multiplier), "Prefix multiplier must be greater or equal to 1000."); }
            UnitValue = value;
            Prefix = prefix;
            PrefixValue = prefix?.ToPrefixValue(this) ?? 0;
            _setup = setup;
        }

        /// <summary>
        /// Gets the name of this unit (byte).
        /// </summary>
        /// <value>The name of this unit.</value>
        public string UnitName => Name;

        /// <summary>
        /// Gets the symbol of this unit (B).
        /// </summary>
        /// <value>The symbol of this unit.</value>
        public string UnitSymbol => Symbol;

        /// <summary>
        /// Gets the value of this unit expressed as bytes.
        /// </summary>
        /// <value>The value of this unit.</value>
        public double UnitValue { get; }

        /// <summary>
        /// Gets the prefix multiple to this unit.
        /// </summary>
        /// <value>The prefix multiple to this unit.</value>
        public IPrefixMultiple Prefix { get; }

        /// <summary>
        /// Gets the prefix value of this unit.
        /// </summary>
        /// <value>The prefix value of this unit.</value>
        public double PrefixValue { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(IUnit other)
        {
            if (other == null) { return false; }
            return UnitValue.Equals(other.UnitValue);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString(_setup);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="setup">The <see cref="UnitFormatOptions" /> which may be configured.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public string ToString(Action<UnitFormatOptions> setup)
        {
            var options = Patterns.Configure(setup);
            var formatter = new UnitPrefixFormatter();
            var format = Prefix == null ? $"{options.NumberFormat} {UnitSymbol}" : $"{options.NumberFormat} {Prefix.Symbol}{UnitSymbol}";
            return formatter.Format(options.Style == NamingStyle.Compound ? $"{format} X" : format, this, options.FormatProvider);
        }
    }
}