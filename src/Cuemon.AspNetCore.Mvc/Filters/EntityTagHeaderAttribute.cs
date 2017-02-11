using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Represents an attribute that is used to mark an action method that computes the response body and applies an appropriate HTTP Etag header.
    /// </summary>
    /// <seealso cref="ResultFilterAttribute" />
    public class EntityTagHeaderAttribute : ResultFilterAttribute
    {
        /// <summary>
        /// Called asynchronously before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ResultExecutingContext" />.</param>
        /// <param name="next">The <see cref="ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            return Infrastructure.InvokeResultExecutionAsync(context, next);
        }
    }
}