using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc.Filters.Cacheable
{
    /// <summary>
    /// A filter that asynchronously surrounds execution of action results successfully returned from an action.
    /// </summary>
    public interface ICacheableAsyncResultFilter
    {
        /// <summary>
        /// Called asynchronously before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ResultExecutingContext" />.</param>
        /// <param name="next">The <see cref="ResultExecutionDelegate" />. Invoked to execute the next result filter or the result itself.</param>
        /// <returns>A <see cref="Task" /> that on completion indicates the filter has executed.</returns>
        Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next);
    }
}