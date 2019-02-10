using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Extension methods for the <see cref="IWrapper{T}"/>.
    /// </summary>
    public static class WrapperExtensions
    {
        /// <summary>
        /// Wrap and extend an existing object of <typeparamref name="T"/> with additional data.
        /// </summary>
        /// <typeparam name="T">The type of the object to extend.</typeparam>
        /// <param name="instance">The instance to wrap and extend.</param>
        /// <param name="extender">The delegate that provides an easy way of supplying additional data to an object.</param>
        /// <returns>An implementation of <see cref="IWrapper{T}"/> encapsulating the specified <paramref name="instance"/>.</returns>
        public static IWrapper<T> UseWrapper<T>(this T instance, Action<IDictionary<string, object>> extender = null)
        {
            return UseWrapper(instance, null, extender);
        }

        /// <summary>
        /// Wrap and extend an existing object of <typeparamref name="T"/> with additional data.
        /// </summary>
        /// <typeparam name="T">The type of the object to extend.</typeparam>
        /// <param name="instance">The instance to wrap and extend.</param>
        /// <param name="memberReference">The optional member reference to assign <see cref="IWrapper{T}.MemberReference"/>.</param>
        /// <param name="extender">The delegate that provides an easy way of supplying additional data to an object.</param>
        /// <returns>An implementation of <see cref="IWrapper{T}"/> encapsulating the specified <paramref name="instance"/>.</returns>
        public static IWrapper<T> UseWrapper<T>(this T instance, MemberInfo memberReference, Action<IDictionary<string, object>> extender = null)
        {
            var wrapper = new Wrapper<T>(instance, memberReference);
            extender?.Invoke(wrapper.Data);
            return wrapper;
        }
    }
}