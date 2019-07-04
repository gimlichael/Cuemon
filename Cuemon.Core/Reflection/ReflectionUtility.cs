using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.ComponentModel.Converters;

namespace Cuemon.Reflection
{
    /// <summary>
    /// This utility class is designed to make <see cref="Reflection"/> operations easier to work with.
    /// </summary>
    public static class ReflectionUtility
    {
        private static readonly string CircularReferenceKey = "circularReference";
        private static readonly string IndexKey = "index";

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> has a circular reference.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to check for circular reference.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> has a circular reference; otherwise, <c>false</c>.</returns>
        public static bool HasCircularReference<T>(T source) where T : class
        {
            return HasCircularReference(source, 2);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> has a circular reference.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to check for circular reference.</param>
        /// <param name="maxDepth">The maximum depth to traverse of <paramref name="source"/>.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> has a circular reference; otherwise, <c>false</c>.</returns>
        public static bool HasCircularReference<T>(T source, int maxDepth) where T : class
        {
            return HasCircularReference(source, maxDepth, DefaultPropertyIndexParametersResolver);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="source"/> has a circular reference.
        /// </summary>
        /// <typeparam name="T">The type of the source.</typeparam>
        /// <param name="source">The source to check for circular reference.</param>
        /// <param name="maxDepth">The maximum depth to traverse of <paramref name="source"/>.</param>
        /// <param name="propertyIndexParametersResolver">The function delegate that is invoked if a property has one or more index parameters.</param>
        /// <returns><c>true</c> if the specified <paramref name="source"/> has a circular reference; otherwise, <c>false</c>.</returns>
        public static bool HasCircularReference<T>(T source, int maxDepth, Func<ParameterInfo[], object[]> propertyIndexParametersResolver) where T : class
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (maxDepth <= 0) { throw new ArgumentOutOfRangeException(nameof(maxDepth)); }
            var hasCircularReference = false;
            var currentDepth = 0;
            var stack = new Stack<T>();
            stack.Push(source);
            while (stack.Count != 0 && currentDepth <= maxDepth)
            {
                var current = stack.Pop();
                var currentType = current.GetType();
                foreach (var property in currentType.GetProperties())
                {
                    if (property.CanRead && property.PropertyType == currentType)
                    {
                        var propertyValue = (T)GetPropertyValue(current, property, propertyIndexParametersResolver);
                        stack.Push(propertyValue);
                        hasCircularReference = currentDepth == maxDepth;
                    }
                }
                currentDepth++;
            }
            return hasCircularReference;
        }

        /// <summary>
        /// Gets the property value of a specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source whose property value will be returned.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> to access it's value from.</param>
        /// <returns>The property value of the specified <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="property"/> is null.
        /// </exception>
	    public static object GetPropertyValue(object source, PropertyInfo property)
        {
            return GetPropertyValue(source, property, DefaultPropertyIndexParametersResolver);
        }

        /// <summary>
        /// Gets the property value of a specified <paramref name="source"/> with check for the need of property index values initialized by the specified <paramref name="propertyIndexParametersResolver"/>.
        /// </summary>
        /// <param name="source">The source whose property value will be returned.</param>
        /// <param name="property">The <see cref="PropertyInfo"/> to access it's value from.</param>
        /// <param name="propertyIndexParametersResolver">The function delegate that is invoked if a property has one or more index parameters.</param>
        /// <returns>The property value of the specified <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="property"/> is null - or - <paramref name="propertyIndexParametersResolver"/> is null.
        /// </exception>
        public static object GetPropertyValue(object source, PropertyInfo property, Func<ParameterInfo[], object[]> propertyIndexParametersResolver)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            if (propertyIndexParametersResolver == null) { throw new ArgumentNullException(nameof(propertyIndexParametersResolver)); }
            if (!property.CanRead) { return null; }

            var indexParameters = property.GetIndexParameters();
            if (indexParameters.Length == 0)
            {
                try
                {
                    return property.GetValue(source, null);
                }
                catch (TargetInvocationException) // possible TargetInvocationException for InvalidOperation scenarios and the like of - ignore for now
                {
                }
            }

            var indexValues = propertyIndexParametersResolver(indexParameters);
            if (indexValues != null && indexValues.Length > 0)
            {
                try
                {
                    var currentAsEnumerable = source as IEnumerable;
                    if (currentAsEnumerable != null)
                    {
                        if (!currentAsEnumerable.Cast<object>().Any()) { return null; }
                    }
                    return property.GetValue(source, indexValues); // possible TargetInvocationException as we have no clue what the indexer is (except the assumed one)
                }
                catch (TargetInvocationException)
                {
                }
            }
            return null;
        }

        private static object[] DefaultPropertyIndexParametersResolver(ParameterInfo[] infos)
        {
            var resolvedParameters = new List<object>();
            for (var i = 0; i < infos.Length; i++)
            {
                // because we don't know the values to pass to an indexer we will try to do some assumptions on a "normal" indexer
                // however; this has it flaws: an indexer does not necessarily have an item on 0, 1, 2 etc., so must handle the possible
                // TargetInvocationException.
                // more info? check here: http://blog.nkadesign.com/2006/net-the-limits-of-using-reflection/comment-page-1/#comment-10813
                if (TypeInsight.FromType(infos[i].ParameterType).HasType(typeof(byte), typeof(short), typeof(int), typeof(long))) // check to see if we have a "normal" indexer
                {
                    resolvedParameters.Add(0);
                }
            }
            return resolvedParameters.ToArray();
        }

        /// <summary>
        /// Gets the tree structure of the specified <paramref name="source"/> wrapped in an <see cref="IHierarchy{T}"/> node representing a hierarchical structure.
        /// </summary>
        /// <param name="source">The source whose properties will be traversed while building the hierarchical structure.</param>
        /// <param name="setup">The <see cref="ObjectHierarchyOptions"/> which need to be configured.</param>
        /// <returns>An <see cref="IHierarchy{T}"/> node representing the entirety of a hierarchical structure from the specified <paramref name="source"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null.
        /// </exception>
        public static IHierarchy<object> GetObjectHierarchy(object source, Action<ObjectHierarchyOptions> setup = null)
        {
            Validator.ThrowIfNull(source, nameof(source));
            var options = Patterns.Configure(setup);
            IDictionary<int, int> referenceSafeguards = new Dictionary<int, int>();
            var stack = new Stack<Wrapper<object>>();

            var index = 0;
            var maxCircularCalls = options.MaxCircularCalls;

            var current = new Wrapper<object>(source);
            current.Data.Add(IndexKey, index);
            stack.Push(current);

            var result = new Hierarchy<object>();
            result.Add(source);

            while (stack.Count != 0)
            {
                current = stack.Pop();
                var currentType = current.Instance.GetType();
                if (options.SkipPropertyType(currentType))
                {
                    if (index == 0) { continue; }
                    index++;
                    result[(int)current.Data[IndexKey]].Add(current.Instance, current.MemberReference);
                    continue;
                }

                foreach (var property in currentType.GetProperties(new MemberReflection(true, true)))
                {
                    if (options.SkipProperty(property)) { continue; }
                    if (!property.CanRead) { continue; }
                    var reflector = TypeInsight.FromType(currentType);
                    if (reflector.HasEnumerableContract())
                    {
                        if (property.GetIndexParameters().Length > 0) { continue; }
                        if (reflector.HasDictionaryContract())
                        {
                            if (property.Name == "Keys" || property.Name == "Values") { continue; }
                        }
                    }

                    var propertyValue = GetPropertyValue(current.Instance, property, options.PropertyIndexParametersResolver);
                    if (propertyValue == null) { continue; }
                    index++;
                    result[(int)current.Data[IndexKey]].Add(propertyValue, property);
                    if (TypeInsight.FromType(property.PropertyType).IsComplex())
                    {
                        var circularCalls = 0;
                        if (current.Data.ContainsKey(CircularReferenceKey))
                        {
                            circularCalls = (int)current.Data[CircularReferenceKey];
                        }
                        var safetyHashCode = propertyValue.GetHashCode();
                        int calls;
                        if (!referenceSafeguards.TryGetValue(safetyHashCode, out calls)) { referenceSafeguards.Add(safetyHashCode, 0); }
                        if (calls <= maxCircularCalls && result[index].Depth < options.MaxDepth)
                        {
                            referenceSafeguards[safetyHashCode]++;
                            var wrapper = new Wrapper<object>(propertyValue);
                            wrapper.Data.Add(IndexKey, index);
                            wrapper.Data.Add(CircularReferenceKey, circularCalls + 1);
                            stack.Push(wrapper);
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Parses and returns a collection of key/value pairs representing the specified <paramref name="methodName"/>.
        /// </summary>
        /// <param name="source">The source to locate the specified <paramref name="methodName"/> in.</param>
        /// <param name="methodName">The name of the method to parse on <paramref name="source"/>.</param>
        /// <param name="methodParameters">A variable number of values passed to the <paramref name="methodName"/> on this instance.</param>
        /// <returns>A collection of key/value pairs representing the specified <paramref name="methodName"/>.</returns>
        /// <remarks>This method will parse the specified <paramref name="methodName"/> for parameter names and tie them with <paramref name="methodParameters"/>.</remarks>
        /// <exception cref="ArgumentNullException">This exception is thrown if <paramref name="methodName"/> is null, if <paramref name="source"/> is null or if <paramref name="methodParameters"/> is null and method has resolved parameters.</exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if either of the following is true:<br/>
        /// the size of <paramref name="methodParameters"/> does not match the resolved parameters size of <paramref name="methodName"/>,<br/>
        /// the type of <paramref name="methodParameters"/> does not match the resolved parameters type of <paramref name="methodName"/>.
        /// </exception>
        /// <remarks>This method auto resolves the associated <see cref="Type"/> for each object in <paramref name="methodParameters"/>. In case of a null referenced parameter, an <see cref="ArgumentNullException"/> is thrown, and you are encouraged to use the overloaded method instead.</remarks>
        public static IDictionary<string, object> ParseMethodParameters(Type source, string methodName, params object[] methodParameters)
        {
            var methodSignature = new List<Type>();
            if (methodParameters != null)
            {
                foreach (var parameter in methodParameters)
                {
                    if (parameter == null) { throw new ArgumentNullException(nameof(methodParameters), "One or more method parameters has a null value. Unable to auto resolve associated types for method signature."); }
                    methodSignature.Add(parameter.GetType());
                }
            }
            return ParseMethodParameters(source, methodName, methodSignature.ToArray(), methodParameters);
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
        /// <exception cref="ArgumentException">This exception is thrown if <paramref name="methodName"/> is empty.</exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if either of the following is true:<br/>
        /// the size of <paramref name="methodParameters"/> does not match the resolved parameters size of <paramref name="methodName"/>,<br/>
        /// the type of <paramref name="methodParameters"/> does not match the resolved parameters type of <paramref name="methodName"/>.
        /// </exception>
        public static IDictionary<string, object> ParseMethodParameters(Type source, string methodName, Type[] methodSignature, params object[] methodParameters)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (methodName == null) { throw new ArgumentNullException(nameof(methodName)); }
            if (methodName.Length == 0) { throw new ArgumentException("Value cannot be empty", nameof(methodName)); }
            if (methodSignature == null) { throw new ArgumentNullException(nameof(methodSignature)); }
            if (methodParameters != null) { if (methodSignature.Length != methodParameters.Length) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "There is a size mismatch between the method signature and provided parameters. Expected size of the method signature: {0}.", methodParameters.Length), nameof(methodSignature)); } }
            IDictionary<string, object> parsedParameters = new Dictionary<string, object>();
            var method = source.GetMethod(methodName, methodSignature);
            if (method != null)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 0) { return parsedParameters; }
                if (methodParameters == null) { throw new ArgumentNullException(nameof(methodParameters), "Value cannot be null when method has one or more resolved parameters."); }
                if (methodParameters.Length != parameters.Length) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "There is a size mismatch between resolved method parameter names and provided parameter values. Expected parameters: {0}.", parameters.Length), nameof(methodParameters)); }
                for (var i = 0; i < parameters.Length; i++)
                {
                    var parameterType = parameters[i].ParameterType;
                    var methodParameterType = methodSignature[i];
                    if (parameterType != typeof(object))
                    {
                        if (parameterType != methodParameterType) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "There is a type mismatch between resolved method parameters and provided parameters. First mismatch: {0} != {1}.", parameterType.Name, methodParameterType.Name), nameof(methodParameters)); }
                    }
                    parsedParameters.Add(parameters[i].Name, methodParameters[i]);
                }
            }
            return parsedParameters;
        }

        /// <summary>
        /// Gets a sequence of the specified <typeparamref name="TDecoration"/> attribute, narrowed to property attribute decorations.
        /// </summary>
        /// <typeparam name="TDecoration">The type of the attribute to locate in <paramref name="source"/>.</typeparam>
        /// <param name="source">The source type to locate <typeparamref name="TDecoration"/> attributes in.</param>
        /// <returns>An <see cref="IEnumerable{TDecoration}"/> of the specified <typeparamref name="TDecoration"/> attributes.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static IDictionary<PropertyInfo, TDecoration[]> GetPropertyAttributeDecorations<TDecoration>(Type source) where TDecoration : Attribute
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            return GetPropertyAttributeDecorations<TDecoration>(source, new MemberReflection(excludeInheritancePath: true));
        }

        /// <summary>
        /// Gets a sequence of the specified <typeparamref name="TDecoration"/> attribute, narrowed to property attribute decorations.
        /// </summary>
        /// <typeparam name="TDecoration">The type of the attribute to locate in <paramref name="source"/>.</typeparam>
        /// <param name="source">The source type to locate <typeparamref name="TDecoration"/> attributes in.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An <see cref="IEnumerable{TDecoration}"/> of the specified <typeparamref name="TDecoration"/> attributes.</returns>
        public static IDictionary<PropertyInfo, TDecoration[]> GetPropertyAttributeDecorations<TDecoration>(Type source, BindingFlags bindings) where TDecoration : Attribute
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            IDictionary<PropertyInfo, TDecoration[]> attributeDecorations = new Dictionary<PropertyInfo, TDecoration[]>();
            var properties = source.GetProperties(bindings);
            foreach (var property in properties)
            {
                var attributes = (Attribute[])property.GetCustomAttributes(typeof(TDecoration), false);
                var decorations = new List<TDecoration>();
                for (var i = 0; i < attributes.Length; i++) { decorations.Add((TDecoration)attributes[i]); }
                attributeDecorations.Add(property, decorations.ToArray());
            }
            return attributeDecorations;
        }



        /// <summary>
        /// Gets the types contained within the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to search the types from.</param>
        /// <returns>A sequence of all <see cref="Type"/> elements from the specified <paramref name="assembly"/>.</returns>
        public static IEnumerable<Type> GetAssemblyTypes(Assembly assembly)
        {
            return GetAssemblyTypes(assembly, null);
        }

        /// <summary>
        /// Gets the types contained within the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to search the types from.</param>
        /// <param name="namespaceFilter">The namespace filter to apply on the types in the <paramref name="assembly"/>.</param>
        /// <returns>A sequence of <see cref="Type"/> elements, matching the applied filter, from the specified <paramref name="assembly"/>.</returns>
        public static IEnumerable<Type> GetAssemblyTypes(Assembly assembly, string namespaceFilter)
        {
            return GetAssemblyTypes(assembly, namespaceFilter, null);
        }

        /// <summary>
        /// Gets the types contained within the specified <paramref name="assembly"/>.
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/> to search the types from.</param>
        /// <param name="namespaceFilter">The namespace filter to apply on the types in the <paramref name="assembly"/>.</param>
        /// <param name="typeFilter">The type filter to apply on the types in the <paramref name="assembly"/>.</param>
        /// <returns>A sequence of <see cref="Type"/> elements, matching the applied filters, from the specified <paramref name="assembly"/>.</returns>
        public static IEnumerable<Type> GetAssemblyTypes(Assembly assembly, string namespaceFilter, Type typeFilter)
        {
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }
            var hasNamespaceFilter = !string.IsNullOrEmpty(namespaceFilter);
            var hasTypeFilter = (typeFilter != null);
            IEnumerable<Type> types = assembly.GetTypes();
            if (hasNamespaceFilter || hasTypeFilter)
            {
                if (hasNamespaceFilter) { types = GetAssemblyTypesByNamespace(types, namespaceFilter); }
                if (hasTypeFilter)
                {
                    types = typeFilter.GetTypeInfo().IsInterface ? GetAssemblyTypesByInterfaceType(types, typeFilter) : GetAssemblyTypesByBaseType(types, typeFilter);
                }
            }
            return types;
        }

        private static IEnumerable<Type> GetAssemblyTypesByInterfaceType(IEnumerable<Type> types, Type typeFilter)
        {
            foreach (var type in types)
            {
                if (TypeInsight.FromType(type).HasInterface(typeFilter))
                {
                    yield return type;
                }
            }
        }

        private static IEnumerable<Type> GetAssemblyTypesByBaseType(IEnumerable<Type> types, Type typeFilter)
        {
            foreach (var type in types)
            {
                if (TypeInsight.FromType(type).HasType(typeFilter))
                {
                    yield return type;
                }
            }
        }

        private static IEnumerable<Type> GetAssemblyTypesByNamespace(IEnumerable<Type> types, string namespaceFilter)
        {
            foreach (var type in types)
            {
                if (type.Namespace == null) { continue; }
                if (type.Namespace.Equals(namespaceFilter, StringComparison.OrdinalIgnoreCase))
                {
                    yield return type;
                }
            }
        }
    }
}