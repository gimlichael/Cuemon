using System.Collections.Generic;
using System.IO;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using Cuemon.AspNetCore.Integrity;
using Cuemon.Integrity;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Mvc
{
    internal static class Infrastructure
    {
        internal static async Task InvokeResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {

            using (var memoryStream = new MemoryStream())
            {
                var stream = context.HttpContext.Response.Body;
                context.HttpContext.Response.Body = memoryStream;
                await next();

                var httpMethod = context.HttpContext.Request.Method;
                if (HttpMethods.IsGet(httpMethod) || HttpMethods.IsHead(httpMethod))
                {
                    if (context.HttpContext.Response.IsSuccessStatusCode())
                    {
                        var validator = CacheValidator.ReferencePoint.CombineWith(memoryStream.ComputeHash(HashAlgorithmType.MD5, true).Value);
                        validator.SetEntityTagHeaderInformation(context.HttpContext.Request, context.HttpContext.Response);
                        if (context.HttpContext.Response.StatusCode == StatusCodes.Status304NotModified)
                        {
                            return;
                        }
                    }
                }
                memoryStream.Seek(0, SeekOrigin.Begin);
                await memoryStream.CopyToAsync(stream);
            }
        }

        internal static void InterceptControllerWithProfiler(ActionExecutingContext context, TimeMeasureOptions options)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var expectedObjects = context.ParseRuntimeParameters(descriptor);
                var verifiedObjects = context.ActionArguments.Values.ToArray();
                if (verifiedObjects.Length == expectedObjects.Length) { expectedObjects = verifiedObjects; }
                context.Result = TimeMeasure.WithFunc(descriptor.MethodInfo.Invoke, context.Controller, expectedObjects, o =>
                {
                    o.RuntimeParameters = options.RuntimeParameters ?? expectedObjects;
                    o.MethodDescriptor = options.MethodDescriptor ?? (() => context.ParseMethodDescriptor(descriptor));
                    o.TimeMeasureCompletedThreshold = options.TimeMeasureCompletedThreshold;
                }).Result as IActionResult;
            }
        }

        internal static object[] ParseRuntimeParameters(this FilterContext context, ControllerActionDescriptor descriptor)
        {
            if (descriptor == null) { return null; }
            var objects = new List<object>();
            foreach (var pi in descriptor.Parameters)
            {
                objects.Add(ObjectConverter.ChangeType(context.RouteData.Values[pi.Name], pi.ParameterType));
            }
            return objects.ToArray();
        }

        internal static MethodDescriptor ParseMethodDescriptor(this FilterContext context, ControllerActionDescriptor descriptor)
        {
            if (descriptor == null) { return null; }
            return new MethodDescriptor(descriptor.MethodInfo);
        }
    }
}