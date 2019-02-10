using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Extension methods for Swagger related <see cref="IDictionary{TKey,TValue}"/> implementations.
    /// </summary>
    public static class SwaggerDictionaryExtensions
    {
        /// <summary>
        /// Creates a new instance of <see cref="SwaggerExtension"/> and adds this to the specified <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary holding the extensions to the Swagger Schema.</param>
        /// <param name="name">The name of the extension which MUST begin with "x-".</param>
        /// <param name="value">The object associated with <paramref name="name"/>.</param>
        public static void Add(this IDictionary<SwaggerExtension, object> dictionary, string name, object value)
        {
            dictionary.Add(new SwaggerExtension(name), value);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SwaggerMimeType"/> and adds this to the specified <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary holding the examples for operation responses.</param>
        /// <param name="name">The media type (MIME) name which is a two-part identifier for file formats.</param>
        /// <param name="value">The object associated with <paramref name="name"/>.</param>
        public static void Add(this IDictionary<SwaggerMimeType, object> dictionary, string name, object value)
        {
            dictionary.Add(new SwaggerMimeType(name), value);
        }

        /// <summary>
        /// Creates a new instance of <see cref="SwaggerPath"/> and adds this to the specified <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary holding the examples for operation responses.</param>
        /// <param name="path">The relative path to an individual endpoint.</param>
        /// <param name="pathItem">The operations available on a single path.</param>
        public static void Add(this IDictionary<SwaggerPath, SwaggerPathItem> dictionary, string path, SwaggerPathItem pathItem)
        {
            dictionary.Add(new SwaggerPath(path), pathItem);
        }

        /// <summary>
        /// Adds the specified security scheme and requirements to the specified <paramref name="dictionary"/>.
        /// </summary>
        /// <param name="dictionary">The dictionary holding the security requirements.</param>
        /// <param name="scheme">The security scheme of the requirements.</param>
        /// <param name="requirements">The requirements tied to the specified <paramref name="scheme"/>.</param>
        public static void Add(this IDictionary<string, IList<string>> dictionary, string scheme, params string[] requirements)
        {
            dictionary.Add(scheme, new List<string>(requirements));
        }
    }
}