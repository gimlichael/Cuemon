using System;

namespace Cuemon
{
    /// <summary>
    /// Represents a unit of measurement for bits and is used with measurement of data.
    /// Implements the <see cref="IPrefixUnit" />
    /// </summary>
    /// <seealso cref="IPrefixUnit" />
    /// <seealso cref="ByteUnit"/>
    /// <seealso cref="BinaryPrefix"/>
    public readonly struct BitUnit : IPrefixUnit
    {
        private readonly Action<UnitFormatOptions> _setup;

        /// <summary>
        /// Performs an implicit conversion from <see cref="BitUnit"/> to <see cref="double"/>.
        /// </summary>
        /// <param name="unit">The <see cref="ByteUnit"/> to convert.</param>
        /// <returns>A <see cref="double"/> that is equivalent to <paramref name="unit"/>.</returns>
        public static implicit operator double(BitUnit unit)
        {
            return unit.Value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitUnit"/> struct.
        /// </summary>
        /// <param name="value">The value of this unit expressed as bits½.</param>
        /// <param name="prefix">The prefix to associate with this unit.</param>
        /// <param name="setup">The <see cref="UnitFormatOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value" /> is lower than 0 -or-
        /// <paramref name="prefix"/> is less than a metric-multiple <see cref="DecimalPrefix.Kilo"/>.
        /// </exception>
        public BitUnit(double value, IPrefixMultiple prefix = null, Action<UnitFormatOptions> setup = null) : this()
        {
            Validator.ThrowIfLowerThan(value, 0, nameof(value));
            if (prefix != null) { Validator.ThrowIfLowerThan(prefix.Multiplier, DecimalPrefix.Kilo.Multiplier, nameof(prefix.Multiplier), "Prefix multiplier must be greater or equal to 1000."); }
            Value = value;
            Prefix = prefix;
            PrefixValue = prefix?.ToPrefixValue(this) ?? 0;
            _setup = setup;
        }

        /// <summary>
        /// Gets the name of this unit (bit).
        /// </summary>
        /// <value>The name of this unit.</value>
        public string Name => "bit";

        /// <summary>
        /// Gets the symbol of this unit (b).
        /// </summary>
        /// <value>The symbol of this unit.</value>
        public string Symbol => "b";

        /// <summary>
        /// Gets the value of this unit expressed as bytes.
        /// </summary>
        /// <value>The value of this unit.</value>
        public double Value { get; }

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
            return Value.Equals(other.Value);
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
            var format = Prefix == null ? $"{options.NumberFormat} {Symbol}" : $"{options.NumberFormat} {Prefix.Symbol}{Symbol}";
            return formatter.Format(options.UseCompoundName ? $"{format} X" : format, this, options.FormatProvider);
        }
    }
}