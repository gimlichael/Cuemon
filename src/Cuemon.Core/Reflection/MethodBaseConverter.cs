using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This utility class is designed to make <see cref="MethodBase"/> related conversions easier to work with.
    /// </summary>
    public static class MethodBaseConverter
    {
        /// <summary>
        /// Converts the specified caller information into an instance of a <see cref="MethodBase"/> object.
        /// </summary>
        /// <param name="caller">The <see cref="Type"/> to conduct a search for <paramref name="memberName"/>.</param>
        /// <param name="memberName">The name of the member of <paramref name="caller"/>.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <param name="flags">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, <c>null</c>.</returns>
        public static MethodBase FromType(Type caller, Type[] types = null, [CallerMemberName] string memberName = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public)
        {
            Validator.ThrowIfNull(caller, nameof(caller));
            Validator.ThrowIfNullOrEmpty(memberName, nameof(memberName));
            var methods = caller.GetMethods(flags).Where(info => info.Name.Equals(memberName)).ToList();
            var matchedMethod = Parse(methods, types);
            if (matchedMethod != null) { return matchedMethod; }
            throw new AmbiguousMatchException(string.Format(CultureInfo.InvariantCulture, "Ambiguous matching in method resolution. Found {0} matching the member name of {1}. Consider specifying the signature of the member.", methods.Count, memberName));
        }

        private static MethodInfo Parse(List<MethodInfo> methods, Type[] types)
        {
            if (methods.Count == 0) { return null; }
            if (methods.Count == 1) { return methods[0]; }
            if (methods.Count > 1 && types == null) { return null; }
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == types.Length)
                {
                    bool match = true;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        match &= parameters[i].ParameterType == types[i];
                    }
                    if (match) { return method; }
                }
            }
            return null;
        }
    }
}