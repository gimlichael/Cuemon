using System;
using System.Collections.Generic;
using Cuemon.Configuration;
using Cuemon.Runtime.Serialization.Converters;
using Cuemon.Reflection;
using Cuemon.Text;
using Cuemon.Text.Yaml;
using Cuemon.Text.Yaml.Converters;

namespace Cuemon.Runtime.Serialization
{
    /// <summary>
    /// Configuration options for <see cref="YamlSerializer"/>.
    /// </summary>
    [Obsolete("All YAML marshalling has been moved to its own assembly; Cuemon.Extensions.YamlDotNet. This member will be removed with next major version.")]
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
        ///         <description><see cref="List{YamlConverter}"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="WhiteSpaceIndentation"/></term>
        ///         <description><c>2</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="NamingPolicy"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public YamlSerializerOptions()
        {
            ReflectionRules = new MemberReflection(true, true);
            WhiteSpaceIndentation = 2;
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
        /// Gets or sets a value that specifies the policy used to convert a property's name on an object to another format, or null to leave property names unchanged.
        /// </summary>
        /// <value>A property naming policy, or null to leave property names unchanged.</value>
        public YamlNamingPolicy NamingPolicy { get; set; }

        /// <summary>
        /// Gets or sets the binding constraints for reflection based member searching.
        /// </summary>
        /// <value>The binding constraints for reflection based member searching.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public MemberReflection ReflectionRules { get; set; }

        /// <summary>
        /// Returns the specified <paramref name="name"/> adhering to the underlying <see cref="NamingPolicy"/>.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <returns>When <see cref="NamingPolicy"/> is null, the specified <paramref name="name"/> is returned unaltered; otherwise it is converted according to the <see cref="NamingPolicy"/>.</returns>
        public string SetPropertyName(string name)
        {
            return Decorator.Enclose(NamingPolicy, false).DefaultOrConvertName(name);
        }

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
