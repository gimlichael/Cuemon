using System;
using System.Collections.Generic;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cuemon.AspNetCore.Mvc
{
    internal static class Infrastructure
    {
        internal static void InterceptControllerWithProfiler(ActionExecutingContext context, Action<TimeMeasureOptions> setup)
        {
            var options = setup.ConfigureOptions();
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                var objects = new List<object>();
                foreach (var pi in context.ActionDescriptor.Parameters)
                {
                    objects.Add(ObjectConverter.ChangeType(context.RouteData.Values[pi.Name], pi.ParameterType));
                }
                context.Result = TimeMeasure.WithFunc(descriptor.MethodInfo.Invoke, context.Controller, objects.ToArray(), o =>
                {
                    o.RuntimeParameters = options.RuntimeParameters ?? objects.ToArray();
                    o.MethodDescriptor = options.MethodDescriptor ?? (() => new MethodDescriptor(descriptor.MethodInfo));
                    o.TimeMeasureCompletedThreshold = options.TimeMeasureCompletedThreshold;
                }).Result as IActionResult;
            }
        }
    }
}