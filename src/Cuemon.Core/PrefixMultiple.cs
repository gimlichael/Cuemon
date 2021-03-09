using System;

namespace Cuemon
{
    /// <summary>
    /// Represents the base class from which all implementations of unit prefix that can can be expressed as a either a multiple or a submultiple of the unit of measurement should derive.
    /// Implements the <see cref="IPrefixMultiple" />
    /// </summary>
    /// <seealso cref="IPrefixMultiple" />
    public abstract class PrefixMultiple : IPrefixMultiple
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixMultiple"/> struct.
        /// </summary>
        /// <param name="name">The name of the binary unit.</param>
        /// <param name="symbol">The symbol of the the unit prefix.</param>
        /// <param name="value">The number to be raised to a power.</param>
        /// <param name="exponent">The number that specifies a power.</param>
        protected PrefixMultiple(string name, string symbol, double value, double exponent)
        {
            Name = name;
            Symbol = symbol;
            Value = value;
            Multiplier = Math.Pow(value, exponent);
        }

        private double Value { get; }

        /// <summary>
        /// Gets the name of the unit prefix.
        /// </summary>
        /// <value>The name of the unit prefix.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the symbol of the unit prefix.
        /// </summary>
        /// <value>The symbol of the unit prefix.</value>
        public string Symbol { get; }

        /// <summary>
        /// Gets the unit prefix multiplier.
        /// </summary>
        /// <value>The unit prefix multiplier.</value>
        public double Multiplier { get; }

        /// <summary>
        /// Converts the unit base <paramref name="value"/> to a unit prefix value.
        /// </summary>
        /// <param name="value">The value of the base unit.</param>
        /// <returns>A <see cref="double"/> that represents a unit prefix value.</returns>
        public double ToPrefixValue(double value)
        {
            return value / Multiplier;
        }

        /// <summary>
        /// Converts the unit <paramref name="prefixValue"/> back to a unit base value.
        /// </summary>
        /// <param name="prefixValue">The value of the unit prefix.</param>
        /// <returns>A <see cref="double"/> that represents a unit base value.</returns>
        public double ToBaseValue(double prefixValue)
        {
            return prefixValue * Multiplier;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return FormattableString.Invariant($"{Name} ({Symbol}) {Value}^{Math.Log(Multiplier, Value)}");
        }
    }
}