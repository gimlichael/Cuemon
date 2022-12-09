using System;
using System.Collections.Generic;
using Asp.Versioning;
using Asp.Versioning.Conventions;
using Cuemon.Configuration;

namespace Cuemon.Extensions.Asp.Versioning
{
	/// <summary>
	/// Provides programmatic configuration for the <see cref="ServiceCollectionExtensions.AddRestfulApiVersioning" /> method.
	/// </summary>
	/// <see cref="IValidatableParameterObject" />
	public class RestfulApiVersioningOptions : IValidatableParameterObject
	{
#if NET6_0
        /// <summary>
        /// Initializes a new instance of the <see cref="RestfulApiVersioningOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="RestfulApiVersioningOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ApiVersionSelectorType"/></term>
        ///         <description><c>typeof(CurrentImplementationApiVersionSelector)</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ProblemDetailsFactoryType"/></term>
        ///         <description><c>typeof(RestfulProblemDetailsFactory)</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Conventions"/></term>
        ///         <description><c>new ApiVersionConventionBuilder()</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="DefaultApiVersion"/></term>
        ///         <description><c>ApiVersion.Default</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ParameterName"/></term>
        ///         <description><c>v</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ReportApiVersions"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ValidAcceptHeaders"/></term>
        ///         <description><c>application/json, application/xml, application/vnd</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
#else
		/// <summary>
		/// Initializes a new instance of the <see cref="RestfulApiVersioningOptions"/> class.
		/// </summary>
		/// <remarks>
		/// The following table shows the initial property values for an instance of <see cref="RestfulApiVersioningOptions"/>.
		/// <list type="table">
		///     <listheader>
		///         <term>Property</term>
		///         <description>Initial Value</description>
		///     </listheader>
		///     <item>
		///         <term><see cref="ApiVersionSelectorType"/></term>
		///         <description><c>typeof(CurrentImplementationApiVersionSelector)</c></description>
		///     </item>
		///     <item>
		///         <term><see cref="Conventions"/></term>
		///         <description><c>new ApiVersionConventionBuilder()</c></description>
		///     </item>
		///     <item>
		///         <term><see cref="DefaultApiVersion"/></term>
		///         <description><c>ApiVersion.Default</c></description>
		///     </item>
		///     <item>
		///         <term><see cref="ParameterName"/></term>
		///         <description><c>v</c></description>
		///     </item>
		///     <item>
		///         <term><see cref="ReportApiVersions"/></term>
		///         <description><c>false</c></description>
		///     </item>
		///     <item>
		///         <term><see cref="UseBuiltInRfc7807"/></term>
		///         <description><c>false</c></description>
		///     </item>
		///     <item>
		///         <term><see cref="ValidAcceptHeaders"/></term>
		///         <description><c>application/json, application/xml, application/vnd</c></description>
		///     </item>
		/// </list>
		/// </remarks>
#endif
		public RestfulApiVersioningOptions()
		{
			ApiVersionSelectorType = typeof(CurrentImplementationApiVersionSelector);
#if NET6_0
            ProblemDetailsFactoryType = typeof(RestfulProblemDetailsFactory);
#endif
			Conventions = new ApiVersionConventionBuilder();
			DefaultApiVersion = ApiVersion.Default;
			ParameterName = "v";
			ValidAcceptHeaders = new List<string>()
			{
				"application/json",
				"application/xml",
				"application/vnd"
			};
		}

#if NET6_0
        /// <summary>
        /// Specify what <see cref="IProblemDetailsFactory"/> to set on the <seealso cref="ProblemDetailsFactoryType"/>. Default is <see cref="RestfulProblemDetailsFactory"/>.
        /// </summary>
        /// <typeparam name="T">The type that implements the <see cref="IProblemDetailsFactory"/> interface.</typeparam>
        /// <returns>A reference to this instance so that additional calls can be chained.</returns>
        public RestfulApiVersioningOptions UseProblemDetailsFactory<T>() where T : class, IProblemDetailsFactory
        {
            ProblemDetailsFactoryType = typeof(T);
            return this;
        }
#endif

#if NET7_0
		/// <summary>
		/// Gets or sets a value indicating whether responses will be based on the built in support for RFC 7807.
		/// </summary>
		/// <value><c>true</c> if the responses will be based on the built in support for RFC 7807; otherwise, <c>false</c>.</value>
		public bool UseBuiltInRfc7807 { get; set; } = false;
#endif

		/// <summary>
		/// Gets or sets the valid accept headers used as a filter by <see cref="RestfulApiVersionReader"/>.
		/// </summary>
		/// <value>The valid accept headers used as a filter by <see cref="RestfulApiVersionReader"/>.</value>
		/// <remarks>This option was introduced to have an inclusive filter on what MIME types to consider valid when parsing HTTP Accept headers; for more information have a read at https://github.com/dotnet/aspnet-api-versioning/issues/887</remarks>
		public IList<string> ValidAcceptHeaders { get; set; }

#if NET6_0
        /// <summary>
        /// Gets the implementation type of the <see cref="IProblemDetailsFactory"/>.
        /// </summary>
        /// <value>The implementation type of the <see cref="IProblemDetailsFactory"/>.</value>
        public Type ProblemDetailsFactoryType { get; private set; }
#endif

		/// <summary>
		/// Specify what <see cref="IApiVersionSelector"/> to set on the <seealso cref="ApiVersionSelectorType"/>. Default is <see cref="CurrentImplementationApiVersionSelector"/>.
		/// </summary>
		/// <typeparam name="T">The type that implements the <see cref="IApiVersionSelector"/> interface.</typeparam>
		/// <returns>A reference to this instance so that additional calls can be chained.</returns>
		public RestfulApiVersioningOptions UseApiVersionSelector<T>() where T : class, IApiVersionSelector
		{
			ApiVersionSelectorType = typeof(T);
			return this;
		}

		/// <summary>
		/// Gets the concrete implementation type of a type that implements the <see cref="IApiVersionSelector"/> interface.
		/// </summary>
		/// <value>The concrete implementation type of a type that implements the <see cref="IApiVersionSelector"/> interface.</value>
		/// <remarks>Default is <see cref="CurrentImplementationApiVersionSelector"/>. This can be changed by calling the <see cref="UseApiVersionSelector{T}"/>.</remarks>
		public Type ApiVersionSelectorType { get; private set; }

		/// <summary>
		/// Gets or sets the builder used to define API version conventions.
		/// </summary>
		/// <value>The builder used to define API version conventions.</value>
		public IApiVersionConventionBuilder Conventions { get; set; }

		/// <summary>
		/// Gets or sets the default API version applied to services that do not have explicit versions.
		/// </summary>
		/// <value>The default API version applied to services that do not have explicit versions.</value>
		public ApiVersion DefaultApiVersion { get; set; }

		/// <summary>
		/// Gets or sets the name of the media type parameter to read the service API version from.
		/// </summary>
		/// <value>The name of the media type parameter to read the service API version from.</value>
		public string ParameterName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether requests report the API version compatibility information in responses.
		/// </summary>
		/// <value><c>true</c> if the responses contain API version compatibility information; otherwise, <c>false</c>.</value>
		public bool ReportApiVersions { get; set; }

		/// <summary>
		/// Determines whether the public read-write properties of this instance are in a valid state.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// <see cref="ParameterName"/> cannot be null, empty or consist only of white-space characters - or -
		/// <see cref="ValidAcceptHeaders"/> cannot be null.
		/// </exception>
		/// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
		public void ValidateOptions()
		{
			Validator.ThrowIfObjectInDistress(Condition.IsNull(ParameterName) || Condition.IsEmpty(ParameterName) || Condition.IsWhiteSpace(ParameterName));
			Validator.ThrowIfObjectInDistress(ValidAcceptHeaders == null);
		}
	}
}
