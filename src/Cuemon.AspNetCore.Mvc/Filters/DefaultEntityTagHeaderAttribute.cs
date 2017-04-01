namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Represents an attribute that is used to mark an action method that computes the response body and applies an appropriate HTTP Etag header.
    /// </summary>
    /// <seealso cref="EntityTagHeaderAttribute" />
    public sealed class DefaultEntityTagHeaderAttribute : EntityTagHeaderAttribute
    {
        /// <summary>
        /// Gets the default configured options of this instance.
        /// </summary>
        /// <value>The default configured options of this instance.</value>
        protected override EntityTagHeaderOptions Options { get; } = new EntityTagHeaderOptions();
    }
}