using System;
using System.Collections.Generic;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Holds the relative paths to the individual endpoints. The path is appended to the basePath in order to construct the full URL. The Paths may be empty, due to ACL constraints.
    /// </summary>
    public class SwaggerPath : IEquatable<SwaggerPath>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerPath"/> class.
        /// </summary>
        /// <param name="path">The relative path to an individual endpoint.</param>
        public SwaggerPath(string path)
        {
            Validator.ThrowIfNullOrWhitespace(path, nameof(path));
            if (!path.StartsWith("/")) { path = "/" + path; }
            Path = path;
        }

        /// <summary>
        /// Gets the relative path to an individual endpoint.
        /// </summary>
        /// <value>The relative path to an individual endpoint.</value>
        public string Path { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(SwaggerPath other)
        {
            if (other == null) { return false; }
            return GetHashCode() == other.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var swaggerPath = obj as SwaggerPath;
            if (swaggerPath == null) { return false; }
            return Equals(swaggerPath);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Path.GetHashCode32();
        }

        /// <summary>
        /// Gets the extensions to the Swagger Schema.
        /// </summary>
        /// <value>The extensions to the Swagger Schema.</value>
        public IDictionary<SwaggerExtension, object> Extensions { get; } = new Dictionary<SwaggerExtension, object>();
    }
}