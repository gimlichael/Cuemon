using Cuemon.Configuration;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Represents the base class of an <see cref="IDocumentFilter"/> implementation.
    /// </summary>
    /// <seealso cref="IDocumentFilter" />
    /// <remarks>https://github.com/domaindrivendev/Swashbuckle.AspNetCore#document-filters</remarks>
    public abstract class DocumentFilter : IDocumentFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFilter"/> class.
        /// </summary>
        protected DocumentFilter()
        {
        }

        /// <summary>
        /// Applies post-processing to the <paramref name="swaggerDoc"/>.
        /// </summary>
        /// <param name="swaggerDoc">The <see cref="OpenApiDocument"/> to modify.</param>
        /// <param name="context">The <see cref="DocumentFilterContext"/> that provides additional context.</param>
        /// <remarks>Once an <seealso cref="OpenApiDocument"/> has been generated you have full control to modify the document however you see fit.</remarks>
        public abstract void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context);
    }

    /// <summary>
    /// Represents a configurable base class of an <see cref="IDocumentFilter"/> implementation.
    /// </summary>
    /// <seealso cref="DocumentFilter"/>
    public abstract class DocumentFilter<T> : DocumentFilter, IConfigurable<T> where T : class, IParameterObject, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFilter{T}"/> class.
        /// </summary>
        /// <param name="options">The configured options of this instance.</param>
        protected DocumentFilter(T options)
        {
            Validator.ThrowIfNull(options);
            Options = options;
        }

        /// <summary>
        /// Gets the configured options of this instance.
        /// </summary>
        /// <value>The configured options of this instance.</value>
        public T Options { get; }
    }
}
