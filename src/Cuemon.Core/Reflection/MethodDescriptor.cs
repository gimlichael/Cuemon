using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Provides information about a method, such as its name, parameters and whether its a property or method.
    /// </summary>
    public sealed class MethodDescriptor
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptor" /> class.
        /// </summary>
        /// <param name="method">The method to extract a signature for.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public MethodDescriptor(MethodBase method) : this(method?.DeclaringType, method)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptor" /> class.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="method"/> resides.</param>
        /// <param name="method">The method to extract a signature for.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public MethodDescriptor(Type caller, MethodBase method)
        {
            if (method == null) { throw new ArgumentNullException(nameof(method)); }
            var methodName = string.IsNullOrEmpty(method.Name) ? "NotAvailable" : method.Name;
            var isPresumedProperty = methodName.StartsWith("get_", StringComparison.OrdinalIgnoreCase) | methodName.StartsWith("set_", StringComparison.OrdinalIgnoreCase);
            IsProperty = isPresumedProperty;
            MethodName = isPresumedProperty ? methodName.Remove(0, 4) : methodName;
            Caller = caller;
            Parameters = ParameterSignature.Parse(method);
            HasParameters = Parameters.Any();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptor" /> class.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="methodName"/> resides.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <remarks>This represents a method with no parameters.</remarks>
        public MethodDescriptor(Type caller, string methodName) : this(caller, methodName, new ParameterSignature[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptor" /> class.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="methodName"/> resides.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="isProperty">A value indicating whether the method is a property. Default is <c>false</c>.</param>
        /// <remarks>This represents a method with no parameters or a normal property.</remarks>
        public MethodDescriptor(Type caller, string methodName, bool isProperty) : this(caller, methodName, isProperty, new ParameterSignature[0])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptor" /> class.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="methodName"/> resides.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameters">A sequence of <see cref="ParameterSignature"/> that represent the parameter signature of the method.</param>
        /// <remarks>This represents a method with one or more parameters.</remarks>
        public MethodDescriptor(Type caller, string methodName, params ParameterSignature[] parameters) : this(caller, methodName, false, parameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MethodDescriptor" /> class.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="methodName"/> resides.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="isProperty">A value indicating whether the method is a property. Default is <c>false</c>.</param>
        /// <param name="parameters">A sequence of <see cref="ParameterSignature" /> that represent the parameter signature of the method.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="caller"/> is null or <br/>
        /// <paramref name="methodName"/> is null.
        /// </exception>
        /// <remarks>This represents a method with one or more parameters or a property indexer.</remarks>
        public MethodDescriptor(Type caller, string methodName, bool isProperty, params ParameterSignature[] parameters)
        {
            Validator.ThrowIfNull(caller, nameof(caller));
            Validator.ThrowIfNullOrWhitespace(methodName, nameof(methodName));

            Caller = caller;
            MethodName = methodName;
            IsProperty = isProperty;
            Parameters = parameters ?? new ParameterSignature[0];
            HasParameters = (parameters != null && parameters.Length > 0);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the name of the class where the method is located.
        /// </summary>
        /// <value>The name of the class where the method is located.</value>
        public Type Caller { get; }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        /// <value>The name of the method.</value>
        public string MethodName { get; }

        /// <summary>
        /// Gets the parameter of the method.
        /// </summary>
        /// <value>A sequence of type <see cref="ParameterSignature"/> containing information that matches the signature of the method.</value>
        public IEnumerable<ParameterSignature> Parameters { get; }

        /// <summary>
        /// Gets a value indicating whether the method has parameters.
        /// </summary>
        /// <value><c>true</c> if the method has parameters; otherwise, <c>false</c>.</value>
        public bool HasParameters { get; }

        /// <summary>
        /// Gets a value indicating whether the method is a property.
        /// </summary>
        /// <value><c>true</c> if the method is a property; otherwise, <c>false</c>.</value>
        public bool IsProperty { get; }

        #endregion

        #region Methods
        /// <summary>
        /// Creates and returns a <see cref="MethodDescriptor"/> object and automatically determines the type of the signature (be that method or property).
        /// </summary>
        /// <param name="method">The method to extract a signature for.</param>
        /// <returns>A <see cref="MethodDescriptor"/> object.</returns>
        /// <remarks>Although confusing a property is to be thought of as a method with either one or two methods (Get, Set) contained inside the property declaration.</remarks>
        public static MethodDescriptor Create(MethodBase method)
        {
            return new MethodDescriptor(method);
        }

        /// <summary>
        /// Creates and returns a <see cref="MethodDescriptor"/> object.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="methodName"/> resides.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <returns>A <see cref="MethodDescriptor"/> object.</returns>
        public static MethodDescriptor CreateMethod(Type caller, string methodName)
        {
            return new MethodDescriptor(caller, methodName);
        }

        /// <summary>
        /// Creates and returns a <see cref="MethodDescriptor"/> object.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="methodName"/> resides.</param>
        /// <param name="methodName">The name of the method.</param>
        /// <param name="parameters">A sequence of <see cref="ParameterSignature" /> that represent the parameter signature of the method.</param>
        /// <returns>A <see cref="MethodDescriptor"/> object.</returns>
        public static MethodDescriptor CreateMethod(Type caller, string methodName, params ParameterSignature[] parameters)
        {
            return new MethodDescriptor(caller, methodName, parameters);
        }

        /// <summary>
        /// Creates and returns a <see cref="MethodDescriptor"/> object.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="propertyName"/> resides.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>A <see cref="MethodDescriptor"/> object with <see cref="IsProperty"/> initialized to <c>true</c>.</returns>
        /// <remarks>Although confusing a property is to be thought of as a method with either one or two methods (Get, Set) contained inside the property declaration.</remarks>
        public static MethodDescriptor CreateProperty(Type caller, string propertyName)
        {
            return new MethodDescriptor(caller, propertyName, true);
        }

        /// <summary>
        /// Creates and returns a <see cref="MethodDescriptor"/> object.
        /// </summary>
        /// <param name="caller">The class on which the <paramref name="propertyName"/> resides.</param>
        /// <param name="propertyName">The name of the property.</param>
        /// <param name="parameters">A sequence of <see cref="ParameterSignature" /> that represent the parameter signature of the method.</param>
        /// <returns>A <see cref="MethodDescriptor"/> object with <see cref="IsProperty"/> initialized to <c>true</c>.</returns>
        /// <remarks>Although confusing a property is to be thought of as a method with either one or two methods (Get, Set) contained inside the property declaration.</remarks>
        public static MethodDescriptor CreateProperty(Type caller, string propertyName, params ParameterSignature[] parameters)
        {
            return new MethodDescriptor(caller, propertyName, true, parameters);
        }

        /// <summary>
        /// Merges the <see cref="Parameters"/> signature of this instance with the specified <paramref name="runtimeParameterValues"/>.
        /// </summary>
        /// <param name="runtimeParameterValues">The runtime parameter values.</param>
        /// <returns>An <see cref="IDictionary{TKey,TValue}"/> containing the merged result of the <see cref="Parameters"/> signature of this instance and <paramref name="runtimeParameterValues"/>.</returns>
        public IDictionary<string, object> MergeParameters(params object[] runtimeParameterValues)
        {
            return MergeParameters(Parameters, runtimeParameterValues);
        }

        /// <summary>
        /// Merges the <paramref name="method"/> parameter signature with the specified <paramref name="runtimeParameterValues"/>.
        /// </summary>
        /// <param name="method">The method holding the parameter signature to merge with the runtime parameter values.</param>
        /// <param name="runtimeParameterValues">The runtime parameter values.</param>
        /// <returns>An <see cref="IDictionary{TKey,TValue}"/> containing the merged result of the <paramref name="method"/> parameter signature and <paramref name="runtimeParameterValues"/>.</returns>
        public static IDictionary<string, object> MergeParameters(MethodDescriptor method, params object[] runtimeParameterValues)
        {
            Validator.ThrowIfNull(method, nameof(method));
            return MergeParameters(method.Parameters, runtimeParameterValues);
        }

        /// <summary>
        /// Merges the <paramref name="parameters"/> signature with the specified <paramref name="runtimeParameterValues"/>.
        /// </summary>
        /// <param name="parameters">The parameter signature to merge with the runtime parameter values.</param>
        /// <param name="runtimeParameterValues">The runtime parameter values.</param>
        /// <returns>An <see cref="IDictionary{TKey,TValue}"/> containing the merged result of the <paramref name="parameters"/> signature and <paramref name="runtimeParameterValues"/>.</returns>
        public static IDictionary<string, object> MergeParameters(IEnumerable<ParameterSignature> parameters, params object[] runtimeParameterValues)
        {
            var wrapper = new Dictionary<string, object>();
            if (runtimeParameterValues != null)
            {
                var methodParameters = parameters.ToArray();
                var hasEqualNumberOfParameters = methodParameters.Length == runtimeParameterValues.Length;
                for (var i = 0; i < runtimeParameterValues.Length; i++)
                {
                    wrapper.Add(string.Format(CultureInfo.InvariantCulture, "{0}", hasEqualNumberOfParameters ? methodParameters[i].ParameterName : string.Format(CultureInfo.InvariantCulture, "arg{0}", i + 1)), runtimeParameterValues[i]);
                }
            }
            return wrapper;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the method signature.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents the method signature.</returns>
        public override string ToString()
        {
            return ToString(true);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents the method signature.
        /// </summary>
        /// <param name="fullName">Specify <c>true</c> to use the fully qualified name of the <see cref="Caller"/>; otherwise, <c>false</c> for the simple name.</param>
        /// <returns>A <see cref="string" /> that represents the method signature.</returns>
        /// <remarks>
        /// The returned string has the following format: <br/>
        /// Method without parameters: [<see cref="Caller"/>].[<see cref="MethodName"/>]()<br/>
        /// Method with at least one or more parameter: [<see cref="Caller"/>].[<see cref="MethodName"/>]([<see cref="ParameterSignature.ParameterType"/>] [<see cref="ParameterSignature.ParameterName"/>])<br/><br/>
        /// Property: [<see cref="Caller"/>].[<see cref="MethodName"/>]<br/>
        /// Property with at least one indexer: [<see cref="Caller"/>].[<see cref="MethodName"/>][[<see cref="ParameterSignature.ParameterType"/>] [<see cref="ParameterSignature.ParameterName"/>]]
        /// </remarks>
        public string ToString(bool fullName)
        {
            var className = Caller == null ? "NotAvailable" : TypeInsight.FromType(Caller).ToHumanReadableString(o => o.FullName = fullName);
            var signature = new StringBuilder(string.Concat(className, ".", MethodName));
            if (!IsProperty) { signature.Append("("); }
            if (Parameters.Any())
            {
                if (IsProperty) { signature.Append("["); }
                var parameterCount = Parameters.Count();
                var i = 1;
                foreach (var parameter in Parameters)
                {
                    signature.AppendFormat(CultureInfo.InvariantCulture, "{0} {1}", parameter.ParameterType.Name, parameter.ParameterName);
                    if (i < parameterCount) { signature.Append(", "); }
                    i++;
                }
                if (IsProperty) { signature.Append("]"); }
            }
            if (!IsProperty) { signature.Append(")"); }
            return signature.ToString();
        }
        #endregion
    }
}