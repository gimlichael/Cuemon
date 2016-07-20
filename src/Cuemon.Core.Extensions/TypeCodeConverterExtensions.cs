using System;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="TypeCodeConverter"/> class.
    /// </summary>
    public static class TypeCodeConverterExtensions
    {
        /// <summary>
        /// Gets the underlying type code of the specified <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose underlying <see cref="TypeCode"/> to get.</param>
        /// <returns>The code of the underlying type, or Empty if <paramref name="type"/> is null.</returns>
        public static TypeCode AsCode(this Type type)
        {
            return TypeCodeConverter.FromType(type);
        }
    }
}