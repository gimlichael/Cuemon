using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// The header that can be sent as part of a response.
    /// </summary>
    public class SwaggerHeader : SwaggerRules
    {
        /// <summary>
        /// Gets or sets a short description of the header.
        /// </summary>
        /// <value>The short description of the header.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the object. The value MUST be one of "string", "number", "integer", "boolean", or "array".
        /// </summary>
        /// <value>The type of the object.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the extending format for the previously mentioned <see cref="Type"/>. See <a href="http://swagger.io/specification/#dataTypeFormat">Data Type Formats</a> for further details.
        /// </summary>
        /// <value>The extending format for the previously mentioned type.</value>
        public string Format { get; set; }

        /// <summary>
        /// Gets the type description of items in the array.
        /// </summary>
        /// <value>The type description of items in the array.</value>
        public IList<SwaggerItem> Items { get; } = new List<SwaggerItem>();

        /// <summary>
        /// Gets or sets the format of the array if type array is used.
        /// </summary>
        /// <value>The format of the array if type array is used.</value>
        public SwaggerCollectionFormat CollectionFormat { get; } = SwaggerCollectionFormat.Csv;

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}