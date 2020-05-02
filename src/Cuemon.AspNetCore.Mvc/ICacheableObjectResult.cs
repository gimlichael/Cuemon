using Cuemon.Integrity;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// An interface for providing hints to an implementor that an object is cacheable.
    /// </summary>
    /// <seealso cref="ICacheableTimestamp"/>.
    /// <seealso cref="ICacheableIntegrity"/>.
    /// <seealso cref="ICacheableEntity"/>.
    public interface ICacheableObjectResult 
    {
        /// <summary>
        /// Gets or sets the value of the cacheable object.
        /// </summary>
        /// <value>The value of the cacheable object.</value>
        object Value { get; set; }
    }
}