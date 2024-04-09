using System;
using System.Collections.Generic;
using System.Globalization;
using Cuemon.Configuration;
using Cuemon.Extensions.YamlDotNet.Converters;
using Cuemon.Reflection;
using Cuemon.Text;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Cuemon.Extensions.YamlDotNet
{
    /// <summary>
    /// Configuration options for <see cref="SerializerBuilder"/> and <see cref="DeserializerBuilder"/>.
    /// </summary>
    public class YamlSerializerOptions : EncodingOptions, IValidatableParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlSerializerOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="YamlSerializerOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Converters"/></term>
        ///         <description><see cref="List{T}"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ReflectionRules"/></term>
        ///         <description><c>new MemberReflection(true, true);</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="WhiteSpaceIndentation"/></term>
        ///         <description><c>2</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="UseAliases"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="NamingConvention"/></term>
        ///         <description><c>NullNamingConvention.Instance</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="EnumNamingConvention"/></term>
        ///         <description><c>NullNamingConvention.Instance</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="MaximumRecursion"/></term>
        ///         <description>25</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="NewLine"/></term>
        ///         <description><c>Environment.NewLine</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Formatter"/></term>
        ///         <description><c>YamlFormatter.Default</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FormatProvider"/></term>
        ///         <description><c>CultureInfo.InvariantCulture</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IndentSequences"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TextWidth"/></term>
        ///         <description><c>int.MaxValue</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="ValuesHandling"/></term>
        ///         <description>DefaultValuesHandling.Preserve</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="EnsureRoundtrip"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public YamlSerializerOptions()
        {
            ReflectionRules = new MemberReflection(true, true);
            WhiteSpaceIndentation = 2;
            UseAliases = false;
            NamingConvention = NullNamingConvention.Instance;
            EnumNamingConvention = NullNamingConvention.Instance;
            MaximumRecursion = 25;
            NewLine = Environment.NewLine;
            Formatter = YamlFormatter.Default;
            FormatProvider = CultureInfo.InvariantCulture;
            IndentSequences = true;
            TextWidth = int.MaxValue;
            ValuesHandling = DefaultValuesHandling.Preserve;
            EnsureRoundtrip = false;
        }

        /// <summary>
        /// Gets a <see cref="YamlConverter" /> collection that will be used during serialization.
        /// </summary>
        /// <value>The converters that will be used during serialization.</value>
        public IList<YamlConverter> Converters { get; set; } = new List<YamlConverter>();

        /// <summary>
        /// Gets or sets the white space indentation.
        /// </summary>
        /// <value>The white space indentation.</value>
        public int WhiteSpaceIndentation { get; set; }

        /// <summary>
        /// Gets or sets the binding constraints for reflection based member searching.
        /// </summary>
        /// <value>The binding constraints for reflection based member searching.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public MemberReflection ReflectionRules { get; set; }

        /// <summary>
        /// Gets or sets how properties with default and null values should be handled. The default value is <see cref="DefaultValuesHandling.Preserve"/>.
        /// </summary>
        /// <value>How properties with default and null values should be handled. The default value is <see cref="DefaultValuesHandling.Preserve"/>.</value>
        public DefaultValuesHandling ValuesHandling { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow use of aliases in the generated YAML. The default value is <c>false</c>.
        /// </summary>
        /// <value><c>true</c> to allow use of aliases in the generated YAML; otherwise, <c>false</c>.</value>
        public bool UseAliases { get; set; }

        /// <summary>
        /// Gets or sets a value that will force the emission of tags and emit only properties with setters. The default value is <c>false</c>.
        /// </summary>
        /// <value><c>true</c> to force the emission of tags and emit only properties with setters; otherwise, <c>false</c>.</value>
        public bool EnsureRoundtrip { get; set; }

        /// <summary>
        /// Gets or sets the default quoting style for scalar values. The default value is <see cref="ScalarStyle.Any" />.
        /// </summary>
        /// <value>The default quoting style for scalar values.</value>
        public ScalarStyle ScalarStyle { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="INamingConvention"/> used to convert a property's name on an object to another format. The default value is <see cref="NullNamingConvention.Instance"/> (unaltered).
        /// </summary>
        /// <value>The <see cref="INamingConvention"/> used to convert a property's name on an object to another format.</value>
        public INamingConvention NamingConvention { get; set; }

        /// <summary>
        /// Gets or sets the maximum recursion that is allowed while traversing the object graph. The default value is 50.
        /// </summary>
        /// <value>The maximum recursion that is allowed while traversing the object graph.</value>
        public int MaximumRecursion { get; set; }

        /// <summary>
        /// Gets or sets the new line character to use when serializing to YAML. Default is <see cref="Environment.NewLine"/>.
        /// </summary>
        /// <value>The new line character to use when serializing to YAML.</value>
        public string NewLine { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="INamingConvention"/> used when handling enum values. The default value is <see cref="NullNamingConvention.Instance"/> (unaltered).
        /// </summary>
        /// <value>The <see cref="INamingConvention"/> used when handling enum values.</value>
        public INamingConvention EnumNamingConvention { get; set; }

        /// <summary>
        /// Gets or sets formatting options for the generated YAML. Defaults to <see cref="YamlFormatter.Default"/>.
        /// </summary>
        /// <value>The formatting options for the generated YAML.</value>
        public YamlFormatter Formatter { get; set; }

        /// <summary>
        /// Gets or sets the culture-specific formatting information used when writing YAML. The default is <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <value>An <see cref="IFormatProvider"/> that contains the culture-specific formatting information.</value>
        public CultureInfo FormatProvider { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to indent sequences/arrays in the generated YAML. The default value is <c>true</c>.
        /// </summary>
        /// <value><c>true</c> to indent sequences/arrays in the generated YAML; otherwise, <c>false</c>.</value>
        public bool IndentSequences { get; set; }

        /// <summary>
        /// Gets or sets the preferred width of text in the generated YAML.
        /// </summary>
        /// <value>The preferred width of text in the generated YAML.</value>
        public int TextWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the output is in a canonical form.
        /// </summary>
        /// <value><c>true</c> if the output is in a canonical form; otherwise, <c>false</c>.</value>
        public bool IsCanonical { get; set; }

        /// <summary>
        /// Determines whether the public read-write properties of this instance are in a valid state.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// <see cref="Converters"/> cannot be null - or -,
        /// <see cref="ReflectionRules"/> cannot be null.
        /// </exception>
        /// <remarks>This method is expected to throw exceptions when one or more conditions fails to be in a valid state.</remarks>
        public void ValidateOptions()
        {
            Validator.ThrowIfInvalidState(Converters == null);
            Validator.ThrowIfInvalidState(ReflectionRules == null);
        }
    }
}
