using System;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon.Extensions.Reflection
{
    /// <summary>
    /// Extension methods for the <see cref="Exception"/> class.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Refines the specified <paramref name="exception"/> with valuable meta information extracted from the associated <paramref name="method"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="exception">The exception that needs to be thrown.</param>
        /// <param name="method">The method to extract valuable meta information from.</param>
        /// <param name="parameters">The optional parameters to accompany <paramref name="method"/>.</param>
        /// <returns>The specified <paramref name="exception"/> refined with valuable meta information within a <see cref="MethodWrappedException"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        public static MethodWrappedException Refine(this Exception exception, MethodBase method, params object[] parameters)
        {
            return ExceptionUtility.Refine(exception, method, parameters);
        }

        /// <summary>
        /// Refines the specified <paramref name="exception"/> with valuable meta information extracted from the associated <paramref name="method"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <param name="exception">The exception that needs to be thrown.</param>
        /// <param name="method">The method signature containing valuable meta information.</param>
        /// <param name="parameters">The optional parameters to accompany <paramref name="method"/>.</param>
        /// <returns>The specified <paramref name="exception"/> refined with valuable meta information within a <see cref="MethodWrappedException"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        public static MethodWrappedException Refine(this Exception exception, MethodDescriptor method, params object[] parameters)
        {
            return ExceptionUtility.Refine(exception, method, parameters);
        }
    }
}