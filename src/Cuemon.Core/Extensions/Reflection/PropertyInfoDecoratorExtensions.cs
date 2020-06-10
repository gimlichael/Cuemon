using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Extension methods for the <see cref="PropertyInfo"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class PropertyInfoDecoratorExtensions
    {
        /// <summary>
        /// Determines whether the underlying <see cref="PropertyInfo"/> of the <paramref name="decorator"/> has been overridden.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <returns><c>true</c> if the underlying <see cref="PropertyInfo"/> of the <paramref name="decorator"/> has been overridden; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static bool IsOverridden(this IDecorator<PropertyInfo> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            return decorator.Inner.GetGetMethod().GetBaseDefinition().DeclaringType != decorator.Inner.DeclaringType;
        }

        /// <summary>
        /// Determines whether the underlying <see cref="PropertyInfo"/> of the <paramref name="decorator"/> is considered an automatic property implementation.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <returns><c>true</c> if the underlying <see cref="PropertyInfo"/> of the <paramref name="decorator"/> is considered an automatic property implementation; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static bool IsAutoProperty(this IDecorator<PropertyInfo> decorator)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            if (decorator.Inner.GetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null ||
                decorator.Inner.SetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
            {
                return decorator.Inner.DeclaringType != null && decorator.Inner.DeclaringType.GetFields(new MemberReflection(excludeInheritancePath: true)).Any(f => f.Name.Contains(FormattableString.Invariant($"<{decorator.Inner.Name}>")));
            }
            return false;
        }
    }
}