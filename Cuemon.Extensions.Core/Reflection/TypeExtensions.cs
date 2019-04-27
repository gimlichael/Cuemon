using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.Collections.Generic;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Core.Reflection
{
    /// <summary>
    /// Extension methods for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
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

        /// <summary>
        /// Retrieves a collection that represents all the properties defined on a specified <paramref name="type"/> except those defined on <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type to exclude properties on <paramref name="type"/>.</typeparam>
        /// <param name="type">The type that contains the properties to include except those defined on <typeparamref name="T"/>.</param>
        /// <returns>A collection of properties for the specified <paramref name="type"/> except those defined on <typeparamref name="T"/>.</returns>
        public static IEnumerable<PropertyInfo> GetRuntimePropertiesExceptOf<T>(this Type type)
        {
            var baseProperties = typeof(T).GetRuntimeProperties();
            var typeProperties = type.GetRuntimeProperties();
            return typeProperties.Except(baseProperties, DynamicEqualityComparer.Create<PropertyInfo>(pi => StructUtility.GetHashCode32($"{pi.Name}-{pi.PropertyType.Name}"), (x, y) => x.Name == y.Name && x.PropertyType.Name == y.PropertyType.Name));
        }

        /// <summary>
        /// Converts the <see cref="Type"/> to its equivalent string representation.
        /// </summary>
        /// <param name="type">The <see cref="Type"/> to extend.</param>
        /// <returns>A string that contains the fully qualified name of the type, including its namespace, comma delimited with the simple name of the assembly.</returns>
        public static string ToFullNameIncludingAssemblyName(this Type type)
        {
            return $"{type.FullName}, {type.GetTypeInfo().Assembly.GetName().Name}";
        }
    }
}