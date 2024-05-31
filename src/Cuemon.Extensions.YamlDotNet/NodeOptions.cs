using Cuemon.Configuration;
using YamlDotNet.Core;

namespace Cuemon.Extensions.YamlDotNet
{
    /// <summary>
    /// Configuration options for <see cref="EmitterExtensions"/>.
    /// </summary>
    public class NodeOptions : IParameterObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NodeOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="NodeOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Anchor"/></term>
        ///         <description><see cref="AnchorName.Empty"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Tag"/></term>
        ///         <description><see cref="TagName.Empty"/></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public NodeOptions()
        {
            Anchor = AnchorName.Empty;
            Tag = TagName.Empty;
        }

        /// <summary>
        /// Gets or sets the anchor.
        /// </summary>
        /// <value>The anchor.</value>
        public AnchorName Anchor { get; set; }

        /// <summary>
        /// Gets or sets the tag.
        /// </summary>
        /// <value>The tag.</value>
        public TagName Tag { get; set; }
    }
}
