using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="ReflectionUtility"/> class.
    /// </summary>
    public static class ReflectionUtilityExtension
    {
        /// <summary>
        /// Parses and returns a collection of key/value pairs representing the specified <paramref name="methodName"/>.
        /// </summary>
        /// <param name="source">The source to locate the specified <paramref name="methodName"/> in.</param>
        /// <param name="methodName">The name of the method to parse on <paramref name="source"/>.</param>
        /// <param name="methodParameters">A variable number of values passed to the <paramref name="methodName"/> on this instance.</param>
        /// <returns>A collection of key/value pairs representing the specified <paramref name="methodName"/>.</returns>
        /// <remarks>This method will parse the specified <paramref name="methodName"/> for parameter names and tie them with <paramref name="methodParameters"/>.</remarks>
        /// <exception cref="ArgumentNullException">This exception is thrown if <paramref name="methodName"/> is null, if <paramref name="source"/> is null or if <paramref name="methodParameters"/> is null and method has resolved parameters.</exception>
        /// <exception cref="ArgumentEmptyException">This exception is thrown if <paramref name="methodName"/> is empty.</exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if either of the following is true:<br/>
        /// the size of <paramref name="methodParameters"/> does not match the resolved parameters size of <paramref name="methodName"/>,<br/>
        /// the type of <paramref name="methodParameters"/> does not match the resolved parameters type of <paramref name="methodName"/>.
        /// </exception>
        /// <remarks>This method auto resolves the associated <see cref="Type"/> for each object in <paramref name="methodParameters"/>. In case of a null referenced parameter, an <see cref="ArgumentNullException"/> is thrown, and you are encouraged to use the overloaded method instead.</remarks>
        public static IDictionary<string, object> ParseMethodParameters(this Type source, string methodName, params object[] methodParameters)
        {
            return ReflectionUtility.ParseMethodParameters(source, methodName, methodParameters);
        }

        /// <summary>
        /// Parses and returns a collection of key/value pairs representing the specified <paramref name="methodName"/>.
        /// </summary>
        /// <param name="source">The source to locate the specified <paramref name="methodName"/> in.</param>
        /// <param name="methodName">The name of the method to parse on <paramref name="source"/>.</param>
        /// <param name="methodParameters">A variable number of values passed to the <paramref name="methodName"/> on this instance.</param>
        /// <param name="methodSignature">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <returns>A collection of key/value pairs representing the specified <paramref name="methodName"/>.</returns>
        /// <remarks>This method will parse the specified <paramref name="methodName"/> for parameter names and tie them with <paramref name="methodParameters"/>.</remarks>
        /// <exception cref="ArgumentNullException">This exception is thrown if <paramref name="methodName"/> is null, if <paramref name="source"/> is null or if <paramref name="methodParameters"/> is null and method has resolved parameters.</exception>
        /// <exception cref="ArgumentEmptyException">This exception is thrown if <paramref name="methodName"/> is empty.</exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if either of the following is true:<br/>
        /// the size of <paramref name="methodParameters"/> does not match the resolved parameters size of <paramref name="methodName"/>,<br/>
        /// the type of <paramref name="methodParameters"/> does not match the resolved parameters type of <paramref name="methodName"/>.
        /// </exception>
        public static IDictionary<string, object> ParseMethodParameters(this Type source, string methodName, Type[] methodSignature, params object[] methodParameters)
        {
            return ReflectionUtility.ParseMethodParameters(source, methodName, methodSignature, methodParameters);
        }

        /// <summary>
        /// Gets a sequence of the specified <typeparamref name="TDecoration"/> attribute, narrowed to property attribute decorations.
        /// </summary>
        /// <typeparam name="TDecoration">The type of the attribute to locate in <paramref name="source"/>.</typeparam>
        /// <param name="source">The source type to locate <typeparamref name="TDecoration"/> attributes in.</param>
        /// <returns>An <see cref="IEnumerable{TDecoration}"/> of the specified <typeparamref name="TDecoration"/> attributes.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static IDictionary<PropertyInfo, TDecoration[]> GetPropertyAttributes<TDecoration>(this Type source) where TDecoration : Attribute
        {
            return ReflectionUtility.GetPropertyAttributeDecorations<TDecoration>(source);
        }

        /// <summary>
        /// Gets a sequence of the specified <typeparamref name="TDecoration"/> attribute, narrowed to property attribute decorations.
        /// </summary>
        /// <typeparam name="TDecoration">The type of the attribute to locate in <paramref name="source"/>.</typeparam>
        /// <param name="source">The source type to locate <typeparamref name="TDecoration"/> attributes in.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An <see cref="IEnumerable{TDecoration}"/> of the specified <typeparamref name="TDecoration"/> attributes.</returns>
        public static IDictionary<PropertyInfo, TDecoration[]> GetPropertyAttributes<TDecoration>(this Type source, BindingFlags bindings) where TDecoration : Attribute
        {
            return ReflectionUtility.GetPropertyAttributeDecorations<TDecoration>(source, bindings);
        }

        /// <summary>
        /// Gets a specific field of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a field from.</param>
        /// <param name="fieldName">The name of the field to return.</param>
        /// <returns>An object representing the field with the specified name, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static FieldInfo GetField(this Type source, string fieldName)
        {
            return ReflectionUtility.GetField(source, fieldName);
        }

        /// <summary>
        /// Gets a specific field of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a field from.</param>
        /// <param name="fieldName">The name of the field to return.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the field with the specified name, if found; otherwise, null.</returns>
        public static FieldInfo GetField(this Type source, string fieldName, BindingFlags bindings)
        {
            return ReflectionUtility.GetField(source, fieldName, bindings);
        }

        /// <summary>
        /// Returns all the fields of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return fields from.</param>
        /// <returns>A sequence of <see cref="FieldInfo"/> objects representing a common search pattern of the specified <paramref name="source"/>.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static IEnumerable<FieldInfo> GetFields(this Type source)
        {
            return ReflectionUtility.GetFields(source);
        }

        /// <summary>
        /// Returns all the fields of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return fields from.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>A sequence of <see cref="FieldInfo"/> objects representing the search pattern in <paramref name="bindings"/> of the specified <paramref name="source"/>.</returns>
        public static IEnumerable<FieldInfo> GetFields(this Type source, BindingFlags bindings)
        {
            return ReflectionUtility.GetFields(source, bindings);
        }

        /// <summary>
        /// Gets a specific method of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a method from.</param>
        /// <param name="methodName">The name of the method to return.</param>
        /// <returns>An object representing the method with the specified name, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static MethodInfo GetMethod(this Type source, string methodName)
        {
            return ReflectionUtility.GetMethod(source, methodName);
        }

        /// <summary>
        /// Gets a specific method of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a method from.</param>
        /// <param name="methodName">The name of the method to return.</param>
        /// <param name="methodSignature">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <returns>An object representing the method with the specified name, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static MethodInfo GetMethod(this Type source, string methodName, Type[] methodSignature)
        {
            return ReflectionUtility.GetMethod(source, methodName, methodSignature);
        }

        /// <summary>
        /// Returns all the methods of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return methods from.</param>
        /// <returns>A sequence of <see cref="MethodInfo"/> objects representing a common search pattern of the specified <paramref name="source"/>.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static IEnumerable<MethodInfo> GetMethods(this Type source)
        {
            return ReflectionUtility.GetMethods(source);
        }

        /// <summary>
        /// Returns all the methods of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return methods from.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>A sequence of <see cref="MethodInfo"/> objects representing the search pattern in <paramref name="bindings"/> of the specified <paramref name="source"/>.</returns>
        public static IEnumerable<MethodInfo> GetMethods(this Type source, BindingFlags bindings)
        {
            return ReflectionUtility.GetMethods(source, bindings);
        }

        /// <summary>
        /// Gets a specific property of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a property from.</param>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <returns>An object representing the property with the specified name, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static PropertyInfo GetProperty(this Type source, string propertyName)
        {
            return ReflectionUtility.GetProperty(source, propertyName);
        }

        /// <summary>
        /// Gets a specific property of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a property from.</param>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <param name="propertyReturnSignature">The return <see cref="Type"/> of the property.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static PropertyInfo GetProperty(this Type source, string propertyName, Type propertyReturnSignature)
        {
            return ReflectionUtility.GetProperty(source, propertyName, propertyReturnSignature);
        }

        /// <summary>
        /// Gets a specific property of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a property from.</param>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <param name="propertyReturnSignature">The return <see cref="Type"/> of the property.</param>
        /// <param name="propertySignature">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static PropertyInfo GetProperty(this Type source, string propertyName, Type propertyReturnSignature, Type[] propertySignature)
        {
            return ReflectionUtility.GetProperty(source, propertyName, propertyReturnSignature, propertySignature);
        }

        /// <summary>
        /// Returns all the properties of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return properties from.</param>
        /// <returns>A sequence of <see cref="PropertyInfo"/> objects representing a common search pattern of the specified <paramref name="source"/>.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="ReflectionUtility.BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static IEnumerable<PropertyInfo> GetProperties(this Type source)
        {
            return ReflectionUtility.GetProperties(source);
        }

        /// <summary>
        /// Returns all the properties of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return properties from.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>A sequence of <see cref="PropertyInfo"/> objects representing the search pattern in <paramref name="bindings"/> of the specified <paramref name="source"/>.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(this Type source, BindingFlags bindings)
        {
            return ReflectionUtility.GetProperties(source, bindings);
        }

        /// <summary>
        /// Gets the types contained within the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to search the types from.</param>
        /// <returns>A sequence of all <see cref="Type"/> elements from the specified <paramref name="assembly"/>.</returns>
        public static IEnumerable<Type> GetAssemblyTypes(this Assembly assembly)
        {
            return ReflectionUtility.GetAssemblyTypes(assembly);
        }

        /// <summary>
        /// Gets the types contained within the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to search the types from.</param>
        /// <param name="namespaceFilter">The namespace filter to apply on the types in the <paramref name="assembly"/>.</param>
        /// <returns>A sequence of <see cref="Type"/> elements, matching the applied filter, from the specified <paramref name="assembly"/>.</returns>
        public static IEnumerable<Type> GetAssemblyTypes(this Assembly assembly, string namespaceFilter)
        {
            return ReflectionUtility.GetAssemblyTypes(assembly, namespaceFilter);
        }

        /// <summary>
        /// Gets the types contained within the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to search the types from.</param>
        /// <param name="namespaceFilter">The namespace filter to apply on the types in the <paramref name="assembly"/>.</param>
        /// <param name="typeFilter">The type filter to apply on the types in the <paramref name="assembly"/>.</param>
        /// <returns>A sequence of <see cref="Type"/> elements, matching the applied filters, from the specified <paramref name="assembly"/>.</returns>
        public static IEnumerable<Type> GetAssemblyTypes(this Assembly assembly, string namespaceFilter, Type typeFilter)
        {
            return ReflectionUtility.GetAssemblyTypes(assembly, namespaceFilter, typeFilter);
        }

        /// <summary>
        /// Loads the embedded resources from the associated <see cref="Assembly"/> of the specified <see cref="Type"/> following the <see cref="ResourceMatch"/> ruleset of <paramref name="match"/>.
        /// </summary>
        /// <param name="source">The source type to load the resource from.</param>
        /// <param name="name">The name of the resource being requested.</param>
        /// <param name="match">The match ruleset to apply.</param>
        /// <returns>A <see cref="Stream"/> representing the loaded resources; null if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static IEnumerable<Stream> GetEmbeddedResources(this Type source, string name, ResourceMatch match)
        {
            return ReflectionUtility.GetEmbeddedResources(source, name, match);
        }


        /// <summary>
        /// Loads the embedded resource from the associated <see cref="Assembly"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="source">The source type to load the resource from.</param>
        /// <param name="name">The case-sensitive name of the resource being requested.</param>
        /// <returns>A <see cref="Stream"/> representing the loaded resource; null if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static Stream GetEmbeddedResource(this Type source, string name)
        {
            return ReflectionUtility.GetEmbeddedResource(source, name);
        }

        /// <summary>
        /// Loads the embedded resource from the associated <see cref="Assembly"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="source">The source type to load the resource from.</param>
        /// <param name="name">The case-sensitive name of the resource being requested.</param>
        /// <param name="match">The match ruleset to apply.</param>
        /// <returns>A <see cref="Stream"/> representing the loaded resource; null if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static Stream GetEmbeddedResource(this Type source, string name, ResourceMatch match)
        {
            return ReflectionUtility.GetEmbeddedResource(source, name, match);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="property"/> is considered an automatic property implementation.
        /// </summary>
        /// <param name="property">The property to check for automatic property implementation.</param>
        /// <returns><c>true</c> if the specified <paramref name="property"/> is considered an automatic property implementation; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="property"/> is null.
        /// </exception>
        public static bool IsAutoProperty(this PropertyInfo property)
        {
            return ReflectionUtility.IsAutoProperty(property);
        }
    }
}