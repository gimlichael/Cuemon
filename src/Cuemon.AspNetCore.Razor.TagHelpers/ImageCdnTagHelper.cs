using System.Threading.Tasks;
using Cuemon.AspNetCore.Mvc.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Razor.TagHelpers
{
    /// <summary>
    /// <see cref="CdnTagHelper"/> implementation targeting &lt;img&gt; elements that supports <see cref="ICacheBusting"/> versioning. This class cannot be inherited.
    /// Implements the <see cref="CdnTagHelper" />
    /// </summary>
    /// <seealso cref="CdnTagHelper" />
    [HtmlTargetElement("cdn-img")]
    public sealed class ImageCdnTagHelper : CdnTagHelper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCdnTagHelper"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="CdnTagHelperOptions" /> which need to be configured.</param>
        /// <param name="cacheBusting">An optional object implementing the <see cref="ICacheBusting" /> interface.</param>
        public ImageCdnTagHelper(IOptions<CdnTagHelperOptions> setup, ICacheBusting cacheBusting = null) : base(setup, cacheBusting)
        {
        }

        /// <summary>
        /// Gets or sets the identifier of the image. 
        /// </summary>
        /// <value>The identifier of the image.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the class name of the image.
        /// </summary>
        /// <value>The class name of the image.</value>
        public string Class { get; set; }

        /// <summary>
        /// Gets or sets the source of the image.
        /// </summary>
        /// <value>The source of the image.</value>
        public string Src { get; set; }

        /// <summary>
        /// Gets or sets the alternative text of the image.
        /// </summary>
        /// <value>The alternative text of the image.</value>
        public string Alt { get; set; }

        /// <summary>
        /// Gets or sets the title of the image.
        /// </summary>
        /// <value>The title of the image.</value>
        public string Title { get; set; }

        /// <summary>
        /// Asynchronously executes the <see cref="T:TagHelper" /> with the given <paramref name="context" /> and <paramref name="output" />.
        /// </summary>
        /// <param name="context">Contains information associated with the current HTML tag.</param>
        /// <param name="output">A stateful HTML element used to generate an HTML tag.</param>
        /// <returns>A <see cref="T:Task" /> that on completion updates the <paramref name="output" />.</returns>
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagOnly;
            output.TagName = "img";
            if (!Id.IsNullOrWhiteSpace()) { output.Attributes.Add("id", Id); }
            if (!Class.IsNullOrWhiteSpace()) { output.Attributes.Add("class", Class); }
            output.Attributes.Add("src", string.Concat(GetBaseUrl(), UseCacheBusting ? $"{Src}?v={CacheBusting.Version}" : Src));
            if (!Alt.IsNullOrWhiteSpace()) { output.Attributes.Add("alt", Alt); }
            if (!Title.IsNullOrWhiteSpace()) { output.Attributes.Add("title", Title); }
            return Task.CompletedTask;
        }
    }
}