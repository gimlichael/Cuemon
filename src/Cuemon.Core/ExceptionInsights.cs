using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Cuemon.Diagnostics;
using Cuemon.Integrity;
using Cuemon.Reflection;
using Cuemon.Text;
using Cuemon.Threading;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods for embedding environment specific insights to an exception.
    /// </summary>
    public static class ExceptionInsights
    {
        private const string ExceptionInsightsKey = "$(___exceptionInsights___)";
        private const int IndexOfThrower = 0;
        private const int IndexOfRuntimeParameters = 1;
        private const int IndexOfThreadInfo = 2;
        private const int IndexOfProcessInfo = 3;
        private const int IndexOfEnvironmentInfo = 4;

        /// <summary>
        /// Enriches and embed insights about an <see cref="Exception"/> that can be extracted with <see cref="ToExceptionDescriptor"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="exception"/>.</typeparam>
        /// <param name="exception">The exception to enrich.</param>
        /// <param name="runtimeParameters">The runtime parameters of the method that threw the <paramref name="exception"/>.</param>
        /// <param name="snapshot">A bitwise combination of the enumeration values that specify which areas of a system to capture.</param>
        /// <returns>The provided <paramref name="exception"/> enriched with an embedded entry of insights.</returns>
        /// <seealso cref="ToExceptionDescriptor"/>
        public static T Embed<T>(T exception, object[] runtimeParameters = null, SystemSnapshot snapshot = SystemSnapshot.None) where T : Exception
        {
            return Embed(exception, null, runtimeParameters, snapshot);
        }

        /// <summary>
        /// Enriches and embed insights about an <see cref="Exception"/> that can be extracted with <see cref="ToExceptionDescriptor"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="exception"/>.</typeparam>
        /// <param name="exception">The exception to enrich.</param>
        /// <param name="thrower">The method that threw the <paramref name="exception"/>.</param>
        /// <param name="runtimeParameters">The runtime parameters of the <paramref name="thrower"/>.</param>
        /// <param name="snapshot">A bitwise combination of the enumeration values that specify which areas of a system to capture.</param>
        /// <returns>The provided <paramref name="exception"/> enriched with an embedded entry of insights.</returns>
        /// <seealso cref="ToExceptionDescriptor"/>
        public static T Embed<T>(T exception, MethodBase thrower, object[] runtimeParameters, SystemSnapshot snapshot = SystemSnapshot.None) where T : Exception
        {
            Validator.ThrowIfNull(exception, nameof(exception));
            var builder = new StringBuilder();
            var empty = Convert.ToBase64String(Convertible.GetBytes(""));
            if (thrower != null || exception.TargetSite != null)
            {
                var descriptor = new MethodDescriptor(thrower ?? exception.TargetSite);
                builder.Append(Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{descriptor.ToString()}"))));
                builder.Append(".");
                if (runtimeParameters != null && runtimeParameters.Any())
                {
                    var rp = DelimitedString.Create(MethodDescriptor.MergeParameters(descriptor, runtimeParameters), o =>
                    {
                        o.StringConverter = pair => FormattableString.Invariant($"{pair.Key}={pair.Value}");
                        o.Delimiter = FormattableString.Invariant($"{Alphanumeric.NewLine}");
                    });
                    builder.Append(Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{rp}"))));
                }
                else
                {
                    builder.Append(empty);
                }
            }
            else
            {
                builder.Append(empty);
                builder.Append(".");
                builder.Append(empty);
            }
            builder.Append(".");
            if (snapshot.HasFlag(SystemSnapshot.CaptureThreadInfo))
            {
                var ti = string.Join(Alphanumeric.NewLine, new ThreadInfo(Thread.CurrentThread).ToString().Split('^'));
                builder.Append(ti.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{ti}"))) : empty);
            }
            else
            {
                builder.Append(empty);
            }
            builder.Append(".");
            if (snapshot.HasFlag(SystemSnapshot.CaptureProcessInfo))
            {
                var pi = string.Join(Alphanumeric.NewLine, new ProcessInfo(Process.GetCurrentProcess()).ToString().Split('^'));
                builder.Append(pi.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{pi}"))) : empty);
            }
            else
            {
                builder.Append(empty);
            }
            builder.Append(".");
            if (snapshot.HasFlag(SystemSnapshot.CaptureEnvironmentInfo))
            {
                var ei = string.Join(Alphanumeric.NewLine, new EnvironmentInfo().ToString().Split('^'));
                builder.Append(ei.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{ei}"))) : empty);
            }
            else
            {
                builder.Append(empty);
            }
            if (exception.Data[ExceptionInsightsKey] == null) { exception.Data.Add(ExceptionInsightsKey, builder.ToString()); }
            return exception;
        }

        /// <summary>
        /// Converts the specified <paramref name="exception"/> to a developer friendly <see cref="ExceptionDescriptor"/>.
        /// </summary>
        /// <param name="exception">The exception to convert.</param>
        /// <param name="code">The error code that uniquely identifies the type of failure. Default is "UnhandledException".</param>
        /// <param name="message">The message that explains the reason for the failure. Default is "An unhandled exception occurred.".</param>
        /// <param name="helpLink">The optional link to a help page associated with this failure.</param>
        /// <returns>An <see cref="ExceptionDescriptor"/> initialized with the provided arguments, and (if embedded) enriched with insights of the <paramref name="exception"/>.</returns>
        /// <seealso cref="Embed{T}(T,object[],SystemSnapshot)"/>
        /// <seealso cref="Embed{T}(T,MethodBase,object[],SystemSnapshot)"/>
        public static ExceptionDescriptor ToExceptionDescriptor(Exception exception, string code = "UnhandledException", string message = "An unhandled exception occurred.", Uri helpLink = null)
        {
            Validator.ThrowIfNull(exception, nameof(exception));
            var ed = new ExceptionDescriptor(exception, code, message, helpLink);
            var base64Segments = RetrieveAndSweepExceptionData(exception);
            if (!string.IsNullOrWhiteSpace(base64Segments))
            {
                var insights = base64Segments.Split('.');
                if (insights.Length == 5)
                {
                    var builder = new StringBuilder();
                    builder.AppendLine(exception.ToString());
                    var memberSignature = Convertible.ToString(Convert.FromBase64String(insights[IndexOfThrower]));
                    var runtimeParameters = Convertible.ToString(Convert.FromBase64String(insights[IndexOfRuntimeParameters]));
                    ed.AddEvidence("Thrower", new MemberEvidence(memberSignature, string.IsNullOrWhiteSpace(runtimeParameters) ? null : runtimeParameters.Split(Alphanumeric.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToDictionary(k => k.Substring(0, k.IndexOf('=')), v => v.Substring(v.IndexOf('=') + 1))), evidence => evidence);
                    TryAddEvindence(ed, "Thread", insights[IndexOfThreadInfo]);
                    TryAddEvindence(ed, "Process", insights[IndexOfProcessInfo]);
                    TryAddEvindence(ed, "Environment", insights[IndexOfEnvironmentInfo]);
                }
            }
            return ed;
        }

        private static string RetrieveAndSweepExceptionData(Exception exception)
        {
            string result = null;
            var stack = new Stack<Exception>();
            stack.Push(exception);
            while (stack.Count > 0)
            {
                var e = stack.Pop();
                if (e.Data[ExceptionInsightsKey] is string base64Segments && !string.IsNullOrWhiteSpace(base64Segments))
                {
                    if (result == null) { result = base64Segments; }
                    e.Data.Remove(ExceptionInsightsKey);
                }
                if (e.InnerException != null) { stack.Push(e.InnerException); }
            }
            return result;
        }

        private static void TryAddEvindence(ExceptionDescriptor ed, string context, string base64)
        {
            var info = Convertible.ToString(Convert.FromBase64String(base64));
            if (!string.IsNullOrWhiteSpace(info))
            {
                ed.AddEvidence(context, info.Split(Alphanumeric.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries), s => s.ToDictionary(k => k.Substring(0, k.IndexOf(':')), v =>
                {
                    var presult = v.Substring(v.IndexOf(' ') + 1);
                    return ParseFactory.FromSimpleValue().Parse(presult == "null" ? null : presult);
                }));
            }
        }
    }
}