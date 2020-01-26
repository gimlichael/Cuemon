using System;
using System.Linq;
using System.Reflection;

namespace Cuemon.Reflection
{

    /// <summary>
    /// Provides a base class for <see cref="MemberInfo"/> insights.
    /// </summary>
    /// <typeparam name="T">The type of the metadata provider.</typeparam>
    public abstract class MemberInsight<T> where T : MemberInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MemberInsight{T}"/> class.
        /// </summary>
        /// <param name="member">The underlying metadata object.</param>
        protected MemberInsight(T member)
        {
            Member = member;
        }

        /// <summary>
        /// Gets the underlying metadata object.
        /// </summary>
        /// <value>The underlying metadata object.</value>
        public T Member { get; }

        /// <summary>
        /// Determines whether the underlying <see cref="MemberInfo"/> of this instance implements one or more of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="types">The attribute types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the underlying <see cref="MemberInfo"/> of this instance implements one or more of the specified <paramref name="types"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool HasAttribute(params Type[] types)
        {
            foreach (var tt in types) { if (Member.GetCustomAttributes(tt, true).Any()) { return true; } }
            return false;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Member.ToString();
        }
    }

    /// <summary>
    /// Provides a set of methods for working with reflection related operations on a <see cref="MemberInfo"/> object in a robust way. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="MemberInsight{T}"/>
    public sealed class MemberInsight : MemberInsight<MemberInfo>
    {
        /// <summary>
        /// Creates a new instance of <see cref="MemberInsight"/> from the specified <paramref name="member"/>.
        /// </summary>
        /// <param name="member">The underlying <see cref="MemberInfo"/>.</param>
        /// <returns>An instance of <see cref="MemberInsight"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="member"/> cannot be null.
        /// </exception>
        public static MemberInsight FromMember(MemberInfo member)
        {
            Validator.ThrowIfNull(member, nameof(member));
            return new MemberInsight(member);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="MemberInfo"/> to <see cref="MemberInsight"/>.
        /// </summary>
        /// <param name="member">The <see cref="MemberInfo"/> to convert.</param>
        /// <returns>A <see cref="MemberInsight"/> that is equivalent to <paramref name="member"/>.</returns>
        public static implicit operator MemberInsight(MemberInfo member)
        {
            return new MemberInsight(member);
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="MemberInsight"/> to <see cref="MemberInfo"/>.
        /// </summary>
        /// <param name="insight">The <see cref="MemberInsight"/> to convert.</param>
        /// <returns>A <see cref="MemberInfo"/> that is equivalent to <paramref name="insight"/>.</returns>
        public static implicit operator MemberInfo(MemberInsight insight)
        {
            return insight.Member;
        }

        MemberInsight(MemberInfo member) : base(member)
        {
        }
    }
}