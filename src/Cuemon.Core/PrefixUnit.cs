using System;

namespace Cuemon
{
    /// <summary>
    /// Represents the base class from which all implementations of a unit of measurement should derive.
    /// Implements the <see cref="IPrefixUnit" />
    /// </summary>
    /// <seealso cref="IPrefixUnit" />
    public abstract class PrefixUnit : IPrefixUnit
    {
        private readonly Action<UnitFormatOptions> _setup;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteUnit"/> struct.
        /// </summary>
        /// <param name="name">The name of the unit.</param>
        /// <param name="symbol">The symbol of the unit.</param>
        /// <param name="value">The value of this unit expressed as bytes.</param>
        /// <param name="prefix">The prefix to associate with this unit.</param>
        /// <param name="setup">The <see cref="UnitFormatOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> cannot be null -or-
        /// <paramref name="symbol"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> cannot be empty or consist only of white-space characters -or-
        /// <paramref name="symbol"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value" /> is lower than 0.
        /// </exception>
        protected PrefixUnit(string name, string symbol, double value, IPrefixMultiple prefix, Action<UnitFormatOptions> setup = null)
        {
            Validator.ThrowIfNullOrWhitespace(name);
            Validator.ThrowIfNullOrWhitespace(symbol);
            Validator.ThrowIfLowerThan(value, 0, nameof(value));

            UnitName = name;
            UnitSymbol = symbol;
            UnitValue = value;
            Prefix = prefix ?? PrefixMultiple.None;
            PrefixValue = Prefix.ToPrefixValue(value);
            _setup = setup;
        }

        /// <summary>
        /// Gets the name of this unit.
        /// </summary>
        /// <value>The name of this unit.</value>
        public string UnitName { get; }

        /// <summary>
        /// Gets the symbol of this unit.
        /// </summary>
        /// <value>The symbol of this unit.</value>
        public string UnitSymbol { get; }

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
        public virtual bool Equals(IUnit other)
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
            var format = Prefix == PrefixMultiple.None ? $"{options.NumberFormat} {UnitSymbol}" : $"{options.NumberFormat} {Prefix.Symbol}{UnitSymbol}";
            return formatter.Format(options.Style == NamingStyle.Compound ? $"{format} X" : format, this, options.FormatProvider);
        }
    }
}