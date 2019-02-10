namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Defines an API key (either as a header or as a query parameter).
    /// </summary>
    /// <seealso cref="SwaggerSecurityScheme" />
    public class SwaggerApiKeySecurityScheme : SwaggerSecurityScheme
    {
        /// <summary>
        /// Gets the type of the security scheme.
        /// </summary>
        /// <value>The type of the security scheme.</value>
        public override SwaggerSecurityType Type { get; } = SwaggerSecurityType.ApiKey;

        /// <summary>
        /// Gets or sets the name of the header or query parameter to be used.
        /// </summary>
        /// <value>The name of the header or query parameter to be used.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of the API key.
        /// </summary>
        /// <value>The location of the API key.</value>
        public SwaggerSecurityIn In { get; set; }
    }
}