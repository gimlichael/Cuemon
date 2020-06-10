using System;
using System.Linq;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Extension methods for the <see cref="MemberInfo"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class MemberInfoDecoratorExtensions
    {
        /// <summary>
        /// Determines whether the underlying <see cref="MemberInfo"/> of the <paramref name="decorator"/> implements one or more of the specified <paramref name="attributeTypes"/>.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="attributeTypes">The attribute types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the underlying <see cref="MemberInfo"/> of the <paramref name="decorator"/> implements one or more of the specified <paramref name="attributeTypes"/>; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null -or-
        /// <paramref name="attributeTypes"/> cannot be null.
        /// </exception>
        public static bool HasAttribute(this IDecorator<MemberInfo> decorator, params Type[] attributeTypes)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            Validator.ThrowIfNull(attributeTypes, nameof(attributeTypes));
            foreach (var attributeType in attributeTypes) { if (decorator.Inner.GetCustomAttributes(attributeType, true).Any()) { return true; } }
            return false;
        }
    }
}