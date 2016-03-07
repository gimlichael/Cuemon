using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This is an extension implementation of the <see cref="MethodBaseConverter"/> class.
    /// </summary>
    public static class MethodBaseConverterExtension
    {
        /// <summary>
        /// Converts the specified caller information into an instance of a <see cref="MethodBase"/> object.
        /// </summary>
        /// <param name="caller">The <see cref="Type"/> to conduct a search for <paramref name="memberName"/>.</param>
        /// <param name="memberName">The name of the member of <paramref name="caller"/>.</param>
        /// <param name="flags">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, <c>null</c>.</returns>
        public static MethodBase ToMethodBase(this Type caller, [CallerMemberName] string memberName = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            return MethodBaseConverter.FromType(caller, memberName, flags);
        }

        /// <summary>
        /// Converts the specified caller information into an instance of a <see cref="MethodBase"/> object.
        /// </summary>
        /// <param name="caller">The <see cref="Type"/> to conduct a search for <paramref name="memberName"/>.</param>
        /// <param name="memberName">The name of the member of <paramref name="caller"/>.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <param name="flags">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, <c>null</c>.</returns>
        public static MethodBase ToMethodBase(this Type caller, Type[] types = null, [CallerMemberName] string memberName = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            return MethodBaseConverter.FromType(caller, types, memberName, flags);
        }
    }
}