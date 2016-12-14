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
                var objects = context.ParseRuntimeParameters(descriptor);
                context.Result = TimeMeasure.WithFunc(descriptor.MethodInfo.Invoke, context.Controller, objects, o =>
                {
                    o.RuntimeParameters = options.RuntimeParameters ?? objects;
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