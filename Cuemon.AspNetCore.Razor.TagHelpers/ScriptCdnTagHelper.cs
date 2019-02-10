using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    /// <summary>
    /// <see cref="CdnTagHelper"/> implementation targeting &lt;script&gt; elements that supports <see cref="ICacheBusting"/> versioning. This class cannot be inherited.
    /// Implements the <see cref="CdnTagHelper" />
    /// </summary>
    /// <seealso cref="CdnTagHelper" />
    [HtmlTargetElement("cdn-script")]
    public sealed class ScriptCdnTagHelper : CdnTagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptCdnTagHelper"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="CdnTagHelperOptions" /> which need to be configured.</param>
        /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
        public ScriptCdnTagHelper(IOptions<CdnTagHelperOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
        {
        }

        /// <summary>
        /// Gets or sets the source of the script.
        /// </summary>
        /// <value>The source of the script.</value>
        public string Src { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the script is executed when the page has finished parsing.
        /// </summary>
        /// <value><c>true</c> if the script is executed when the page has finished parsing; otherwise, <c>false</c>.</value>
        public bool Defer { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.TagName = "script";
            output.Attributes.Add("type", "text/javascript");
            output.Attributes.Add("src", string.Concat(GetBaseUrl(), UseCacheBusting ? $"{Src}?v={CacheBusting.Version}" : Src));
            if (Defer) { output.Attributes.Add("defer", "defer"); }
            return Task.CompletedTask;
        }
    }
}