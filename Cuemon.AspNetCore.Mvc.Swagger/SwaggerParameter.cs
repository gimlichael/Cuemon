using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Describes a single operation parameter.
    /// </summary>
    public class SwaggerParameter : SwaggerRules
    {
        /// <summary>
        /// Gets or sets the name of the parameter. Parameter names are case sensitive.
        /// </summary>
        /// <value>The name of the parameter.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a brief description of the parameter. This could contain examples of use. GFM syntax can be used for rich text representation.
        /// </summary>
        /// <value>The brief description of the parameter.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the location of the parameter. Possible values are those found in <see cref="SwaggerIn"/>.
        /// </summary>
        /// <value>The location of the parameter.</value>
        public SwaggerIn In { get; set; }

        /// <summary>
        /// Gets or sets a value that determines whether this parameter is mandatory. If the parameter is in <see cref="SwaggerIn.Path"/>, this property is required and its value MUST be true. Otherwise, the property MAY be included and its default value is false.
        /// </summary>
        /// <value><c>true</c> if this parameter is mandatory; otherwise, <c>false</c>.</value>
        public bool Required { get; set; }

        /// <summary>
        /// Gets or sets the schema defining the type used for the body parameter.
        /// </summary>
        /// <value>The schema defining the type used for the body parameter.</value>
        public SwaggerSchema Schema { get; set; }

        /// <summary>
        /// Gets or sets the type of the parameter. Since the parameter is not located at the request body, it is limited to simple types (that is, not an object). The value MUST be one of "string", "number", "integer", "boolean", "array" or "file". If type is "file", the consumes MUST be either "multipart/form-data", " application/x-www-form-urlencoded" or both and the parameter MUST be in "formData".
        /// </summary>
        /// <value>The type of the parameter.</value>
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
        public SwaggerCollectionFormat CollectionFormat { get; set; } = SwaggerCollectionFormat.Csv;

        /// <summary>
        /// Gets or sets the ability to pass empty-valued parameters. This is valid only for either query or formData parameters and allows you to send a parameter with a name only or an empty value. Default value is <c>false</c>.
        /// </summary>
        /// <value><c>true</c> to allow the ability to pass empty-valued parameters; otherwise, <c>false</c>.</value>
        public bool AllowEmptyValue { get; set; }
    }
}