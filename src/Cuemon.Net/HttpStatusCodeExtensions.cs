using System.Net;

namespace Cuemon.Net
{
    /// <summary>
    /// Extension methods for the <see cref="HttpStatusCode"/> enum.
    /// </summary>
    public static class HttpStatusCodeExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="statusCode"/> is within the successful range.
        /// </summary>
        /// <param name="statusCode">The <see cref="HttpStatusCode"/> to evaluate.</param>
        /// <returns><c>true</c> if <see cref="HttpStatusCode"/> was in the <b>Successful</b> range (200-299); otherwise, <c>false</c>.</returns>
        public static bool IsSuccessStatusCode(this HttpStatusCode statusCode)
        {
            var statusCodeInt = (int)statusCode;
            return (statusCodeInt >= 200 && statusCodeInt <= 299);
        }
    }
}