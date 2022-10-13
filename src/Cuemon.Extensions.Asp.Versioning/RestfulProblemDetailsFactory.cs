using System.Linq;
using Asp.Versioning;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuemon.Extensions.Asp.Versioning
{
    /// <summary>
    /// Represents a RESTful implementation of the <see cref="IProblemDetailsFactory"/> which throws variants of <see cref="HttpStatusCodeException"/> that needs to be translated accordingly.
    /// </summary>
    /// <remarks>This class was introduced because of the design decisions made of the author of Asp.Versioning; for more information have a read at https://github.com/dotnet/aspnet-api-versioning/issues/886</remarks>
    public class RestfulProblemDetailsFactory : IProblemDetailsFactory
    {
        /// <summary>
        /// Creates and returns a new problem details instance.
        /// </summary>
        /// <param name="request">The current <see cref="T:Microsoft.AspNetCore.Http.HttpRequest">HTTP request</see>.</param>
        /// <param name="statusCode">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Status" />.</param>
        /// <param name="title">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Title" />.</param>
        /// <param name="type">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Type" />.</param>
        /// <param name="detail">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Detail" />.</param>
        /// <param name="instance">The value for <see cref="P:Microsoft.AspNetCore.Mvc.ProblemDetails.Instance" />.</param>
        /// <returns>A new <see cref="T:Microsoft.AspNetCore.Mvc.ProblemDetails" /> instance.</returns>
        public ProblemDetails CreateProblemDetails(HttpRequest request, int? statusCode = null, string title = null, string type = null, string detail = null, string instance = null)
        {
            var problemDetails = new ProblemDetails()
            {
                Status = statusCode ?? 500,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance,
            };

            DefaultProblemDetailsFactory.ApplyExtensions(problemDetails);

            var value = problemDetails.Extensions.FirstOrDefault(pair => pair.Key == "Code").Value;
            if (value != null && HttpStatusCodeException.TryParse(problemDetails.Status ?? 500, problemDetails.Detail ?? problemDetails.Title ?? value.ToString(), null, out var statusCodeEquivalentException))
            {
                throw statusCodeEquivalentException;
            }

            throw new InternalServerErrorException(problemDetails.Detail);
        }
    }
}
