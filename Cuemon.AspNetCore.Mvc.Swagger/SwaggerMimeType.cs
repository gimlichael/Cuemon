using System;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Represents a Multipurpose Internet Mail Extensions (MIME) type.
    /// </summary>
    public class SwaggerMimeType : IEquatable<SwaggerMimeType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerExtension"/> class.
        /// </summary>
        /// <param name="name">The media type (MIME) name which is a two-part identifier for file formats.</param>
        public SwaggerMimeType(string name)
        {
            Validator.ThrowIfNullOrWhitespace(name, nameof(name));
            Validator.ThrowIfNot(name, s => s.Count('/') == 1, ExceptionUtility.CreateArgumentException, nameof(name), "The MIME type MUST consist of a type/subtype.");
            Name = name;
        }

        /// <summary>
        /// Gets the media type (MIME) name of the example.
        /// </summary>
        /// <value>The name of the extension.</value>
        public string Name { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(SwaggerMimeType other)
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
            var swaggerMimeType = obj as SwaggerMimeType;
            if (swaggerMimeType == null) { return false; }
            return Equals(swaggerMimeType);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            return Name.GetHashCode32();
        }
    }
}