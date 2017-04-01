using System;
using System.Collections.Generic;
using System.IO;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Mvc
{
    internal static class Infrastructure
    {
        internal static async Task InvokeResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next, Action<Stream, HttpRequest, HttpResponse> entityTagParser)
        {
            using (var result = new MemoryStream())
            {
                var body = context.HttpContext.Response.Body;
                context.HttpContext.Response.Body = result;
                await next().ConfigureAwait(false);
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
                await result.CopyToAsync(body).ConfigureAwait(false);
            }
        }

        internal static void InterceptControllerWithProfilerOnActionExecuting(ActionExecutingContext context, TimeMeasureOptions options, TimeMeasureProfiler profiler)
        {
            profiler.Timer.Start();
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var expectedObjects = context.ParseRuntimeParameters(descriptor);
                var verifiedObjects = context.ActionArguments.Values.ToArray();
                if (verifiedObjects.Length == expectedObjects.Length) { expectedObjects = verifiedObjects; }
                var md = options.MethodDescriptor?.Invoke() ?? context.ParseMethodDescriptor(descriptor);
                profiler.Member = md.ToString();
                profiler.Data = md.MergeParameters(options.RuntimeParameters ?? expectedObjects);
            }
        }

        internal static void InterceptControllerWithProfilerOnActionExecuted(ActionExecutedContext context, TimeMeasureOptions options, TimeMeasureProfiler profiler)
        {
            profiler.Timer.Stop();
            if (options.TimeMeasureCompletedThreshold == TimeSpan.Zero || profiler.Elapsed > options.TimeMeasureCompletedThreshold)
            {
                TimeMeasure.CompletedCallback?.Invoke(profiler);
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