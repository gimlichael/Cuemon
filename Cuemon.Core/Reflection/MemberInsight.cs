using System;
using System.Linq;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <remarks>A merged word of Reflection and Inspector (after looking at introspection) combined with Info-suffix (which seems to be the standard naming convention for these sorts of classes).</remarks>
    public abstract class MemberInsight<T> where T : MemberInfo
    {
        protected MemberInsight(T member)
        {
            Member = member;
        }

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

    public sealed class MemberInsight : MemberInsight<MemberInfo>
    {
        public static MemberInsight FromMember(MemberInfo member)
        {
            return new MemberInsight(member);
        }

        public MemberInsight(MemberInfo member) : base(member)
        {
        }
    }
}