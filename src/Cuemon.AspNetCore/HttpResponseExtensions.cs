using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore
{
    /// <summary>
    /// Extension methods for the <see cref="HttpResponse"/> class.
    /// </summary>
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Determines whether the HTTP response was successful.
        /// </summary>
        /// <param name="response">An instance of the <see cref="HttpResponse"/> object.</param>
        /// <returns><c>true</c> if <see cref="HttpResponse.StatusCode"/> was in the <b>Successful</b> range (200-299); otherwise, <c>false</c>.</returns>
        public static bool IsSuccessStatusCode(this HttpResponse response)
        {
            return (response.StatusCode >= 200 && response.StatusCode <= 299);
        }
    }
}