using System;
using System.Collections.Generic;
using Cuemon.Collections.Generic;
using Cuemon.ComponentModel.Parsers;

namespace Cuemon.ComponentModel.Converters
{
    /// <summary>
    /// Provides a converter that converts a <see cref="Type"/> to a <see cref="TypeCode"/>.
    /// Implements the <see cref="IConverter{TInput,TOutput}" />
    /// </summary>
    /// <seealso cref="IConverter{TInput,TOutput}" />
    public class TypeCodeConverter : IConverter<Type, TypeCode>
    {
        private static readonly IDictionary<Type, TypeCode> LookupTable = InitLookupTable();

        private static IDictionary<Type, TypeCode> InitLookupTable()
        {
            var result = new Dictionary<Type, TypeCode>();
            foreach (var pair in new EnumReadOnlyDictionary<TypeCode>())
            {
                try
                {
                    var validType = Type.GetType(FormattableString.Invariant($"System.{pair.Value}"), false);
                    if (validType != null) { result.Add(validType, ConvertFactory.UseParser<EnumParser>().Parse<TypeCode>(pair.Value)); }
                }
                catch
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="input"/> to a <see cref="TypeCode"/> representation.
        /// </summary>
        /// <param name="input">The <see cref="Type"/> to convert.</param>
        /// <returns>A <see cref="TypeCode"/> enumeration that represents the <paramref name="input"/>.</returns>
        public TypeCode ChangeType(Type input)
        {
            if (input == null) { return TypeCode.Empty; }
            if (!LookupTable.TryGetValue(input, out var result))
            {
                return TypeCode.Object;
            }
            return result;
        }
    }
}