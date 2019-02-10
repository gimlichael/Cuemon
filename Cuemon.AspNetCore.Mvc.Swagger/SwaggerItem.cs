using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// A limited subset of JSON-Schema's items object. It is used by parameter definitions that are not located in "body".
    /// </summary>
    /// <seealso cref="SwaggerRules" />
    public class SwaggerItem : SwaggerRules
    {
        /// <summary>
        /// Gets or sets the internal type of the array. The value MUST be one of "string", "number", "integer", "boolean", or "array". Files and models are not allowed.
        /// </summary>
        /// <value>The internal type of the array.</value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the extending format for the previously mentioned <see cref="Type"/>. See <a href="http://swagger.io/specification/#dataTypeFormat">Data Type Formats</a> for further details.
        /// </summary>
        /// <value>The extending format for the previously mentioned <see cref="Type"/>.</value>
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
        public SwaggerCollectionFormat CollectionFormat { get; set; } = SwaggerCollectionFormat.Csv;

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}