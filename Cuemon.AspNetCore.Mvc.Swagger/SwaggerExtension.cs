using System;

namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// While the Swagger Specification tries to accommodate most use cases, additional data can be added to extend the specification at certain points.
    /// </summary>
    public class SwaggerExtension : IEquatable<SwaggerExtension>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerExtension"/> class.
        /// </summary>
        /// <param name="name">The name of the extension which MUST begin with "x-".</param>
        public SwaggerExtension(string name) 
        {
            Validator.ThrowIfNullOrWhitespace(name, nameof(name));
            Validator.ThrowIfNot(name, s => s.StartsWith("x-", StringComparison.OrdinalIgnoreCase), ExceptionUtility.CreateArgumentException, nameof(name), "The name MUST begin with x-, for example, x-internal-id.");
            Name = name;
        }

        /// <summary>
        /// Gets the name of the extension.
        /// </summary>
        /// <value>The name of the extension.</value>
        public string Name { get; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        public bool Equals(SwaggerExtension other)
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
            var swaggerExtension = obj as SwaggerExtension;
            if (swaggerExtension == null) { return false; }
            return Equals(swaggerExtension);
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