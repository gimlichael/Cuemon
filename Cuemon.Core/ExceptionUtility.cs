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
        /// <exception cref="System.ArgumentNullException">
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
        /// <exception cref="System.ArgumentNullException">
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
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="exception"/> is null.
        /// </exception>
        public static TResult Parse<TResult>(Exception exception) where TResult : Exception
        {
            Type resultType = typeof(TResult);
            var exceptions = Flatten(exception);
            foreach (var e in exceptions)
            {
                Type sourceType = e.GetType();
                if (TypeUtility.ContainsType(sourceType, resultType)) { return e as TResult; }
            }
            return null;
        }

        /// <summary>
        /// Parses the specified <paramref name="exceptions"/> for a match on <typeparamref name="TResult"/>.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="Exception"/> to find a match within <paramref name="exceptions"/>.</typeparam>
        /// <param name="exceptions">A sequence of exceptions to parse for a match on <typeparamref name="TResult"/>.</param>
        /// <returns>The first matched <see cref="Exception"/> of <typeparamref name="TResult"/> from the sequence of <paramref name="exceptions"/> or <c>null</c> if no match could be resolved.</returns>
        /// <exception cref="System.ArgumentNullException">
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
        /// <exception cref="System.ArgumentNullException">
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
        /// <exception cref="System.ArgumentNullException">
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
            PropertyInfo innerExceptionsProperty = ReflectionUtility.GetProperty(exceptionType, "InnerExceptions");
            if (innerExceptionsProperty != null) { return innerExceptionsProperty.GetValue(exception, null) as IEnumerable<Exception>; }
            return HierarchyUtility.WhileSourceTraversalHasElements(exception, FlattenCallback).Skip(1);
        }

        private static IEnumerable<Exception> FlattenCallback(Exception source)
        {
            PropertyInfo innerExceptionsProperty = ReflectionUtility.GetProperty(source.GetType(), "InnerExceptions");
            if (innerExceptionsProperty != null) { return innerExceptionsProperty.GetValue(source, null) as IEnumerable<Exception>; }
            return source.InnerException == null ? Enumerable.Empty<Exception>() : source.InnerException.Yield();
        }

        /// <summary>
        /// Creates and returns a new <see cref="ArgumentOutOfRangeException"/> initialized to the provided parameters.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <returns>A new instance of <see cref="ArgumentOutOfRangeException"/> initialized to the provided parameters.</returns>
	    public static ArgumentOutOfRangeException CreateArgumentOutOfRangeException(string paramName, string message)
	    {
	        return new ArgumentOutOfRangeException(paramName, message);
	    }

        /// <summary>
        /// Creates and returns a new <see cref="ArgumentEmptyException"/> initialized to the provided parameters.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <returns>A new instance of <see cref="ArgumentEmptyException"/> initialized to the provided parameters.</returns>
	    public static ArgumentEmptyException CreateArgumentEmptyException(string paramName, string message)
	    {
	        return new ArgumentEmptyException(paramName, message);
	    }

        /// <summary>
        /// Creates and returns a new <see cref="ArgumentNullException"/> initialized to the provided parameters.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <returns>A new instance of <see cref="ArgumentNullException"/> initialized to the provided parameters.</returns>
	    public static ArgumentNullException CreateArgumentNullException(string paramName, string message)
	    {
	        return new ArgumentNullException(paramName, message);
	    }

        /// <summary>
        /// Creates and returns a new <see cref="ArgumentException"/> initialized to the provided parameters.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <returns>A new instance of <see cref="ArgumentException"/> initialized to the provided parameters.</returns>
	    public static ArgumentException CreateArgumentException(string paramName, string message)
	    {
            return CreateArgumentException(paramName, message, null);
	    }

        /// <summary>
        /// Creates and returns a new <see cref="ArgumentException"/> initialized to the provided parameters.
        /// </summary>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="innerException">The exception that is the cause of the current exception. If the <paramref name="innerException"/> parameter is not a null reference, the current exception is raised in a <c>catch</c> block that handles the inner exception.</param>
        /// <returns>A new instance of <see cref="ArgumentException"/> initialized to the provided parameters.</returns>
        public static ArgumentException CreateArgumentException(string paramName, string message, Exception innerException)
        {
            return new ArgumentException(message, paramName, innerException);
        }

        /// <summary>
        /// Creates and returns a new <see cref="TypeArgumentException"/> initialized to the provided parameters.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <returns>A new instance of <see cref="TypeArgumentException"/> initialized to the provided parameters.</returns>
        public static TypeArgumentException CreateTypeArgumentException(string typeParamName, string message)
	    {
	        return new TypeArgumentException(typeParamName, message);
	    }

        /// <summary>
        /// Creates and returns a new <see cref="TypeArgumentOutOfRangeException"/> initialized to the provided parameters.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <returns>A new instance of <see cref="TypeArgumentOutOfRangeException"/> initialized to the provided parameters.</returns>
        public static TypeArgumentOutOfRangeException CreateTypeArgumentOutOfRangeException(string typeParamName, string message)
	    {
	        return new TypeArgumentOutOfRangeException(typeParamName, message);
	    }
    }
}