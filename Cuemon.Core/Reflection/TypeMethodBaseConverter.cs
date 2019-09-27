using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.ComponentModel;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This utility class is designed to make <see cref="MethodBase"/> related conversions easier to work with.
    /// </summary>
    public class TypeMethodBaseConverter : IConverter<Type, MethodBase, MethodBaseOptions>
    {
        public MethodBase ChangeType(Type input, Action<MethodBaseOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            var methods = input.GetMethods(options.Flags).Where(info => info.Name.Equals(options.MemberName, options.Comparison)).ToList();
            var matchedMethod = Parse(methods, options.Types);
            if (matchedMethod != null) { return matchedMethod; }
            throw new AmbiguousMatchException(FormattableString.Invariant($"Ambiguous matching in method resolution. Found {methods.Count} matching the member name of {options.MemberName}. Consider specifying the signature of the member."));
        }

        /// <summary>
        /// Converts the specified caller information into an instance of a <see cref="MethodBase"/> object.
        /// </summary>
        /// <param name="input">The <see cref="Type"/> to conduct a search for <paramref name="memberName"/>.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <param name="memberName">The name of the member of <paramref name="input"/>.</param>
        /// <param name="flags">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, <c>null</c>.</returns>
        public MethodBase DynamicConvert(Type input, Type[] types = null, [CallerMemberName] string memberName = "", BindingFlags flags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, StringComparison comparison = StringComparison.Ordinal)
        {
            return ChangeType(input, o =>
            {
                o.Flags = flags;
                o.MemberName = memberName;
                o.Types = types;
                o.Comparison = comparison;
            });
        }

        private static MethodInfo Parse(IReadOnlyList<MethodInfo> methods, Type[] types)
        {
            if (methods.Count == 0) { return null; }
            if (methods.Count == 1) { return methods[0]; }
            if (methods.Count > 1 && types == null) { return null; }
            foreach (var method in methods)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == types.Length)
                {
                    var match = true;
                    for (var i = 0; i < parameters.Length; i++)
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