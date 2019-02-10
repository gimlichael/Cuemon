using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Http
{
    /// <summary>
    /// Extension methods for the <see cref="HttpResponseMessage"/> class.
    /// </summary>
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Transfers the specified <paramref name="message"/> to the HTTP <paramref name="response"/> pipeline using the <paramref name="transformer"/> delegate.
        /// </summary>
        /// <param name="message">The <see cref="HttpResponseMessage"/> to extend.</param>
        /// <param name="response">The <see cref="HttpResponse"/> to transfer the <paramref name="message"/> to.</param>
        /// <param name="transformer">The delegate that transforms a <see cref="HttpResponseMessage"/> to an HTTP equivalent <see cref="HttpResponse"/>.</param>
        public static void ToHttpResponse(this HttpResponseMessage message, HttpResponse response, Action<HttpResponseMessage, HttpResponse> transformer)
        {
            Validator.ThrowIfNull(message, nameof(message));
            Validator.ThrowIfNull(response, nameof(response));
            response.OnStarting(() =>
            {
                transformer?.Invoke(message, response);
                return Task.CompletedTask;
            });
        }
    }
}