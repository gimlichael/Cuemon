using System;

namespace Cuemon
{
    /// <summary>
    /// Provides a way to represent a table of prefixes that precedes a unit of measure to indicate a multiple of the unit.
    /// Implements the <see cref="IEquatable{IUnit}" />
    /// </summary>
    /// <seealso cref="IEquatable{IUnit}" />
    public abstract class MultipleTable : IEquatable<IUnit>
    {
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
        /// <exception cref="ArgumentNullException">
        /// <paramref name="unit"/> cannot be null.
        /// </exception>
        protected MultipleTable(IPrefixUnit unit)
        {
            Validator.ThrowIfNull(unit);
            Unit = unit;
        }

        /// <summary>
        /// Gets the base unit of this table.
        /// </summary>
        /// <value>The base unit of this table.</value>
        public IPrefixUnit Unit { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents largest metric-multiple prefix of this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents largest metric-multiple prefix of this instance.</returns>
        /// <remarks>Evaluates the largest metric-multiple prefix that is greater or equal to 1 and returns that as either <see cref="UnitPrefix.Binary"/> or <see cref="UnitPrefix.Decimal"/> formatted.</remarks>
        public override string ToString()
        {
            return Unit.ToString();
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the <paramref name="other"/> parameter; otherwise, <c>false</c>. </returns>
        public virtual bool Equals(IUnit other)
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
            return Generate.HashCode32(Unit.UnitValue);
        }
    }
}