using System;
using System.Collections.Generic;
using System.Reflection;
using Cuemon.Reflection;

namespace Cuemon
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="ExceptionUtility"/> class.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Unwraps the specified <paramref name="wrappedException"/>.
        /// </summary>
        /// <param name="wrappedException">The wrapped exception to unwrap.</param>
        /// <returns>The originating exception from within <see cref="MethodWrappedException"/>.</returns>
        public static Exception Unwrap(this MethodWrappedException wrappedException)
        {
            return ExceptionUtility.Unwrap(wrappedException);
        }

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

        /// <summary>
        /// Parses the specified <paramref name="exception"/> for a match on <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <paramref name="exception"/> to find a match on.</typeparam>
        /// <param name="exception">The exception to parse for a match on <typeparamref name="TResult"/>.</param>
        /// <returns>The matched <paramref name="exception"/> cast as <typeparamref name="TResult"/> or <c>null</c> if no match could be resolved.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception"/> is null.
        /// </exception>
        public static TResult ParseException<TResult>(this Exception exception) where TResult : Exception
        {
            return ExceptionUtility.Parse<TResult>(exception);
        }

        /// <summary>
        /// Flattens any inner exceptions descendant-or-self from the specified <paramref name="exception"/> into an <see cref="IEnumerable{T}"/> sequence of exceptions.
        /// </summary>
        /// <param name="exception">The exception to flatten.</param>
        /// <returns>An empty <see cref="IEnumerable{T}"/> sequence if no inner exceptions was specified; otherwise any inner exceptions rooted to the specified <paramref name="exception"/> as well as it's descendants.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception"/> is null.
        /// </exception>
        /// <remarks>
        /// If any inner exceptions are referenced this method will iterative flatten all of them descendant-or-self from the specified <paramref name="exception"/>.<br/>
        /// Should the <paramref name="exception"/> be of the new AggregateException type introduced with .NET 4.0, the return sequence of this method will be equal to the result of the InnerExceptions property.
        /// </remarks>
        public static IEnumerable<Exception> Flatten(this Exception exception)
        {
            return ExceptionUtility.Flatten(exception);
        }

        /// <summary>
        /// Flattens any inner exceptions descendant-or-self from the specified <paramref name="exception"/> into an <see cref="IEnumerable{T}"/> sequence of exceptions.
        /// </summary>
        /// <param name="exception">The exception to flatten.</param>
        /// <param name="exceptionType">The type of the specified <paramref name="exception"/>.</param>
        /// <returns>An empty <see cref="IEnumerable{T}"/> sequence if no inner exceptions was referenced; otherwise any inner exceptions descendant-or-self from the specified <paramref name="exception"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exception"/> -or <paramref name="exceptionType"/> is null.
        /// </exception>
        /// <remarks>
        /// If any inner exceptions are referenced this method will iterative flatten all of them descendant-or-self from the specified <paramref name="exception"/>.<br/>
        /// Should the <paramref name="exception"/> be of the new AggregateException type introduced with .NET 4.0, the return sequence of this method will be equal to the result of the InnerExceptions property.
        /// </remarks>
        public static IEnumerable<Exception> Flatten(this Exception exception, Type exceptionType)
        {
            return ExceptionUtility.Flatten(exception, exceptionType);
        }
    }
}