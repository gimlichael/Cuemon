using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// A metadata object that allows for more fine-tuned XML model definitions.
    /// </summary>
    public class SwaggerXml
    {
        /// <summary>
        /// Gets or sets the name of the element/attribute used for the described schema property. When defined within the <see cref="SwaggerItem.Items"/>, it will affect the name of the individual XML elements within the list. When defined alongside type being array (outside the items), it will affect the wrapping element and only if <see cref="Wrapped"/> is true. If <see cref="Wrapped"/> is false, it will be ignored.
        /// </summary>
        /// <value>The name of the element/attribute used for the described schema property.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the URL of the namespace definition. Value SHOULD be in the form of a URL.
        /// </summary>
        /// <value>The URL of the namespace definition.</value>
        public string Namespace { get; set; }

        /// <summary>
        /// Gets or sets the prefix to be used for the <see cref="Name"/>.
        /// </summary>
        /// <value>The prefix to be used for the <see cref="Name"/>.</value>
        public string Prefix { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property definition translates to an attribute instead of an element. Default value is <c>false</c>.
        /// </summary>
        /// <value><c>true</c> if the property definition translates to an attribute instead of an element; otherwise, <c>false</c>.</value>
        public bool Attribute { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the array is wrapped (for example, <books><book/><book/></books>) or unwrapped (<book/><book/>). Default value is <c>false</c>. The definition takes effect only when defined alongside type being array (outside the items).
        /// </summary>
        /// <value><c>true</c> if the array is wrapped (for example, <books><book/><book/></books>); otherwise, <c>false</c>.</value>
        public bool Wrapped { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}