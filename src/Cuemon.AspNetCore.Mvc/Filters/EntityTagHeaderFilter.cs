using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// A filter that computes the response body and applies an appropriate HTTP Etag header.
    /// </summary>
    /// <seealso cref="IAsyncResultFilter" />
    public class EntityTagHeaderFilter : IAsyncResultFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTagHeaderFilter"/> class.
        /// </summary>
        /// <param name="setup">The <see cref="EntityTagHeaderOptions"/> which need to be configured.</param>
        public EntityTagHeaderFilter(Action<EntityTagHeaderOptions> setup = null)
        {
            Options = setup.ConfigureOptions();
        }

        private EntityTagHeaderOptions Options { get; set; }

        /// <summary>
        /// Called asynchronously before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ResultExecutingContext" />.</param>
        /// <param name="next">The <see cref="ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        public Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return Infrastructure.InvokeResultExecutionAsync(context, next, Options.EntityTagParser);
        }
    }
}