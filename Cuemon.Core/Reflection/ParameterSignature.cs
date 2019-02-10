using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Represent the signature of a parameter to a method, property or similar.
    /// </summary>
    public sealed class ParameterSignature
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterSignature" /> class.
        /// </summary>
        /// <param name="parameterType">The type of the parameter.</param>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="parameterType"/> is null or <br/>
        /// <paramref name="parameterName"/> is null.
        /// </exception>
        /// <exception cref="Cuemon.ArgumentEmptyException">
        /// <paramref name="parameterName"/> is empty.
        /// </exception>
        public ParameterSignature(Type parameterType, string parameterName)
        {
            Validator.ThrowIfNull(parameterType, nameof(parameterType));
            Validator.ThrowIfNullOrEmpty(parameterName, nameof(parameterName));
            ParameterName = parameterName;
            ParameterType = parameterType;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="Type"/> of the parameter.
        /// </summary>
        /// <value>The <see cref="Type"/> of the parameter.</value>
        public Type ParameterType { get; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string ParameterName { get; }

        #endregion

        #region Methods
        /// <summary>
        /// Extracts and converts the <see cref="ParameterInfo"/> sequence to its <see cref="IEnumerable{ParameterSignature}"/> equivalent from <see cref="MethodBase.GetParameters"/>.
        /// </summary>
        /// <param name="method">The <see cref="MethodBase"/> to extract parameter information from.</param>
        /// <returns>An <see cref="IEnumerable{ParameterSignature}"/> that is equivalent to the <see cref="ParameterInfo"/> sequence in <see cref="MethodBase.GetParameters"/>.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="method"/> is null.
        /// </exception>
        public static IEnumerable<ParameterSignature> Parse(MethodBase method)
        {
            Validator.ThrowIfNull(method, nameof(method));
            return Parse(method.GetParameters());
        }

        /// <summary>
        /// Converts the specified <see cref="ParameterInfo" /> sequence to its <see cref="IEnumerable{ParameterSignature}" /> equivalent.
        /// </summary>
        /// <param name="parameters">A sequence of <see cref="ParameterInfo"/>.</param>
        /// <returns>An <see cref="IEnumerable{ParameterSignature}" /> that is equivalent to the <see cref="ParameterInfo" /> sequence.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="parameters" /> is null.
        /// </exception>
        public static IEnumerable<ParameterSignature> Parse(IEnumerable<ParameterInfo> parameters)
        {
            Validator.ThrowIfNull(parameters, nameof(parameters));
            List<ParameterInfo> safeParameters = new List<ParameterInfo>(parameters);
            if (safeParameters.Count == 0) { yield break; }

            int i = 0;
            foreach (ParameterInfo parameter in safeParameters)
            {
                i++;
                yield return new ParameterSignature(parameter.ParameterType, string.IsNullOrEmpty(parameter.Name) ? string.Format(CultureInfo.InvariantCulture, "arg{0}", i) : parameter.Name);
            }
        }
        #endregion
    }
}