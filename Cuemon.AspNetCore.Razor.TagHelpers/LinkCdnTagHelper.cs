using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    /// <summary>
    /// <see cref="CdnTagHelper"/> implementation targeting &lt;link&gt; elements that supports <see cref="ICacheBusting"/> versioning. This class cannot be inherited.
    /// Implements the <see cref="CdnTagHelper" />
    /// </summary>
    /// <seealso cref="CdnTagHelper" />
    [HtmlTargetElement("cdn-link")]
    public sealed class LinkCdnTagHelper : CdnTagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkCdnTagHelper"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="CdnTagHelperOptions" /> which need to be configured.</param>
        /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
        public LinkCdnTagHelper(IOptions<CdnTagHelperOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
        {
            Type = "text/css";
            Rel = "stylesheet";
        }

        /// <summary>
        /// Gets or sets the location of the link.
        /// </summary>
        /// <value>The location of the link.</value>
        public string Href { get; set; }

        /// <summary>
        /// Gets or sets the type of the link.
        /// </summary>
        /// <value>The type of the link.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the relation of the link.
        /// </summary>
        /// <value>The relation of the link.</value>
        public string Rel { get; set; }

        /// <summary>
        /// Asynchronously executes the <see cref="T:TagHelper" /> with the given <paramref name="context" /> and <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <returns>A <see cref="T:Task" /> that on completion updates the <paramref name="output" />.</returns>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagOnly;
            output.TagName = "link";
            output.Attributes.Add("rel", Rel);
            output.Attributes.Add("href", string.Concat(GetBaseUrl(), UseCacheBusting ? $"{Href}?v={CacheBusting.Version}" : Href));
            if (!Type.IsNullOrWhiteSpace()) { output.Attributes.Add("type", Type); }
            return Task.CompletedTask;
        }
    }
}