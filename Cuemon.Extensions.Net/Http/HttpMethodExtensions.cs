using System.Net.Http;
using Cuemon.Net.Http;

namespace Cuemon.Extensions.Net.Http
{
    /// <summary>
    /// This is an extension implementation of the <see cref="HttpMethod"/> class.
    /// </summary>
    public static class HttpMethodExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="method"/> to its equivalent <see cref="HttpMethods"/> representation.
        /// </summary>
        /// <param name="method">The <see cref="HttpMethod"/> to be converted.</param>
        /// <returns>A <see cref="HttpMethods"/> representation of the specified <paramref name="method"/>.</returns>
        public static HttpMethods ToHttpMethod(this HttpMethod method)
        {
            return HttpMethodConverter.ToHttpMethod(method);
        }
    }
}