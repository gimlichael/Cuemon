using System;
using System.Text;

namespace Cuemon
{
    /// <summary>
    /// Provides a way to represent a table of both binary and metric prefixes that precedes a unit of measure to indicate a multiple of the unit.
    /// Implements the <see cref="IEquatable{IUnit}" />
    /// </summary>
    /// <seealso cref="IEquatable{IUnit}" />
    public abstract class MultipleTable : IEquatable<IUnit>
    {
        private readonly Action<MultipleTableOptions> _setup;
        private readonly Action<UnitFormatOptions> _us;

        /// <summary>
        /// Performs an implicit conversion from <see cref="MultipleTable"/> to <see cref="double"/>.
        /// </summary>
        /// <param name="mt">The <see cref="MultipleTable"/> to convert.</param>
        /// <returns>A <see cref="double"/> that is equivalent to <paramref name="mt"/>.</returns>
        public static implicit operator double(MultipleTable mt)
        {
            return mt.Unit.UnitValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleTable"/> class.
        /// </summary>
        /// <param name="unit">The instance of an object that implements the <see cref="IPrefixUnit"/> interface.</param>
        /// <param name="setup">The <see cref="MultipleTableOptions"/> which may be configured.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="unit"/> cannot be null.
        /// </exception>
        protected MultipleTable(IPrefixUnit unit, Action<MultipleTableOptions> setup = null)
        {
            Validator.ThrowIfNull(unit, nameof(unit));
            Unit = unit;
            _setup = setup;
            _us = Patterns.ConfigureExchange<MultipleTableOptions, UnitFormatOptions>(_setup);
        }

        /// <summary>
        /// Gets the base unit of this table.
        /// </summary>
        /// <value>The base unit of this table.</value>
        public IPrefixUnit Unit { get; }

        /// <summary>
        /// Gets the binary-multiple prefix kibi (Ki).
        /// </summary>
        /// <value>The binary-multiple prefix kibi (Ki).</value>
        public IPrefixUnit Kibi => BinaryPrefix.Kibi.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the binary-multiple prefix mebi (Mi).
        /// </summary>
        /// <value>The binary-multiple prefix mebi (Mi).</value>
        public IPrefixUnit Mebi => BinaryPrefix.Mebi.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the binary-multiple prefix gibi (Gi).
        /// </summary>
        /// <value>The binary-multiple prefix gibi (Gi).</value>
        public IPrefixUnit Gibi => BinaryPrefix.Gibi.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the binary-multiple prefix tebi (Ti).
        /// </summary>
        /// <value>The binary-multiple prefix tebi (Ti).</value>
        public IPrefixUnit Tebi => BinaryPrefix.Tebi.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the binary-multiple prefix pebi (Pi).
        /// </summary>
        /// <value>The binary-multiple prefix pebi (Pi).</value>
        public IPrefixUnit Pebi => BinaryPrefix.Pebi.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the metric-multiple prefix kilo (k).
        /// </summary>
        /// <value>The metric-multiple prefix kilo (k).</value>
        public IPrefixUnit Kilo => DecimalPrefix.Kilo.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the metric-multiple prefix mega (M).
        /// </summary>
        /// <value>The metric-multiple prefix mega (M).</value>
        public IPrefixUnit Mega => DecimalPrefix.Mega.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the metric-multiple prefix giga (G).
        /// </summary>
        /// <value>The metric-multiple prefix giga (G).</value>
        public IPrefixUnit Giga => DecimalPrefix.Giga.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the metric-multiple prefix tera (T).
        /// </summary>
        /// <value>The metric-multiple prefix tera (T).</value>
        public IPrefixUnit Tera => DecimalPrefix.Tera.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Gets the metric-multiple prefix peta (P).
        /// </summary>
        /// <value>The metric-multiple prefix peta (P).</value>
        public IPrefixUnit Peta => DecimalPrefix.Peta.ApplyPrefix(Unit, _us);

        /// <summary>
        /// Returns a <see cref="string" /> that represents largest metric-multiple prefix of this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents largest metric-multiple prefix of this instance.</returns>
        /// <remarks>Evaluates the largest metric-multiple prefix that is greater or equal to 1 and returns that as either <see cref="UnitPrefix.Binary"/> or <see cref="UnitPrefix.Decimal"/> formatted.</remarks>
        public override string ToString()
        {
            var options = Patterns.Configure(_setup);
            if (Peta.PrefixValue >= 1) { return options.Prefix == UnitPrefix.Binary ? Pebi.ToString() : Peta.ToString(); }
            if (Tera.PrefixValue >= 1) { return options.Prefix == UnitPrefix.Binary ? Tebi.ToString() : Tera.ToString(); }
            if (Giga.PrefixValue >= 1) { return options.Prefix == UnitPrefix.Binary ? Gibi.ToString() : Giga.ToString(); }
            if (Mega.PrefixValue >= 1) { return options.Prefix == UnitPrefix.Binary ? Mebi.ToString() : Mega.ToString(); }
            if (Kilo.PrefixValue >= 1) { return options.Prefix == UnitPrefix.Binary ? Kibi.ToString() : Kilo.ToString(); }
            return Unit.ToString();
        }

        /// <summary>
        /// Returns an aggregated <see cref="string" /> of all multiple prefix of this instance.
        /// </summary>
        /// <param name="includePowerOfTwoGroup">if set to <c>true</c> all binary-multiple prefix is included in the aggregate.</param>
        /// <param name="includePowerOfTenGroup">if set to <c>true</c> all metric-multiple prefix is included in the aggregate.</param>
        /// <param name="includeUnit">if set to <c>true</c> the base unit is included in the aggregate.</param>
        /// <returns>System.String.</returns>
        public string ToAggregateString(bool includePowerOfTwoGroup = true, bool includePowerOfTenGroup = true, bool includeUnit = true)
        {
            var sb = new StringBuilder();
            if (includePowerOfTwoGroup)
            {
                sb.AppendLine(Pebi.ToString());
                sb.AppendLine(Tebi.ToString());
                sb.AppendLine(Gibi.ToString());
                sb.AppendLine(Mebi.ToString());
                sb.AppendLine(Kibi.ToString());
            }
            if (includePowerOfTenGroup)
            {
                sb.AppendLine(Peta.ToString());
                sb.AppendLine(Tera.ToString());
                sb.AppendLine(Giga.ToString());
                sb.AppendLine(Mega.ToString());
                sb.AppendLine(Kilo.ToString());
            }
            if (includeUnit) { sb.AppendLine(Unit.ToString()); }
            return sb.ToString();
        }


        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(IUnit other)
        {
            if (other == null) { return false; }
            return Unit.UnitValue.Equals(other.UnitValue);
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) { return false; }
            if (ReferenceEquals(this, obj)) { return true; }
            if (obj.GetType() != GetType()) { return false; }
            return Equals(((MultipleTable)obj).Unit);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Unit != null ? Unit.UnitValue.GetHashCode() : 0;
        }
    }
}