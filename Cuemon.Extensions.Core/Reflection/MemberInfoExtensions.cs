using System;
using System.Reflection;

namespace Cuemon.Extensions.Core.Reflection
{
    /// <summary>
    /// Extension methods for the <see cref="MemberInfo"/>.
    /// </summary>
    public static class MemberInfoExtensions
    {
        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The member to match against.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified member contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttributes(this MemberInfo source, params Type[] targets)
        {
            return TypeUtility.ContainsAttributeType(source, targets);
        }

        /// <summary>
        /// Determines whether the specified source type contains one or more of the specified attribute target types.
        /// </summary>
        /// <param name="source">The member to match against.</param>
        /// <param name="inherit"><c>true</c> to search the <paramref name="source"/> inheritance chain to find the attributes; otherwise, <c>false</c>.</param>
        /// <param name="targets">The attribute target types to be matched against.</param>
        /// <returns>
        /// 	<c>true</c> if the specified member contains one or more of the specified attribute target types; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasAttributes(this MemberInfo source, bool inherit, params Type[] targets)
        {
            return TypeUtility.ContainsAttributeType(source, inherit, targets);
        }
    }
}