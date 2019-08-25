using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cuemon.Reflection;
using Cuemon.Collections.Generic;

namespace Cuemon
{
	/// <summary>
    /// This utility class is designed to make exception operations more flexible and easier to work with.
	/// </summary>
	public static class ExceptionUtility
	{
	    private const string ThrowingMethod = "throwingMethod";

        /// <summary>
        /// Unwraps the specified <paramref name="wrappedException"/> and returns the originating exception.
        /// </summary>
        /// <param name="wrappedException">The wrapped exception to unwrap.</param>
        /// <returns>The originating exception from within <see cref="MethodWrappedException"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="wrappedException"/> is null.
        /// </exception>
        public static Exception Unwrap(MethodWrappedException wrappedException)
        {
            Validator.ThrowIfNull(wrappedException, nameof(wrappedException));
            var original = wrappedException.InnerException;
            if (!original.Data.Contains(ThrowingMethod)) { original.Data.Add(ThrowingMethod, wrappedException.ToString()); }
            return original;
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
        public static MethodWrappedException Refine(Exception exception, MethodBase method, params object[] parameters)
        {
            Validator.ThrowIfNull(method, nameof(method));
            return Refine(exception, MethodDescriptor.Create(method), parameters);
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
        public static MethodWrappedException Refine(Exception exception, MethodDescriptor method, params object[] parameters)
	    {
            Validator.ThrowIfNull(exception, nameof(exception));
            Validator.ThrowIfNull(method, nameof(method));
            return new MethodWrappedException(exception, method, method.MergeParameters(parameters));
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
        public static TResult Parse<TResult>(Exception exception) where TResult : Exception
        {
            var resultType = typeof(TResult);
            var exceptions = Flatten(exception);
            foreach (var e in exceptions)
            {
                var sourceType = e.GetType();
                if (TypeInsight.FromType(sourceType).HasType(resultType)) { return e as TResult; }
            }
            return null;
        }

        /// <summary>
        /// Parses the specified <paramref name="exceptions"/> for a match on <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Exception"/> to find a match within <paramref name="exceptions"/>.</typeparam>
        /// <param name="exceptions">A sequence of exceptions to parse for a match on <typeparamref name="TResult"/>.</param>
        /// <returns>The first matched <see cref="Exception"/> of <typeparamref name="TResult"/> from the sequence of <paramref name="exceptions"/> or <c>null</c> if no match could be resolved.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="exceptions"/> is null.
        /// </exception>
        public static TResult Parse<TResult>(IEnumerable<Exception> exceptions) where TResult : Exception
        {
            Validator.ThrowIfNull(exceptions, nameof(exceptions));
            var matches = exceptions.Where(e => e is TResult);
            return matches.FirstOrDefault() as TResult;
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
        public static IEnumerable<Exception> Flatten(Exception exception)
        {
            Validator.ThrowIfNull(exception, nameof(exception));
            return Flatten(exception, exception.GetType());
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
        public static IEnumerable<Exception> Flatten(Exception exception, Type exceptionType)
        {
            Validator.ThrowIfNull(exception, nameof(exception));
            Validator.ThrowIfNull(exceptionType, nameof(exceptionType));
            var innerExceptionsProperty = exceptionType.GetProperty("InnerExceptions", new MemberReflection(excludeInheritancePath: true));
            if (innerExceptionsProperty != null) { return innerExceptionsProperty.GetValue(exception, null) as IEnumerable<Exception>; }
            return Hierarchy.WhileSourceTraversalHasElements(exception, FlattenCallback).Skip(1);
        }

        private static IEnumerable<Exception> FlattenCallback(Exception source)
        {
            var innerExceptionsProperty = source.GetType().GetProperty("InnerExceptions", new MemberReflection(excludeInheritancePath: true));
            if (innerExceptionsProperty != null) { return innerExceptionsProperty.GetValue(source, null) as IEnumerable<Exception>; }
            return source.InnerException == null ? Enumerable.Empty<Exception>() : Arguments.Yield(source.InnerException);
        }
    }
}