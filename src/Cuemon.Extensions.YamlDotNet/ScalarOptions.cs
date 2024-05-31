using YamlDotNet.Core;

namespace Cuemon.Extensions.YamlDotNet
{
    /// <summary>
    /// Configuration options for <see cref="EmitterExtensions"/>.
    /// </summary>
    /// <seealso cref="NodeOptions" />
    public class ScalarOptions : NodeOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScalarOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ScalarOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Style"/></term>
        ///         <description><see cref="ScalarStyle.Any"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IsPlainImplicit"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IsQuotedImplicit"/></term>
        ///         <description><c>true</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="IsKey"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ScalarOptions()
        {
            Style = ScalarStyle.Any;
            IsPlainImplicit = true;
            IsQuotedImplicit = true;
            IsKey = false;
        }

        /// <summary>
        /// Gets or sets the style of the scalar.
        /// </summary>
        /// <value>The style of the scalar.</value>
        public ScalarStyle Style { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tag is optional for the plain style.
        /// </summary>
        /// <value><c>true</c> if tag is optional for the plain style; otherwise, <c>false</c>.</value>
        public bool IsPlainImplicit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the tag is optional for any non-plain style.
        /// </summary>
        /// <value><c>true</c> if tag is optional for any non-plain style; otherwise, <c>false</c>.</value>
        public bool IsQuotedImplicit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this scalar event is a key
        /// </summary>
        /// <value><c>true</c> if this scalar event is a key; otherwise, <c>false</c>.</value>
        public bool IsKey { get; set; }
    }
}
