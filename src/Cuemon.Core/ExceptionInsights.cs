using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Cuemon.Diagnostics;
using Cuemon.Reflection;
using Cuemon.Threading;

namespace Cuemon
{
    /// <summary>
    /// Provides a set of static methods for embedding environment specific insights to an exception.
    /// </summary>
    public static class ExceptionInsights
    {
        /// <summary>
        /// The <see cref="Key"/> used when applying insights to the <see cref="Exception.Data"/> dictionary.
        /// </summary>
        public const string Key = "$(___exceptionInsights___)";

        /// <summary>
        /// Enriches and embed insights about an <see cref="Exception"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="exception"/>.</typeparam>
        /// <param name="exception">The exception to enrich.</param>
        /// <param name="runtimeParameters">The runtime parameters of the method that threw the <paramref name="exception"/>.</param>
        /// <param name="snapshot">A bitwise combination of the enumeration values that specify which areas of a system to capture.</param>
        /// <returns>The provided <paramref name="exception"/> enriched with an embedded entry of insights.</returns>
        public static T Embed<T>(T exception, object[] runtimeParameters = null, SystemSnapshot snapshot = SystemSnapshot.None) where T : Exception
        {
            return Embed(exception, null, runtimeParameters, snapshot);
        }

        /// <summary>
        /// Enriches and embed insights about an <see cref="Exception"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <paramref name="exception"/>.</typeparam>
        /// <param name="exception">The exception to enrich.</param>
        /// <param name="thrower">The method that threw the <paramref name="exception"/>.</param>
        /// <param name="runtimeParameters">The runtime parameters of the <paramref name="thrower"/>.</param>
        /// <param name="snapshot">A bitwise combination of the enumeration values that specify which areas of a system to capture.</param>
        /// <returns>The provided <paramref name="exception"/> enriched with an embedded entry of insights.</returns>
        public static T Embed<T>(T exception, MethodBase thrower, object[] runtimeParameters, SystemSnapshot snapshot = SystemSnapshot.None) where T : Exception
        {
            Validator.ThrowIfNull(exception, nameof(exception));
            var builder = new StringBuilder();
            var empty = Convert.ToBase64String(Convertible.GetBytes(""));
            if (thrower != null || exception.TargetSite != null)
            {
                var descriptor = new MethodDescriptor(thrower ?? exception.TargetSite);
                builder.Append(Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{descriptor.ToString()}"))));
                builder.Append('.');
                if (runtimeParameters != null && runtimeParameters.Any())
                {
                    var rp = DelimitedString.Create(MethodDescriptor.MergeParameters(descriptor, runtimeParameters), o =>
                    {
                        o.StringConverter = pair => FormattableString.Invariant($"{pair.Key}={pair.Value ?? "null"}");
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
                builder.Append('.');
                builder.Append(empty);
            }
            EmbedSystemSnapshot(builder, snapshot, empty);
            if (exception.Data[Key] == null) { exception.Data.Add(Key, builder.ToString()); }
            return exception;
        }

        private static void EmbedSystemSnapshot(StringBuilder builder, SystemSnapshot snapshot, string empty)
        {
            builder.Append('.');
            if (snapshot.HasFlag(SystemSnapshot.CaptureThreadInfo))
            {
                var ti = string.Join(Alphanumeric.NewLine, new ThreadInfo(Thread.CurrentThread).ToString().Split('^'));
                builder.Append(ti.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{ti}"))) : empty);
            }
            else
            {
                builder.Append(empty);
            }
            builder.Append('.');
            if (snapshot.HasFlag(SystemSnapshot.CaptureProcessInfo))
            {
                var pi = string.Join(Alphanumeric.NewLine, new ProcessInfo(Process.GetCurrentProcess()).ToString().Split('^'));
                builder.Append(pi.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{pi}"))) : empty);
            }
            else
            {
                builder.Append(empty);
            }
            builder.Append('.');
            if (snapshot.HasFlag(SystemSnapshot.CaptureEnvironmentInfo))
            {
                var ei = string.Join(Alphanumeric.NewLine, new EnvironmentInfo().ToString().Split('^'));
                builder.Append(ei.Length > 0 ? Convert.ToBase64String(Convertible.GetBytes(FormattableString.Invariant($"{ei}"))) : empty);
            }
            else
            {
                builder.Append(empty);
            }
        }
    }
}