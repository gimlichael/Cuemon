using System;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Provides a set of methods for working with reflection related operations on a <see cref="MethodInfo"/> object in a robust way. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="MemberInsight{T}"/>
    public sealed class MethodInsight : MemberInsight<MethodInfo>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MethodInsight"/> from the specified <paramref name="method"/>.
        /// </summary>
        /// <param name="method">The underlying <see cref="MethodInfo"/>.</param>
        /// <returns>An instance of <see cref="MethodInsight"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> cannot be null.
        /// </exception>
        public static MethodInsight FromMethod(MethodInfo method)
        {
            Validator.ThrowIfNull(method, nameof(method));
            return new MethodInsight(method);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="MethodInfo"/> to <see cref="MethodInsight"/>.
        /// </summary>
        /// <param name="method">The <see cref="MethodInfo"/> to convert.</param>
        /// <returns>A <see cref="MethodInsight"/> that is equivalent to <paramref name="method"/>.</returns>
        public static implicit operator MethodInsight(MethodInfo method)
        {
            return new MethodInsight(method);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="MethodInsight"/> to <see cref="MethodInfo"/>.
        /// </summary>
        /// <param name="insight">The <see cref="MethodInsight"/> to convert.</param>
        /// <returns>A <see cref="MethodInfo"/> that is equivalent to <paramref name="insight"/>.</returns>
        public static implicit operator MethodInfo(MethodInsight insight)
        {
            return insight.Member;
        }

        MethodInsight(MethodInfo method) : base(method)
        {
        }

        /// <summary>
        /// Determines whether the underlying <see cref="MethodInfo"/> of this instance has been overridden.
        /// </summary>
        /// <returns><c>true</c> if the underlying <see cref="MethodInfo"/> of this instance has been overridden; otherwise, <c>false</c>.</returns>
        public bool IsOverridden()
        {
            return Member.GetBaseDefinition().DeclaringType != Member.DeclaringType;
        }
    }
}