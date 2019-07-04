using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Reflection
{
    public static class PropertyInfoExtensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="property"/> is considered an automatic property implementation.
        /// </summary>
        /// <param name="property">The property to check for automatic property implementation.</param>
        /// <returns><c>true</c> if the specified <paramref name="property"/> is considered an automatic property implementation; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="property"/> is null.
        /// </exception>
        public static bool IsAutoProperty(this PropertyInfo property)
        {
            Validator.ThrowIfNull(property, nameof(property));
            return ((PropertyInsight)property).IsAutoProperty();
        }
    }
}