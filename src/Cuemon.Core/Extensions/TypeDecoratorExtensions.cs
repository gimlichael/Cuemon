using System;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="Type"/> class tailored to adhere the decorator pattern.
    /// </summary>
    /// <seealso cref="IDecorator{T}"/>
    /// <seealso cref="Decorator{T}"/>
    public static class TypeDecoratorExtensions
    {
        /// <summary>
        /// Determines whether the enclosed <see cref="Type"/> of the <paramref name="decorator"/> contains one or more of the specified target types.
        /// </summary>
        /// <param name="decorator">The <see cref="IDecorator{T}"/> to extend.</param>
        /// <param name="targets">The target types to be matched against.</param>
        /// <returns><c>true</c> if the enclosed <see cref="Type"/> of the <paramref name="decorator"/> contains one or more of the specified target types; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null -or-
        /// <paramref name="targets"/> cannot be null.
        /// </exception>
        public static bool HasTypes(this IDecorator<Type> decorator, params Type[] targets)
        {
            Validator.ThrowIfNull(decorator, nameof(decorator));
            Validator.ThrowIfNull(targets, nameof(targets));
            foreach (var tt in targets)
            {
                var st = decorator.Inner;
                while (st != typeof(object) && st != null)
                {
                    if (st.IsGenericType && tt == st.GetGenericTypeDefinition()) { return true; }
                    if (st == tt) { return true; }
                    st = st.BaseType;
                }
            }
            return false;
        }
    }
}