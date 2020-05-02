namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    /// <summary>
    /// Configuration options for <see cref="CdnTagHelper"/>.
    /// </summary>
    public class CdnTagHelperOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CdnTagHelperOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="CdnTagHelperOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Scheme"/></term>
        ///         <description><see cref="CdnUriScheme.Relative"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="BaseUrl"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public CdnTagHelperOptions()
        {
            Scheme = CdnUriScheme.Relative;
        }

        /// <summary>
        /// Gets or sets the <see cref="CdnUriScheme"/> of these options.
        /// </summary>
        /// <value>The <see cref="CdnUriScheme"/> of these options.</value>
        public CdnUriScheme Scheme { get; set; }

        /// <summary>
        /// Gets or sets the base URL of these options.
        /// </summary>
        /// <value>The base URL of these options.</value>
        public string BaseUrl { get; set; }
    }
}