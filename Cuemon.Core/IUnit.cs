using System;

namespace Cuemon
{
    /// <summary>
    /// Defines a unit of measurement that is used as a standard for measurement of the same kind of quantity.
    /// Implements the <see cref="IEquatable{IUnit}" />
    /// </summary>
    /// <seealso cref="IEquatable{IUnit}" />
    public interface IUnit : IEquatable<IUnit>
    {
        /// <summary>
        /// Gets the name of the base unit.
        /// </summary>
        /// <value>The name of the base unit.</value>
        string UnitName { get; }

        /// <summary>
        /// Gets the symbol of the base unit.
        /// </summary>
        /// <value>The symbol of the base unit.</value>
        string UnitSymbol { get; }

        /// <summary>
        /// Gets the base value of the unit.
        /// </summary>
        /// <value>The base value of the unit.</value>
        double UnitValue { get; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="setup">The <see cref="UnitFormatOptions"/> which may be configured.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        string ToString(Action<UnitFormatOptions> setup);
    }

    /// <summary>
    /// Defines a unit of measurement that is used as a standard for measurement of the same kind of quantity.
    /// Any other quantity of that kind can be expressed as a multiple or fraction of the unit of measurement.
    /// Implements the <see cref="IUnit" />
    /// </summary>
    /// <seealso cref="IUnit" />
    public interface IPrefixUnit : IUnit
    {
        /// <summary>
        /// Gets the prefix that can be either a multiple or a submultiple to the base unit.
        /// </summary>
        /// <value>The prefix that can be either a multiple or a submultiple to the base unit.</value>
        IPrefixMultiple Prefix { get; }

        /// <summary>
        /// Gets the prefix value of the base unit.
        /// </summary>
        /// <value>The prefix value of the base unit.</value>
        double PrefixValue { get; }
    }
}