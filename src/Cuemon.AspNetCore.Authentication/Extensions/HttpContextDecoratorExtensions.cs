using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Authentication
{
    internal static class HttpContextDecoratorExtensions
    {
        internal static async Task InvokeAuthenticationAsync<TOptions>(this IDecorator<HttpContext> decorator, TOptions options, Action<HttpResponseMessage, HttpResponse> transformer) where TOptions : AuthenticationOptions
        {
            var message = options.ResponseHandler?.Invoke();
            if (message != null)
            {
                transformer?.Invoke(message, decorator.Inner.Response);
                throw new UnauthorizedException(await message.Content.ReadAsStringAsync().ConfigureAwait(false));
            }
        }
    }
}