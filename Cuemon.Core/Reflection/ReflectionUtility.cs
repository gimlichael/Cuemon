using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Cuemon.Collections.Generic;

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
        /// Specifies that public instance members are to be included in the search.
        /// </summary>
        /// <remarks>Returns the the following <see cref="BindingFlags"/> combination: BindingFlags.Instance | BindingFlags.Public.</remarks>
        public static readonly BindingFlags BindingInstancePublic = BindingFlags.Instance | BindingFlags.Public;

        /// <summary>
        /// Specifies that public and none-public instance members are to be included in the search.
        /// </summary>
        /// <remarks>Returns the the following <see cref="BindingFlags"/> combination: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic.</remarks>
        public static readonly BindingFlags BindingInstancePublicAndPrivate = BindingInstancePublic | BindingFlags.NonPublic;

        /// <summary>
        /// Specifies that public and none-public instance members are to be included in the search. Inherited members are excluded from the search.
        /// </summary>
        /// <remarks>Returns the the following <see cref="BindingFlags"/> combination: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly.</remarks>
        public static readonly BindingFlags BindingInstancePublicAndPrivateNoneInherited = BindingInstancePublicAndPrivate | BindingFlags.DeclaredOnly;

        /// <summary>
        /// Specifies that public and none-public instance and static members are to be included in the search. Inherited members are excluded from the search.
        /// </summary>
        /// <remarks>Returns the the following <see cref="BindingFlags"/> combination: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Static.</remarks>
        public static readonly BindingFlags BindingInstancePublicAndPrivateNoneInheritedIncludeStatic = BindingInstancePublicAndPrivateNoneInherited | BindingFlags.Static;

        /// <summary>
        /// Determines whether the specified <paramref name="method"/> has been overridden.
        /// </summary>
        /// <param name="method">The method to evaluate has been overridden.</param>
        /// <returns><c>true</c> if the specified <paramref name="method"/> has been overridden; otherwise, <c>false</c>.</returns>
	    public static bool IsOverride(MethodInfo method)
        {
            Validator.ThrowIfNull(method, nameof(method));
            return method.GetBaseDefinition().DeclaringType != method.DeclaringType;
        }

        /// <summary>
        /// Determines whether the specified <paramref name="property"/> has been overridden.
        /// </summary>
        /// <param name="property">The property to evaluate has been overridden.</param>
        /// <returns><c>true</c> if the specified <paramref name="property"/> has been overridden; otherwise, <c>false</c>.</returns>
	    public static bool IsOverride(PropertyInfo property)
        {
            Validator.ThrowIfNull(property, nameof(property));
            return IsOverride(property.GetGetMethod());
        }

        /// <summary>
        /// Determines whether the specified <paramref name="property"/> is considered an automatic property implementation.
        /// </summary>
        /// <param name="property">The property to check for automatic property implementation.</param>
        /// <returns><c>true</c> if the specified <paramref name="property"/> is considered an automatic property implementation; otherwise, <c>false</c>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="property"/> is null.
        /// </exception>
	    public static bool IsAutoProperty(PropertyInfo property)
        {
            Validator.ThrowIfNull(property, nameof(property));
            if (property?.GetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null ||
                property?.SetMethod.GetCustomAttribute<CompilerGeneratedAttribute>() != null)
            {
                return property.DeclaringType.GetFields(BindingInstancePublicAndPrivateNoneInheritedIncludeStatic).Any(f => f.Name.Contains("<{0}>".FormatWith(property.Name)));
            }
            return false;
        }

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
            bool hasCircularReference = false;
            int currentDepth = 0;
            Stack<T> stack = new Stack<T>();
            stack.Push(source);
            while (stack.Count != 0 && currentDepth <= maxDepth)
            {
                T current = stack.Pop();
                Type currentType = current.GetType();
                foreach (PropertyInfo property in currentType.GetProperties())
                {
                    if (property.CanRead && property.PropertyType == currentType)
                    {
                        T propertyValue = (T)GetPropertyValue(current, property, propertyIndexParametersResolver);
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
        /// <exception cref="System.ArgumentNullException">
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
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="source"/> is null - or - <paramref name="property"/> is null - or - <paramref name="propertyIndexParametersResolver"/> is null.
        /// </exception>
        public static object GetPropertyValue(object source, PropertyInfo property, Func<ParameterInfo[], object[]> propertyIndexParametersResolver)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            if (propertyIndexParametersResolver == null) { throw new ArgumentNullException(nameof(propertyIndexParametersResolver)); }
            if (!property.CanRead) { return null; }

            ParameterInfo[] indexParameters = property.GetIndexParameters();
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

            object[] indexValues = propertyIndexParametersResolver(indexParameters);
            if (indexValues != null && indexValues.Length > 0)
            {
                try
                {
                    IEnumerable currentAsEnumerable = source as IEnumerable;
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

        /// <summary>
        /// A default callback implementation that evaluates if public property iteration should be skipped in the specified <paramref name="source"/> type.
        /// </summary>
        /// <param name="source">The source type to evaluate.</param>
        /// <returns><c>true</c> if public property iteration should be skipped; otherwise <c>false</c>.</returns>
        public static bool DefaultSkipPropertiesCallback(Type source)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            switch (TypeCodeConverter.FromType(source))
            {
                case TypeCode.Boolean:
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Empty:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.String:
                    return true;
                default:
                    if (TypeUtility.IsKeyValuePair(source)) { return true; }
                    if (TypeUtility.ContainsType(source, typeof(MemberInfo))) { return true; }
                    return false;
            }
        }

        /// <summary>
        /// A default callback implementation that evaluates if the specified <paramref name="property"/> should be skipped for further processing.
        /// </summary>
        /// <param name="property">The property to evaluate.</param>
        /// <returns><c>true</c> if the specified <paramref name="property"/> should be skipped; otherwise <c>false</c>.</returns>
        public static bool DefaultSkipPropertyCallback(PropertyInfo property)
        {
            if (property == null) { throw new ArgumentNullException(nameof(property)); }
            return (property.PropertyType.GetTypeInfo().IsMarshalByRef ||
                property.PropertyType.GetTypeInfo().IsSubclassOf(typeof(Delegate)) ||
                property.Name.Equals("SyncRoot", StringComparison.Ordinal) ||
                property.Name.Equals("IsReadOnly", StringComparison.Ordinal) ||
                property.Name.Equals("IsFixedSize", StringComparison.Ordinal) ||
                property.Name.Equals("IsSynchronized", StringComparison.Ordinal) ||
                property.Name.Equals("Count", StringComparison.Ordinal) ||
                property.Name.Equals("HResult", StringComparison.Ordinal) ||
                property.Name.Equals("TargetSite", StringComparison.Ordinal));
        }

        private static object[] DefaultPropertyIndexParametersResolver(ParameterInfo[] infos)
        {
            List<object> resolvedParameters = new List<object>();
            for (int i = 0; i < infos.Length; i++)
            {
                // because we don't know the values to pass to an indexer we will try to do some assumptions on a "normal" indexer
                // however; this has it flaws: an indexer does not necessarily have an item on 0, 1, 2 etc., so must handle the possible
                // TargetInvocationException.
                // more info? check here: http://blog.nkadesign.com/2006/net-the-limits-of-using-reflection/comment-page-1/#comment-10813
                if (TypeUtility.ContainsType(infos[i].ParameterType, typeof(Byte), typeof(Int16), typeof(Int32), typeof(Int64))) // check to see if we have a "normal" indexer
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
            var options = setup.Configure();
            IDictionary<int, int> referenceSafeguards = new Dictionary<int, int>();
            Stack<Wrapper<object>> stack = new Stack<Wrapper<object>>();

            int index = 0;
            int maxCircularCalls = options.MaxCircularCalls;

            Wrapper<object> current = new Wrapper<object>(source);
            current.Data.Add(IndexKey, index);
            stack.Push(current);

            Hierarchy<object> result = new Hierarchy<object>();
            result.Add(source);

            while (stack.Count != 0)
            {
                current = stack.Pop();
                Type currentType = current.Instance.GetType();
                if (options.SkipPropertyType(currentType))
                {
                    if (index == 0) { continue; }
                    index++;
                    result[(int)current.Data[IndexKey]].Add(current.Instance, current.MemberReference);
                    continue;
                }

                foreach (PropertyInfo property in currentType.GetProperties(BindingInstancePublic))
                {
                    if (options.SkipProperty(property)) { continue; }
                    if (!property.CanRead) { continue; }
                    if (TypeUtility.IsEnumerable(currentType))
                    {
                        if (property.GetIndexParameters().Length > 0) { continue; }
                        if (TypeUtility.IsDictionary(currentType))
                        {
                            if (property.Name == "Keys" || property.Name == "Values") { continue; }
                        }
                    }

                    object propertyValue = GetPropertyValue(current.Instance, property, options.PropertyIndexParametersResolver);
                    if (propertyValue == null) { continue; }
                    index++;
                    result[(int)current.Data[IndexKey]].Add(propertyValue, property);
                    if (TypeUtility.IsComplex(property.PropertyType))
                    {
                        int circularCalls = 0;
                        if (current.Data.ContainsKey(CircularReferenceKey))
                        {
                            circularCalls = (int)current.Data[CircularReferenceKey];
                        }
                        int safetyHashCode = propertyValue.GetHashCode();
                        int calls;
                        if (!referenceSafeguards.TryGetValue(safetyHashCode, out calls)) { referenceSafeguards.Add(safetyHashCode, 0); }
                        if (calls <= maxCircularCalls && result[index].Depth < options.MaxDepth)
                        {
                            referenceSafeguards[safetyHashCode]++;
                            Wrapper<object> wrapper = new Wrapper<object>(propertyValue);
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
        /// <exception cref="ArgumentEmptyException">This exception is thrown if <paramref name="methodName"/> is empty.</exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if either of the following is true:<br/>
        /// the size of <paramref name="methodParameters"/> does not match the resolved parameters size of <paramref name="methodName"/>,<br/>
        /// the type of <paramref name="methodParameters"/> does not match the resolved parameters type of <paramref name="methodName"/>.
        /// </exception>
        /// <remarks>This method auto resolves the associated <see cref="Type"/> for each object in <paramref name="methodParameters"/>. In case of a null referenced parameter, an <see cref="ArgumentNullException"/> is thrown, and you are encouraged to use the overloaded method instead.</remarks>
        public static IDictionary<string, object> ParseMethodParameters(Type source, string methodName, params object[] methodParameters)
        {
            List<Type> methodSignature = new List<Type>();
            if (methodParameters != null)
            {
                foreach (object parameter in methodParameters)
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
        /// <exception cref="ArgumentEmptyException">This exception is thrown if <paramref name="methodName"/> is empty.</exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown if either of the following is true:<br/>
        /// the size of <paramref name="methodParameters"/> does not match the resolved parameters size of <paramref name="methodName"/>,<br/>
        /// the type of <paramref name="methodParameters"/> does not match the resolved parameters type of <paramref name="methodName"/>.
        /// </exception>
        public static IDictionary<string, object> ParseMethodParameters(Type source, string methodName, Type[] methodSignature, params object[] methodParameters)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (methodName == null) { throw new ArgumentNullException(nameof(methodName)); }
            if (methodName.Length == 0) { throw new ArgumentEmptyException(nameof(methodName)); }
            if (methodSignature == null) { throw new ArgumentNullException(nameof(methodSignature)); }
            if (methodParameters != null) { if (methodSignature.Length != methodParameters.Length) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "There is a size mismatch between the method signature and provided parameters. Expected size of the method signature: {0}.", methodParameters.Length), nameof(methodSignature)); } }
            IDictionary<string, object> parsedParameters = new Dictionary<string, object>();
            MethodInfo method = GetMethod(source, methodName, methodSignature);
            if (method != null)
            {
                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length == 0) { return parsedParameters; }
                if (methodParameters == null) { throw new ArgumentNullException(nameof(methodParameters), "Value cannot be null when method has one or more resolved parameters."); }
                if (methodParameters.Length != parameters.Length) { throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "There is a size mismatch between resolved method parameter names and provided parameter values. Expected parameters: {0}.", parameters.Length), nameof(methodParameters)); }
                for (int i = 0; i < parameters.Length; i++)
                {
                    Type parameterType = parameters[i].ParameterType;
                    Type methodParameterType = methodSignature[i];
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
            return GetPropertyAttributeDecorations<TDecoration>(source, BindingInstancePublicAndPrivateNoneInheritedIncludeStatic);
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
            PropertyInfo[] properties = source.GetProperties(bindings);
            foreach (PropertyInfo property in properties)
            {
                Attribute[] attributes = (Attribute[])property.GetCustomAttributes(typeof(TDecoration), false);
                List<TDecoration> decorations = new List<TDecoration>();
                for (int i = 0; i < attributes.Length; i++) { decorations.Add((TDecoration)attributes[i]); }
                attributeDecorations.Add(property, decorations.ToArray());
            }
            return attributeDecorations;
        }

        /// <summary>
        /// Gets a specific field of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a field from.</param>
        /// <param name="fieldName">The name of the field to return.</param>
        /// <returns>An object representing the field with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null -or- <paramref name="fieldName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="fieldName"/> is empty.
        /// </exception>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static FieldInfo GetField(Type source, string fieldName)
        {
            return GetField(source, fieldName, BindingInstancePublicAndPrivateNoneInheritedIncludeStatic);
        }

        /// <summary>
        /// Gets a specific field of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a field from.</param>
        /// <param name="fieldName">The name of the field to return.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the field with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null -or- <paramref name="fieldName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="fieldName"/> is empty.
        /// </exception>
        public static FieldInfo GetField(Type source, string fieldName, BindingFlags bindings)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNullOrEmpty(fieldName, "memberName");
            return source.GetField(fieldName, bindings);
        }

        /// <summary>
		/// Returns all the fields of the specified <paramref name="source"/>.
		/// </summary>
		/// <param name="source">The source to return fields from.</param>
		/// <returns>A sequence of <see cref="FieldInfo"/> objects representing a common search pattern of the specified <paramref name="source"/>.</returns>
		/// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
		public static IEnumerable<FieldInfo> GetFields(Type source)
        {
            return GetFields(source, BindingInstancePublicAndPrivateNoneInheritedIncludeStatic);
        }

        /// <summary>
        /// Returns all the fields of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return fields from.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>A sequence of <see cref="FieldInfo"/> objects representing the search pattern in <paramref name="bindings"/> of the specified <paramref name="source"/>.</returns>
        public static IEnumerable<FieldInfo> GetFields(Type source, BindingFlags bindings)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            FieldInfo[] fields = source.GetFields(bindings);
            foreach (FieldInfo field in fields) { yield return field; }
        }

        /// <summary>
        /// Gets a specific method of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a method from.</param>
        /// <param name="methodName">The name of the method to return.</param>
        /// <returns>An object representing the method with the specified name, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static MethodInfo GetMethod(Type source, string methodName)
        {
            return GetMethod(source, methodName, Type.EmptyTypes);
        }

        /// <summary>
        /// Gets a specific method of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a method from.</param>
        /// <param name="methodName">The name of the method to return.</param>
        /// <param name="methodSignature">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <returns>An object representing the method with the specified name, if found; otherwise, null.</returns>
        public static MethodInfo GetMethod(Type source, string methodName, Type[] methodSignature)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (methodName == null) { throw new ArgumentNullException(nameof(methodName)); }
            if (methodName.Length == 0) { throw new ArgumentException("Value cannot be empty.", nameof(methodName)); }
            return source.GetMethod(methodName, methodSignature);
        }

        /// <summary>
        /// Gets a specific method of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a method from.</param>
        /// <param name="methodName">The name of the method to return.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the method with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null -or- <paramref name="methodName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="methodName"/> is empty.
        /// </exception>
        public static MethodInfo GetMethod(Type source, string methodName, BindingFlags bindings)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNullOrEmpty(methodName, nameof(methodName));
            return source.GetMethod(methodName, bindings);
        }

        /// <summary>
        /// Returns all the methods of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return methods from.</param>
        /// <returns>A sequence of <see cref="MethodInfo"/> objects representing a common search pattern of the specified <paramref name="source"/>.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static IEnumerable<MethodInfo> GetMethods(Type source)
        {
            return GetMethods(source, BindingInstancePublicAndPrivateNoneInheritedIncludeStatic);
        }

        /// <summary>
        /// Returns all the methods of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return methods from.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>A sequence of <see cref="MethodInfo"/> objects representing the search pattern in <paramref name="bindings"/> of the specified <paramref name="source"/>.</returns>
        public static IEnumerable<MethodInfo> GetMethods(Type source, BindingFlags bindings)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            MethodInfo[] methods = source.GetMethods(bindings);
            foreach (MethodInfo method in methods) { yield return method; }
        }

        /// <summary>
        /// Gets a specific property of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a property from.</param>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <returns>An object representing the property with the specified name, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static PropertyInfo GetProperty(Type source, string propertyName)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }
            if (propertyName.Length == 0) { throw new ArgumentException("Value cannot be empty.", nameof(propertyName)); }
            return source.GetProperty(propertyName, BindingInstancePublicAndPrivateNoneInheritedIncludeStatic);
        }

        /// <summary>
        /// Gets a specific property of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a property from.</param>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <param name="propertyReturnSignature">The return <see cref="Type"/> of the property.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static PropertyInfo GetProperty(Type source, string propertyName, Type propertyReturnSignature)
        {
            return GetProperty(source, propertyName, propertyReturnSignature, new Type[0]);
        }

        /// <summary>
        /// Gets a specific property of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a property from.</param>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <param name="propertyReturnSignature">The return <see cref="Type"/> of the property.</param>
        /// <param name="propertySignature">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, null.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static PropertyInfo GetProperty(Type source, string propertyName, Type propertyReturnSignature, Type[] propertySignature)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }
            if (propertyName.Length == 0) { throw new ArgumentException("Value cannot be empty.", nameof(propertyName)); }
            return source.GetProperty(propertyName, propertyReturnSignature, propertySignature ?? new Type[0]);
        }

        /// <summary>
        /// Gets a specific property of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return a property from.</param>
        /// <param name="propertyName">The name of the property to return.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>An object representing the property with the specified name, if found; otherwise, null.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is null -or- <paramref name="propertyName"/> is null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="propertyName"/> is empty.
        /// </exception>
        public static PropertyInfo GetProperty(Type source, string propertyName, BindingFlags bindings)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));
            return source.GetProperty(propertyName, bindings);
        }

        /// <summary>
        /// Returns all the properties of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return properties from.</param>
        /// <returns>A sequence of <see cref="PropertyInfo"/> objects representing a common search pattern of the specified <paramref name="source"/>.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInheritedIncludeStatic"/>.</remarks>
        public static IEnumerable<PropertyInfo> GetProperties(Type source)
        {
            return GetProperties(source, BindingInstancePublicAndPrivateNoneInheritedIncludeStatic);
        }

        /// <summary>
        /// Returns all the properties of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return properties from.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>A sequence of <see cref="PropertyInfo"/> objects representing the search pattern in <paramref name="bindings"/> of the specified <paramref name="source"/>.</returns>
        public static IEnumerable<PropertyInfo> GetProperties(Type source, BindingFlags bindings)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            PropertyInfo[] properties = source.GetProperties(bindings);
            foreach (PropertyInfo property in properties) { yield return property; }
        }

        /// <summary>
        /// Returns all the constructors of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return constructors from.</param>
        /// <returns>A sequence of <see cref="ConstructorInfo"/> objects representing a common search pattern of the specified <paramref name="source"/>.</returns>
        /// <remarks>Searches the <paramref name="source"/> using the following <see cref="BindingFlags"/> combination: <see cref="BindingInstancePublicAndPrivateNoneInherited"/>.</remarks>
	    public static IEnumerable<ConstructorInfo> GetConstructors(Type source)
        {
            return GetConstructors(source, BindingInstancePublicAndPrivateNoneInherited);
        }

        /// <summary>
        /// Returns all the properties of the specified <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source to return constructors from.</param>
        /// <param name="bindings">A bitmask comprised of one or more <see cref="BindingFlags"/> that specify how the search is conducted.</param>
        /// <returns>A sequence of <see cref="ConstructorInfo"/> objects representing the search pattern in <paramref name="bindings"/> of the specified <paramref name="source"/>.</returns>
	    public static IEnumerable<ConstructorInfo> GetConstructors(Type source, BindingFlags bindings)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            ConstructorInfo[] constructors = source.GetConstructors(bindings);
            foreach (ConstructorInfo constructor in constructors) { yield return constructor; }
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
            bool hasNamespaceFilter = !string.IsNullOrEmpty(namespaceFilter);
            bool hasTypeFilter = (typeFilter != null);
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
            foreach (Type type in types)
            {
                if (TypeUtility.ContainsInterface(type, true, typeFilter))
                {
                    yield return type;
                }
            }
        }

        private static IEnumerable<Type> GetAssemblyTypesByBaseType(IEnumerable<Type> types, Type typeFilter)
        {
            foreach (Type type in types)
            {
                if (TypeUtility.ContainsType(type, typeFilter))
                {
                    yield return type;
                }
            }
        }

        private static IEnumerable<Type> GetAssemblyTypesByNamespace(IEnumerable<Type> types, string namespaceFilter)
        {
            foreach (Type type in types)
            {
                if (type.Namespace == null) { continue; }
                if (type.Namespace.Equals(namespaceFilter, StringComparison.OrdinalIgnoreCase))
                {
                    yield return type;
                }
            }
        }

        /// <summary>
        /// Loads the embedded resources from the associated <see cref="Assembly"/> of the specified <see cref="Type"/> following the <see cref="ResourceMatch"/> ruleset of <paramref name="match"/>.
        /// </summary>
        /// <param name="source">The source type to load the resource from.</param>
        /// <param name="name">The name of the resource being requested.</param>
        /// <param name="match">The match ruleset to apply.</param>
        /// <returns>A <see cref="Stream"/> representing the loaded resources; null if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static IEnumerable<Stream> GetEmbeddedResources(Type source, string name, ResourceMatch match)
        {
            Validator.ThrowIfNull(source, nameof(source));
            Validator.ThrowIfNullOrEmpty(name, nameof(name));

            List<Stream> resources = new List<Stream>();

            switch (match)
            {
                case ResourceMatch.Name:
                    resources.Add(GetEmbeddedResource(source, name));
                    break;
                case ResourceMatch.Extension:
                case ResourceMatch.ContainsName:
                case ResourceMatch.ContainsExtension:
                    string[] resourceNames = source.GetTypeInfo().Assembly.GetManifestResourceNames();
                    string matchExtension = Path.GetExtension(name).ToUpperInvariant();
                    switch (match)
                    {
                        case ResourceMatch.ContainsName:
                            foreach (string resourceName in resourceNames)
                            {
                                if (resourceName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > 0)
                                {
                                    resources.Add(GetEmbeddedResource(source, resourceName));
                                }
                            }
                            break;
                        case ResourceMatch.Extension:
                            foreach (string resourceName in resourceNames)
                            {
                                string extension = Path.GetExtension(resourceName).ToUpperInvariant();
                                if (extension == matchExtension)
                                {
                                    resources.Add(GetEmbeddedResource(source, resourceName));
                                }
                            }
                            break;
                        case ResourceMatch.ContainsExtension:
                            foreach (string resourceName in resourceNames)
                            {
                                string extension = Path.GetExtension(resourceName);
                                if (extension.IndexOf(matchExtension, StringComparison.OrdinalIgnoreCase) > 0)
                                {
                                    resources.Add(GetEmbeddedResource(source, resourceName));
                                }
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(match));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(match));
            }

            return resources;
        }


        /// <summary>
        /// Loads the embedded resource from the associated <see cref="Assembly"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="source">The source type to load the resource from.</param>
        /// <param name="name">The case-sensitive name of the resource being requested.</param>
        /// <returns>A <see cref="Stream"/> representing the loaded resource; null if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static Stream GetEmbeddedResource(Type source, string name)
        {
            return GetEmbeddedResource(source, name, ResourceMatch.Name);
        }

        /// <summary>
        /// Loads the embedded resource from the associated <see cref="Assembly"/> of the specified <see cref="Type"/>.
        /// </summary>
        /// <param name="source">The source type to load the resource from.</param>
        /// <param name="name">The case-sensitive name of the resource being requested.</param>
        /// <param name="match">The match ruleset to apply.</param>
        /// <returns>A <see cref="Stream"/> representing the loaded resource; null if no resources were specified during compilation, or if the resource is not visible to the caller.</returns>
        public static Stream GetEmbeddedResource(Type source, string name, ResourceMatch match)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source), "This parameter cannot be null!"); }
            if (name == null) { throw new ArgumentNullException(nameof(name), "This parameter cannot be null!"); }
            if (string.IsNullOrEmpty(name)) { throw new ArgumentException("This parameter cannot be empty!", nameof(name)); }

            Stream resource = null;
            switch (match)
            {
                case ResourceMatch.Name:
                    resource = GetEmbeddedResourceCore(source, name);
                    break;
                case ResourceMatch.Extension:
                case ResourceMatch.ContainsName:
                case ResourceMatch.ContainsExtension:
                    string[] resourceNames = source.GetTypeInfo().Assembly.GetManifestResourceNames();
                    string matchExtension = Path.GetExtension(name).ToUpperInvariant();
                    switch (match)
                    {
                        case ResourceMatch.ContainsName:
                            foreach (string resourceName in resourceNames)
                            {
                                if (resourceName.IndexOf(name, StringComparison.OrdinalIgnoreCase) > 0)
                                {
                                    resource = GetEmbeddedResourceCore(source, resourceName);
                                    break;
                                }
                            }
                            break;
                        case ResourceMatch.Extension:
                            foreach (string resourceName in resourceNames)
                            {
                                string extension = Path.GetExtension(resourceName).ToUpperInvariant();
                                if (extension == matchExtension)
                                {
                                    resource = GetEmbeddedResourceCore(source, resourceName);
                                    break;
                                }
                            }
                            break;
                        case ResourceMatch.ContainsExtension:
                            foreach (string resourceName in resourceNames)
                            {
                                string extension = Path.GetExtension(resourceName);
                                if (extension.IndexOf(matchExtension, StringComparison.OrdinalIgnoreCase) > 0)
                                {
                                    resource = GetEmbeddedResourceCore(source, resourceName);
                                    break;
                                }
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(match));
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(match));
            }
            return resource;
        }

        private static Stream GetEmbeddedResourceCore(Type source, string name)
        {
            return source.GetTypeInfo().Assembly.GetManifestResourceStream(name);
        }
    }

    /// <summary>
    /// Specifies the way of finding and returning an embedded resource.
    /// </summary>
    public enum ResourceMatch
    {
        /// <summary>
        /// Specifies an exact match on the file name of the embedded resource.
        /// </summary>
        Name = 0,
        /// <summary>
        /// Specifies a partial match on the file name of the embedded resource.
        /// </summary>
        ContainsName = 1,
        /// <summary>
        /// Specifies an exact match on the file extension contained within the file name of the embedded resource.
        /// </summary>
        Extension = 2,
        /// <summary>
        /// Specifies a partial match on the file extension contained within the file name of the embedded resource.
        /// </summary>
        ContainsExtension = 3
    }
}