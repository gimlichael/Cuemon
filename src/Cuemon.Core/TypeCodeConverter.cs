using System;
using System.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// This utility class is designed to make <see cref="TypeCode"/> related conversions easier to work with.
    /// </summary>
    public static class TypeCodeConverter
    {
        private static readonly IDictionary<Type, TypeCode> TypeToTypeCodeLookupTable = InitTypeToTypeCodeLookupTable();

        private static IDictionary<Type, TypeCode> InitTypeToTypeCodeLookupTable()
        {
            Dictionary<Type, TypeCode> result = new Dictionary<Type, TypeCode>();
            foreach (var pair in EnumUtility.ToEnumerable<TypeCode>())
            {
                try
                {
                    Type validType = Type.GetType(string.Format("System.{0}", pair.Value), false);
                    if (validType != null) { result.Add(validType, EnumUtility.Parse<TypeCode>(pair.Value)); }
                }
                catch (Exception)
                {
                }
            }
            return result;
        }

        /// <summary>
        /// Converts the specified <paramref name="source"/> to its equivalent <see cref="TypeCode"/> representation.
        /// </summary>
        /// <param name="source">The <see cref="Type"/> to be converted.</param>
        /// <returns>A <see cref="TypeCode"/> representation of the specified <paramref name="source"/>.</returns>
        public static TypeCode FromType(Type source)
        {
            if (source == null) { return TypeCode.Empty; }
            TypeCode result;
            if (!TypeToTypeCodeLookupTable.TryGetValue(source, out result))
            {
                return TypeCode.Object;
            }
            return result;
        }
    }
}