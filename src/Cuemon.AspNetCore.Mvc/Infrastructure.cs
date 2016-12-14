using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace Cuemon.AspNetCore.Mvc
{
    internal static class Infrastructure
    {
        internal static void InterceptControllerWithProfiler(ActionExecutingContext context, TimeMeasureOptions options)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var objects = context.ActionArguments.Values.ToArray();
                context.Result = TimeMeasure.WithFunc(descriptor.MethodInfo.Invoke, context.Controller, objects, o =>
                {
                    o.RuntimeParameters = options.RuntimeParameters ?? objects;
                    o.MethodDescriptor = options.MethodDescriptor ?? (() => context.ParseMethodDescriptor(descriptor));
                    o.TimeMeasureCompletedThreshold = options.TimeMeasureCompletedThreshold;
                }).Result as IActionResult;
            }
        }

        internal static MethodDescriptor ParseMethodDescriptor(this FilterContext context, ControllerActionDescriptor descriptor)
        {
            if (descriptor == null) { return null; }
            return new MethodDescriptor(descriptor.MethodInfo);
        }
    }
}