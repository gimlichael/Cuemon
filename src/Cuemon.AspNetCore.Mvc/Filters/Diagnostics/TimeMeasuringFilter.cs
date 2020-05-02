using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cuemon.AspNetCore.Http.Headers;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// A filter that performs time measure profiling of an action method.
    /// </summary>
    /// <seealso cref="ConfigurableActionFilter{TOptions}"/>
    /// <seealso cref="IActionFilter" />
    public class TimeMeasuringFilter : ConfigurableActionFilter<TimeMeasuringOptions>
    {
        #if NETSTANDARD
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasuringFilter" /> class.
        /// </summary>
        /// <param name="setup">The <see cref="TimeMeasureOptions" /> which need to be configured.</param>
        /// <param name="he">The dependency injected <see cref="IHostingEnvironment"/>.</param>
        public TimeMeasuringFilter(IOptions<TimeMeasuringOptions> setup, IHostingEnvironment he) : base(setup)
        {
            Profiler = new TimeMeasureProfiler();
            Environment = he;
        }
        #elif NETCOREAPP
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeMeasuringFilter" /> class.
        /// </summary>
        /// <param name="setup">The <see cref="TimeMeasureOptions" /> which need to be configured.</param>
        /// <param name="he">The dependency injected <see cref="IHostEnvironment"/>.</param>
        public TimeMeasuringFilter(IOptions<TimeMeasuringOptions> setup, IHostEnvironment he) : base(setup)
        {
            Profiler = new TimeMeasureProfiler();
            Environment = he;
        }
        #endif

        #if NETSTANDARD
        private IHostingEnvironment Environment { get; }
        #elif NETCOREAPP
        private IHostEnvironment Environment { get; }
        #endif

        private TimeMeasureProfiler Profiler { get; }

        /// <summary>
        /// Called before the action executes, after model binding is complete.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutingContext" />.</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Profiler.Timer.Start();
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var expectedObjects = ParseRuntimeParameters(context, descriptor);
                var verifiedObjects = context.ActionArguments.Values.ToArray();
                if (verifiedObjects.Length == expectedObjects.Length) { expectedObjects = verifiedObjects; }
                var md = Options.MethodDescriptor?.Invoke() ?? ParseMethodDescriptor(descriptor);
                Profiler.Member = md.ToString();
                Profiler.Data = md.MergeParameters(Options.RuntimeParameters ?? expectedObjects);
            }
        }

        /// <summary>
        /// Called after the action executes, before the action result.
        /// </summary>
        /// <param name="context">The <see cref="ActionExecutedContext" />.</param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Profiler.Timer.Stop();
            if (Options.TimeMeasureCompletedThreshold == TimeSpan.Zero || Profiler.Elapsed > Options.TimeMeasureCompletedThreshold)
            {
                TimeMeasure.CompletedCallback?.Invoke(Profiler);
                if (!Options.SuppressHeaderPredicate(Environment))
                {
                    if (Options.UseServerTimingHeader) { Decorator.Enclose(context.HttpContext.Response.Headers).TryAddOrUpdateHeader("Server-Timing", FormattableString.Invariant($"CPU;dur={Profiler.Elapsed.TotalMilliseconds.ToString("N1", CultureInfo.InvariantCulture)}")); }
                    if (Options.UseCustomHeader) { Decorator.Enclose(context.HttpContext.Response.Headers).TryAddOrUpdateHeader(Options.HeaderName, Profiler.ToString()); }
                }
            }
        }

        private static MethodDescriptor ParseMethodDescriptor(ControllerActionDescriptor descriptor)
        {
            return descriptor == null ? null : new MethodDescriptor(descriptor.MethodInfo);
        }

        private static object[] ParseRuntimeParameters(FilterContext context, ControllerActionDescriptor descriptor)
        {
            if (descriptor == null) { return new object[0]; }
            var objects = new List<object>();
            foreach (var pi in descriptor.Parameters)
            {
                objects.Add(ConvertFactory.FromObject().ChangeType(context.RouteData.Values[pi.Name], pi.ParameterType));
            }
            return objects.ToArray();
        }
    }
}