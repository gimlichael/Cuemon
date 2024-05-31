using System;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="IPrefixMultiple"/> class hidden behind the <see cref="IDecorator{T}"/> interface.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class PrefixMultipleDecoratorExtensions
    {
        /// <summary>
        /// Converts the enclosed <see cref="IPrefixMultiple"/> of the <paramref name="decorator"/> to an <see cref="IPrefixUnit"/> implementation equivalent.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="unit">The unit of measurement that is used as a standard for measurement of the same kind of quantity.</param>
        /// <param name="setup">The <see cref="UnitFormatOptions"/> which may be configured.</param>
        /// <returns>An <see cref="IPrefixUnit"/> implementation.</returns>
        public static IPrefixUnit ApplyPrefix(this IDecorator<IPrefixMultiple> decorator, IPrefixUnit unit, Action<UnitFormatOptions> setup = null)
        {
            Validator.ThrowIfNull(unit);
            try
            {
                return (IPrefixUnit)Activator.CreateInstance(unit.GetType(), unit.UnitValue, decorator.Inner, setup);
            }
            catch (TargetInvocationException e)
            {
                throw e.InnerException ?? e;
            }
        }
    }
}