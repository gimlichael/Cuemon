using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Cuemon.Reflection;

namespace Cuemon.ComponentModel.Converters
{
    /// <summary>
    /// Provides a converter that converts an arbitrary <see cref="object"/> to a <see cref="string" />.
    /// Implements the <see cref="IConverter{TInput,TOutput,TOptions}" />
    /// </summary>
    /// <seealso cref="IConverter{TInput,TOutput,TOptions}" />
    public class ObjectToStringConverter : IConverter<object, string, ObjectToStringOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="string"/> representation that might contain information about the object state.
        /// </summary>
        /// <param name="input">The object to convert.</param>
        /// <param name="setup">The <see cref="ObjectToStringOptions"/> which may be configured.</param>
        /// <returns>A <see cref="string"/> that represents the <paramref name="input"/>.</returns>
        /// <remarks>
        /// When determining the representation of the specified <paramref name="input"/>, these default rules applies:
        /// 1: if the <see cref="object.ToString"/> method has been overridden, any further processing is skipped (the assumption is, that a custom representation is already in place)
        /// 2: any public properties having index parameters is skipped
        /// 3: any public properties is appended to the result if <see cref="object.ToString"/> has not been overridden
        /// Note: do not call this method from an overridden ToString(..) method without setting <see cref="ObjectToStringOptions.BypassOverrideCheck"/> to <c>true</c>; otherwise a <see cref="StackOverflowException"/> will occur.
        /// </remarks>
        /// <seealso cref="TypeToStringConverter"/>
        /// <seealso cref="DelimitedStringConverter{T}"/>
        public string ChangeType(object input, Action<ObjectToStringOptions> setup = null)
        {
            var options = Patterns.Configure(setup);

            if (input == null) { return options.NullValue; }

            if (!options.BypassOverrideCheck)
            {
                if (TypeInsight.FromInstance(input).When(type => type.GetMethods().SingleOrDefault(m => m.Name == nameof(ToString) && m.GetParameters().Length == 0), out var mi).IsOverridden())
                {
                    var stringResult = input.ToString();
                    return mi.DeclaringType == typeof(bool) ? stringResult.ToLowerInvariant() : stringResult;
                }
            }

            var instanceType = input.GetType();
            var instanceSignature = new StringBuilder(string.Format(options.FormatProvider, "{0}", ConvertFactory.UseConverter<TypeToStringConverter>().ChangeType(instanceType, o => o.FullName = true)));
            var properties = instanceType.GetRuntimeProperties().Where(options.PropertiesPredicate);
            instanceSignature.AppendFormat(" {{ {0} }}", ConvertFactory.UseConverter<DelimitedStringConverter<PropertyInfo>>().ChangeType(properties, o =>
            {
                o.Delimiter = options.Delimiter;
                o.StringConverter = pi => options.PropertyConverter(pi, input, options.FormatProvider);
            }));
            return instanceSignature.ToString();
        }
    }
}