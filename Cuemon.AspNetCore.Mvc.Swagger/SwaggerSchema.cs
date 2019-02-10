using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Allows the definition of input and output data types. These types can be objects, but also primitives and arrays. This object is based on the <a href="http://json-schema.org/">JSON Schema Specification Draft 4</a> and uses a predefined subset of it. On top of this subset, there are extensions provided by this specification to allow for more complete documentation.
    /// </summary>
    /// <seealso cref="SwaggerRules" />
    public class SwaggerSchema : SwaggerRules
    {
        /// <summary>
        /// Gets or sets the value to reference another value in a JSON document.
        /// </summary>
        /// <value>The value to reference another value in a JSON document.</value>
        public string Ref { get; set; }

        /// <summary>
        /// Gets or sets the title that will preferably be short.
        /// </summary>
        /// <value>The title that will preferably be short.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description that will provide explanation about the purpose of the instance described by this schema.
        /// </summary>
        /// <value>The description that will provide explanation about the purpose of the instance described by this schema.</value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the maximum properties of an object instance that MUST be greater than, or equal to, 0.
        /// </summary>
        /// <value>The maximum properties of an object instance.</value>
        /// <remarks>An object instance is valid against "maxProperties" if its number of properties is less than, or equal to, the value of this keyword.</remarks>
        public int MaxProperties { get; set; }

        /// <summary>
        /// Gets or sets the minimum properties of an object instance that MUST be greater than, or equal to, 0.
        /// </summary>
        /// <value>The minimum properties of an object instance.</value>
        /// <remarks>An object instance is valid against "minProperties" if its number of properties is greater than, or equal to, the value of this keyword.</remarks>
        public int MinProperties { get; set; }

        /// <summary>
        /// Gets the required values of an instance property set.
        /// </summary>
        /// <value>The required values of an instance property set.</value>
        /// <remarks>An object instance is valid against this keyword if its property set contains all elements in this keyword's array value.</remarks>
        public IList<string> Required { get; } = new List<string>();

        /// <summary>
        /// Gets or sets the optional format modifier of this schema for primitives.
        /// </summary>
        /// <value>The optional format modifier of this schema for primitives.</value>
        /// <remarks>Primitives have an optional modifier property format. Swagger uses several known formats to more finely define the data type being used. However, the format property is an open string-valued property, and can have any value to support documentation needs. Formats such as "email", "uuid", etc., can be used even though they are not defined by this specification. Types that are not accompanied by a format property follow their definition from the JSON Schema (except for file type).</remarks>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the type of an instance.
        /// </summary>
        /// <value>The type of an instance.</value>
        /// <remarks>An instance matches successfully if its primitive type is one of the types defined by keyword. Recall: "number" includes "integer".</remarks>
        public string Type { get; set; }

        /// <summary>
        /// Gets the items schema associated with this schema.
        /// </summary>
        /// <value>The items schema associated with this schema.</value>
        public SwaggerSchema Items { get; set; }

        /// <summary>
        /// Gets all of the schemas that an instance must validate against.
        /// </summary>
        /// <value>All of the schemas that an instance must validate against.</value>
        /// <remarks>An instance validates successfully against this keyword if it validates successfully against all schemas defined by this keyword's value.</remarks>
        public IList<SwaggerSchema> AllOf { get; } = new List<SwaggerSchema>();

        /// <summary>
        /// Gets the properties of this schema.
        /// </summary>
        /// <value>The properties of this schema.</value>
        public IDictionary<string, SwaggerSchema> Properties { get; } = new Dictionary<string, SwaggerSchema>();

        /// <summary>
        /// Gets or sets the additional properties of this schema.
        /// </summary>
        /// <value>The additional properties of this schema.</value>
        /// <remarks>If "additionalProperties" is an object, validate the value as a schema to all of the properties that weren't validated by "properties" nor "patternProperties".</remarks>
        public SwaggerSchema AdditionalProperties { get; set; }

        /// <summary>
        /// Gets or sets the discriminator that adds support for polymorphism. The discriminator is the schema property name that is used to differentiate between other schema that inherit this schema. The property name used MUST be defined at this schema and it MUST be in the required property list. When used, the value MUST be the name of this schema or any schema that inherits it.
        /// </summary>
        /// <value>The discriminator that adds support for polymorphism.</value>
        public string Discriminator { get; set; }

        /// <summary>
        /// Gets or sets a value that is relevant only for <see cref="Properties"/>. Declares the property as "read only". This means that it MAY be sent as part of a response but MUST NOT be sent as part of the request. Properties marked as readOnly being true SHOULD NOT be in the required list of the defined schema. Default value is false.
        /// </summary>
        /// <value><c>true</c> if <see cref="Properties"/> should be treated as read-only; otherwise, <c>false</c>.</value>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Gets or sets the XML that MAY be used only on properties schemas. It has no effect on root schemas. Adds Additional metadata to describe the XML representation format of this property.
        /// </summary>
        /// <value>The XML that MAY be used only on properties schemas.</value>
        public SwaggerXml Xml { get; set; }

        /// <summary>
        /// Gets or sets the additional external documentation for this schema.
        /// </summary>
        /// <value>The dditional external documentation for this schema.</value>
        public SwaggerExternalDocumentation ExternalDocs { get; set; }

        /// <summary>
        /// Gets or sets a free-form property to include an example of an instance for this schema.
        /// </summary>
        /// <value>The free-form property to include an example of an instance for this schema.</value>
        public object Example { get; set; }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}