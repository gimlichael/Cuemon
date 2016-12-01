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
        /// <summary>
        /// Refines the specified <paramref name="exception"/> with valuable meta information extracted from the associated <paramref name="method"/>.
        /// </summary>
        /// <typeparam name="T">The type of the exception.</typeparam>
        /// <param name="exception">The exception that needs to be thrown.</param>
        /// <param name="method">The method to extract valuable meta information from.</param>
        /// <returns>The specified <paramref name="exception"/> refined with valuable meta information.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="exception"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        public static T Refine<T>(T exception, MethodBase method) where T : Exception
        {
            return Refine(exception, method, new object[] { });
        }

        /// <summary>
        /// Refines the specified <paramref name="exception"/> with valuable meta information extracted from the associated <paramref name="method"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <typeparam name="T">The type of the exception.</typeparam>
        /// <param name="exception">The exception that needs to be thrown.</param>
        /// <param name="method">The method to extract valuable meta information from.</param>
        /// <param name="parameters">The optional parameters to accompany <paramref name="method"/>.</param>
        /// <returns>The specified <paramref name="exception"/> refined with valuable meta information.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="exception"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        public static T Refine<T>(T exception, MethodBase method, params object[] parameters) where T : Exception
        {
            Validator.ThrowIfNull(method, nameof(method));
            return Refine(exception, MethodDescriptor.Create(method), parameters);
        }

        /// <summary>
        /// Refines the specified <paramref name="exception"/> with valuable meta information extracted from the associated <paramref name="method"/> and <paramref name="parameters"/>.
        /// </summary>
        /// <typeparam name="T">The type of the exception.</typeparam>
        /// <param name="exception">The exception that needs to be thrown.</param>
        /// <param name="method">The method signature containing valuable meta information.</param>
        /// <param name="parameters">The optional parameters to accompany <paramref name="method"/>.</param>
        /// <returns>The specified <paramref name="exception"/> refined with valuable meta information.</returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="exception"/> is null - or - <paramref name="method"/> is null.
        /// </exception>
        public static T Refine<T>(T exception, MethodDescriptor method, params object[] parameters) where T : Exception
        {
            Validator.ThrowIfNull(exception, nameof(exception));
            Validator.ThrowIfNull(method, nameof(method));

            exception.Source = method.ToString();
            if (method.HasParameters)
            {
                foreach (KeyValuePair<string, object> item in method.MergeParameters(parameters))
                {
                    string key = item.Key;
                    if (!exception.Data.Contains(key))
                    {
                        exception.Data.Add(key, StringConverter.FromObject(item.Value));
                    }
                }
            }
            return exception;
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
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }

            Type resultType = typeof(TResult);
            Type sourceType = exception.GetType();
            if (TypeUtility.ContainsType(sourceType, resultType)) { return exception as TResult; }
            Type innerSourceType = exception.InnerException.GetType();
            if (TypeUtility.ContainsType(innerSourceType, resultType)) { return exception.InnerException as TResult; }
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
            var matches = EnumerableUtility.FindAll(exceptions, e => e is TResult);
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
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
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
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (exceptionType == null) { throw new ArgumentNullException(nameof(exceptionType)); }
            PropertyInfo innerExceptionsProperty = ReflectionUtility.GetProperty(exceptionType, "InnerExceptions");
            if (innerExceptionsProperty != null) { return innerExceptionsProperty.GetValue(exception, null) as IEnumerable<Exception>; }
            return HierarchyUtility.WhileSourceTraversalIsNotNull(exception, FlattenCallback);
        }

        private static Exception FlattenCallback(Exception source)
        {
            return source.InnerException;
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