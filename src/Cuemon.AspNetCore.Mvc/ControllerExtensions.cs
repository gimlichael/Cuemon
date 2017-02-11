using System;
using Cuemon.Integrity;
using Microsoft.AspNetCore.Mvc;
using Cuemon.AspNetCore.Integrity;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Mvc
{
    /// <summary>
    /// Extension methods for the <see cref="Controller"/> class.
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Creates an <see cref="IActionResult" /> that produces an OK (200) response from <see cref="OkObjectResult" /> or a NotModified (304) response from <see cref="EmptyResult" />.
        /// </summary>
        /// <typeparam name="T">The type of the value for the <see cref="IActionResult"/>.</typeparam>
        /// <param name="controller">The controller to extend.</param>
        /// <param name="value">The content to format into the entity body.</param>
        /// <param name="parser">The function delegate that will retrieve a <see cref="CacheValidator" /> and thereby determines the <see cref="IActionResult" /> of this method.</param>
        /// <returns>An <see cref="IActionResult" /> object that is either created from <see cref="EmptyResult" /> or <see cref="OkObjectResult" />.</returns>
        public static IActionResult OkOrNotModified<T>(this Controller controller, T value, Func<T, CacheValidator> parser = null)
        {
            var validator = parser?.Invoke(value) ?? CacheValidator.ReferencePoint.CombineWith(value.GetHashCode());
            validator.SetEntityTagHeaderInformation(controller.Request, controller.Response);
            return controller.Response.StatusCode == StatusCodes.Status304NotModified ? (IActionResult)new EmptyResult() : new OkObjectResult(value);
        }
    }
}