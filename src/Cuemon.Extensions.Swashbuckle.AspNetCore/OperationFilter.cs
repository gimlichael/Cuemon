using Cuemon.Configuration;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Cuemon.Extensions.Swashbuckle.AspNetCore
{
    /// <summary>
    /// Represents the base class of an <see cref="IOperationFilter"/> implementation.
    /// </summary>
    /// <seealso cref="IDocumentFilter" />
    /// <remarks>https://github.com/domaindrivendev/Swashbuckle.AspNetCore#operation-filters</remarks>
    public abstract class OperationFilter : IOperationFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationFilter"/> class.
        /// </summary>
        protected OperationFilter()
        {
        }

        /// <summary>
        /// Applies post-processing to the <paramref name="operation"/>.
        /// </summary>
        /// <param name="operation">The <see cref="OpenApiOperation"/> to modify.</param>
        /// <param name="context">The <see cref="OperationFilterContext"/> that provides additional context.</param>
        /// <remarks>Swashbuckle retrieves an <see cref="ApiDescription"/> for every action and uses it to generate a corresponding <see cref="OpenApiOperation"/>.</remarks>
        public abstract void Apply(OpenApiOperation operation, OperationFilterContext context);
    }

    /// <summary>
    /// Represents a configurable base class of an <see cref="IOperationFilter"/> implementation.
    /// </summary>
    /// <seealso cref="OperationFilter"/>
    public abstract class OperationFilter<T> : OperationFilter, IConfigurable<T> where T : class, IParameterObject, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFilter{T}"/> class.
        /// </summary>
        /// <param name="options">The configured options of this instance.</param>
        protected OperationFilter(T options)
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
