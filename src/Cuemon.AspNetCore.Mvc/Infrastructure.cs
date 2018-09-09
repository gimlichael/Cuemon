using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Http;
using Cuemon.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Mvc
{
    internal static class Infrastructure
    {
        internal static async Task InvokeEntityTagHeaderOnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next, Action<Stream, HttpRequest, HttpResponse> entityTagParser)
        {
            using (var result = new MemoryStream())
            {
                var body = context.HttpContext.Response.Body;
                context.HttpContext.Response.Body = result;
                await next().ContinueWithSuppressedContext();
                result.Seek(0, SeekOrigin.Begin);

                var method = context.HttpContext.Request.Method;
                if (HttpMethods.IsGet(method) || HttpMethods.IsHead(method))
                {
                    if (context.HttpContext.Response.IsSuccessStatusCode())
                    {
                        entityTagParser?.Invoke(result, context.HttpContext.Request, context.HttpContext.Response);
                        if (context.HttpContext.Response.StatusCode == StatusCodes.Status304NotModified)
                        {
                            return;
                        }
                    }
                }
                await result.CopyToAsync(body).ContinueWithSuppressedContext();
            }
        }
    }
}