using System;
using System.Reflection;

namespace Cuemon.ComponentModel.Converters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="Type"/> to a <see cref="string" />.
    /// Implements the <see cref="IConverter{TInput,TOutput,TOptions}" />
    /// </summary>
    /// <seealso cref="IConverter{TInput,TOutput,TOptions}" />
    public class TypeToStringConverter : IConverter<Type, string, TypeToStringOptions>
    {
        /// <summary>
        /// Converts the specified <paramref name="input"/> to a sanitized <see cref="string"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Type"/> to convert.</param>
        /// <param name="setup">The <see cref="TypeToStringOptions"/> which may be configured.</param>
        /// <returns>A sanitized <see cref="string"/> that represents the <paramref name="input"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> cannot be null.
        /// </exception>
        /// <seealso cref="DelimitedStringConverter{T}"/>
        public string ChangeType(Type input, Action<TypeToStringOptions> setup = null)
        {
            Validator.ThrowIfNull(input, nameof(input));
            var options = Patterns.Configure(setup);
            var typeName = options.TypeConverter(input, options.FormatProvider, options.FullName);
            if (!input.GetTypeInfo().IsGenericType) { return typeName; }
            var parameters = input.GetGenericArguments();
            var indexOfGraveAccent = typeName.IndexOf('`');
            typeName = indexOfGraveAccent >= 0 ? typeName.Remove(indexOfGraveAccent) : typeName;
            var delimitedString = ConvertFactory.UseConverter<DelimitedStringConverter<Type>>().ChangeType(parameters, o =>
            {
                o.Delimiter = ",";
                o.StringConverter = type => options.TypeConverter(type, options.FormatProvider, options.FullName);
            });
            return options.ExcludeGenericArguments ? typeName : string.Format(options.FormatProvider, "{0}<{1}>", typeName, delimitedString);
        }
    }
}