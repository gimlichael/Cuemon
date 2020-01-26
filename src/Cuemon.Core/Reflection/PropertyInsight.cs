using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Provides a set of methods for working with reflection related operations on a <see cref="PropertyInfo"/> object in a robust way. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="MemberInsight{T}"/>
    public sealed class PropertyInsight : MemberInsight<PropertyInfo>
    {
        /// <summary>
        /// Creates a new instance of <see cref="PropertyInsight"/> from the specified <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The underlying <see cref="PropertyInfo"/>.</param>
        /// <returns>An instance of <see cref="TypeInsight"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="property"/> cannot be null.
        /// </exception>
        public static PropertyInsight FromProperty(PropertyInfo property)
        {
            return property == null ? null : new PropertyInsight(property);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PropertyInfo"/> to <see cref="PropertyInsight"/>.
        /// </summary>
        /// <param name="property">The <see cref="PropertyInfo"/> to convert.</param>
        /// <returns>A <see cref="PropertyInsight"/> that is equivalent to <paramref name="property"/>.</returns>
        public static implicit operator PropertyInsight(PropertyInfo property)
        {
            return new PropertyInsight(property);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="PropertyInsight"/> to <see cref="PropertyInfo"/>.
        /// </summary>
        /// <param name="insight">The <see cref="PropertyInsight"/> to convert.</param>
        /// <returns>A <see cref="PropertyInfo"/> that is equivalent to <paramref name="insight"/>.</returns>
        public static implicit operator PropertyInfo(PropertyInsight insight)
        {
            return insight.Member;
        }

        PropertyInsight(PropertyInfo property) : base(property)
        {
        }


        /// <summary>
        /// Determines whether the underlying <see cref="PropertyInfo"/> of this instance has been overridden.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="PropertyInfo"/> of this instance has been overridden; otherwise, <c>false</c>.</returns>
        public bool IsOverridden()
        {
            return Member.GetGetMethod().GetBaseDefinition().DeclaringType != Member.DeclaringType;
        }

        /// <summary>
        /// Determines whether the underlying <see cref="PropertyInfo"/> of this instance is considered an automatic property implementation.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="PropertyInfo"/> of this instance is considered an automatic property implementation; otherwise, <c>false</c>.</returns>
        public bool IsAutoProperty()
        {
            if (Member.GetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null ||
                Member.SetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
            {
                return Member.DeclaringType != null && Member.DeclaringType.GetFields(new MemberReflection(excludeInheritancePath: true)).Any(f => f.Name.Contains(FormattableString.Invariant($"<{Member.Name}>")));
            }
            return false;
        }
    }
}