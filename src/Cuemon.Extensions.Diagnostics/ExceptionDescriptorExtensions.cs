using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cuemon.Diagnostics;
using Cuemon.Runtime.Serialization;

namespace Cuemon.Extensions.Diagnostics
{
    /// <summary>
    /// Extension methods for the <see cref="ExceptionDescriptor"/> class.
    /// </summary>
    public static class ExceptionDescriptorExtensions
    {
        /// <summary>
        /// Converts the specified <paramref name="descriptor"/> to its equivalent string representation.
        /// </summary>
        /// <param name="descriptor">The <see cref="ExceptionDescriptor"/> to extend.</param>
        /// <param name="setup">The <see cref="ExceptionDescriptorSerializationOptions"/> which may be configured.</param>
        /// <returns>A string that represents the specified <paramref name="descriptor"/>.</returns>
        public static string ToInsightsString(this ExceptionDescriptor descriptor, Action<ExceptionDescriptorSerializationOptions> setup = null)
        {
            Validator.ThrowIfNull(descriptor, nameof(descriptor));
            var options = Patterns.Configure(setup);
            var builder = new StringBuilder();
            builder.AppendLine("Error");
            builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}Code: {descriptor.Code}"));
            builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}Message: {descriptor.Message}"));
            if (descriptor.HelpLink != null) { builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}HelpLink: {descriptor.HelpLink.OriginalString}")); }
            if (options.IncludeFailure)
            {
                builder.AppendLine();
                builder.AppendLine("Failure");
                AppendException(builder, descriptor.Failure, options.IncludeStackTrace);
            }

            if (options.IncludeEvidence && descriptor.Evidence.Any())
            {
                builder.AppendLine();
                builder.AppendLine("Evidence");
                foreach (var evidence in descriptor.Evidence)
                {
                    if (evidence.Value == null) { continue; }
                    AppendMemberEvidence(builder, evidence);
                    AppendDictionaryEvidence(builder, evidence);
                    builder.AppendLine();
                }
            }
            return builder.ToString();
        }

        private static void AppendMemberEvidence(StringBuilder builder, KeyValuePair<string, object> evidence)
        {
            if (evidence.Value is MemberEvidence me)
            {
                builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{evidence.Key}:"));
                builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}{me.MemberSignature}"));
                if (me.RuntimeParameters.Count > 0)
                {
                    builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}{{"));
                    foreach (var kvp in me.RuntimeParameters)
                    {
                        builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}{Alphanumeric.Tab}{kvp.Key}={kvp.Value}"));
                    }
                    builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}}}"));
                }
            }
        }

        private static void AppendDictionaryEvidence(StringBuilder builder, KeyValuePair<string, object> evidence)
        {
            if (evidence.Value is IDictionary<string, object> dic)
            {
                builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{evidence.Key}:"));
                if (dic.Count > 0)
                {
                    foreach (var kvp in dic)
                    {
                        builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}{kvp.Key}={kvp.Value ?? "null"}"));
                    }
                }
            }
        }

        private static void AppendException(StringBuilder builder, Exception exception, bool includeStackTrace)
        {
            var exceptionType = exception.GetType();
            builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{exceptionType.FullName}"));
            AppendExceptionCore(builder, exception, includeStackTrace);
        }

        private static void AppendExceptionCore(StringBuilder builder, Exception exception, bool includeStackTrace)
        {
            if (!string.IsNullOrEmpty(exception.Source))
            {
                builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}Source: {exception.Source}"));
            }

            if (!string.IsNullOrEmpty(exception.Message))
            {
                builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}Message: {exception.Message.Replace(Alphanumeric.NewLine, string.Concat(Alphanumeric.NewLine, Alphanumeric.Tab, Alphanumeric.Tab, Alphanumeric.Tab))}"));
            }

            if (exception.StackTrace != null && includeStackTrace)
            {
                builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}Stack:"));
                var lines = exception.StackTrace.Split(new[] { Alphanumeric.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var line in lines)
                {
                    builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}{Alphanumeric.Tab}{line.Trim()}"));
                }
            }

            if (exception.Data.Count > 0)
            {
                builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}Data:"));
                foreach (DictionaryEntry entry in exception.Data)
                {
                    builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}{Alphanumeric.Tab}{entry.Key}={entry.Value}"));
                }
            }

            var properties = Decorator.Enclose(exception.GetType()).GetRuntimePropertiesExceptOf<AggregateException>().Where(pi => !Decorator.Enclose(pi.PropertyType).IsComplex());
            foreach (var property in properties)
            {
                var value = property.GetValue(exception);
                if (value == null) { continue; }
                builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{Alphanumeric.Tab}{property.Name}: {value}"));
            }

            AppendInnerExceptions(builder, exception, includeStackTrace);
        }

        private static void AppendInnerExceptions(StringBuilder builder, Exception exception, bool includeStackTrace)
        {
            var aggregated = exception as AggregateException;
            var innerExceptions = new List<Exception>();
            if (aggregated != null) { innerExceptions.AddRange(aggregated.Flatten().InnerExceptions); }
            if (exception.InnerException != null) { innerExceptions.Add(exception.InnerException); }
            if (innerExceptions.Count > 0)
            {
                foreach (var inner in innerExceptions)
                {
                    var exceptionType = inner.GetType();
                    builder.AppendLine(FormattableString.Invariant($"{Alphanumeric.Tab}{exceptionType.FullName}"));
                    AppendExceptionCore(builder, inner, includeStackTrace);
                }
            }
        }
    }
}