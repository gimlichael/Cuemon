using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Provides a generic way to validate different types of arguments passed to members.
    /// </summary>
    public static class Validator
    {
        /// <summary>
        /// Validates from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>true</c>.
        /// </summary>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIf(Func<bool> condition, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (condition()) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo()).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>true</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIf<TValue>(TValue value, Func<TValue, bool> condition, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (condition(value)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>true</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIf<TValue, T>(TValue value, Func<TValue, T, bool> condition, T arg, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (condition(value, arg)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>true</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIf<TValue, T1, T2>(TValue value, Func<TValue, T1, T2, bool> condition, T1 arg1, T2 arg2, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (condition(value, arg1, arg2)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg1, arg2).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>true</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIf<TValue, T1, T2, T3>(TValue value, Func<TValue, T1, T2, T3, bool> condition, T1 arg1, T2 arg2, T3 arg3, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (condition(value, arg1, arg2, arg3)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg1, arg2, arg3).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>true</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIf<TValue, T1, T2, T3, T4>(TValue value, Func<TValue, T1, T2, T3, T4, bool> condition, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (condition(value, arg1, arg2, arg3, arg4)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg1, arg2, arg3, arg4).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>true</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIf<TValue, T1, T2, T3, T4, T5>(TValue value, Func<TValue, T1, T2, T3, T4, T5, bool> condition, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (condition(value, arg1, arg2, arg3, arg4, arg5)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg1, arg2, arg3, arg4, arg5).Unwrap(); }
        }

        /// <summary>
        /// Validates from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>false</c>.
        /// </summary>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIfNot(Func<bool> condition, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (!condition()) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo()).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>false</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIfNot<TValue>(TValue value, Func<TValue, bool> condition, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (!condition(value)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>false</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T">The type of the parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg">The parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIfNot<TValue, T>(TValue value, Func<TValue, T, bool> condition, T arg, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (!condition(value, arg)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>false</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIfNot<TValue, T1, T2>(TValue value, Func<TValue, T1, T2, bool> condition, T1 arg1, T2 arg2, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (!condition(value, arg1, arg2)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg1, arg2).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>false</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIfNot<TValue, T1, T2, T3>(TValue value, Func<TValue, T1, T2, T3, bool> condition, T1 arg1, T2 arg2, T3 arg3, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (!condition(value, arg1, arg2, arg3)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg1, arg2, arg3).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>false</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIfNot<TValue, T1, T2, T3, T4>(TValue value, Func<TValue, T1, T2, T3, T4, bool> condition, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (!condition(value, arg1, arg2, arg3, arg4)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg1, arg2, arg3, arg4).Unwrap(); }
        }

        /// <summary>
        /// Validates the specified <paramref name="value"/> from the provided <paramref name="condition"/>.
        /// An <paramref name="exception"/> is resolved and thrown if the <paramref name="condition"/> evaluates <c>false</c>.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to evaluate.</typeparam>
        /// <typeparam name="T1">The type of the first parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T2">The type of the second parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T3">The type of the third parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter of the function delegate <paramref name="condition"/>.</typeparam>
        /// <param name="value">The value that will be evaluated by <paramref name="condition"/>.</param>
        /// <param name="condition">The function delegate that determines if an <paramref name="exception"/> is thrown.</param>
        /// <param name="arg1">The first parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg2">The second parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg3">The third parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg4">The fourth parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="arg5">The fifth parameter of the function delegate <paramref name="condition"/>.</param>
        /// <param name="exception">The function delegate that resolves the <see cref="Exception"/> to be thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null -or- <paramref name="exception"/> is null.
        /// </exception>
        public static void ThrowIfNot<TValue, T1, T2, T3, T4, T5>(TValue value, Func<TValue, T1, T2, T3, T4, T5, bool> condition, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Func<string, string, Exception> exception, string paramName, string message)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            if (exception == null) { throw new ArgumentNullException(nameof(exception)); }
            if (!condition(value, arg1, arg2, arg3, arg4, arg5)) { throw ExceptionUtility.Refine(exception(paramName, message), condition.GetMethodInfo(), value, arg1, arg2, arg3, arg4, arg5).Unwrap(); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a number.
        /// </exception>
        public static void ThrowIfNumber(string value, string paramName)
        {
            ThrowIfNumber(value, paramName, NumberStyles.Number);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a number.
        /// </exception>
        public static void ThrowIfNumber(string value, string paramName, NumberStyles styles)
        {
            ThrowIfNumber(value, paramName, styles, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a number.
        /// </exception>
        public static void ThrowIfNumber(string value, string paramName, NumberStyles styles, IFormatProvider provider)
        {
            ThrowIfNumber(value, paramName, "Value cannot be a number.", styles, provider);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a number.
        /// </exception>
        public static void ThrowIfNumber(string value, string paramName, string message)
        {
            ThrowIfNumber(value, paramName, message, NumberStyles.Number);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a number.
        /// </exception>
        public static void ThrowIfNumber(string value, string paramName, string message, NumberStyles styles)
        {
            ThrowIfNumber(value, paramName, message, styles, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a number.
        /// </exception>
        public static void ThrowIfNumber(string value, string paramName, string message, NumberStyles styles, IFormatProvider provider)
        {
            try
            {
                ThrowIf(value, Condition.IsNumeric, styles, provider, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a number.
        /// </exception>
        public static void ThrowIfNotNumber(string value, string paramName)
        {
            ThrowIfNotNumber(value, paramName, NumberStyles.Number);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a number.
        /// </exception>
        public static void ThrowIfNotNumber(string value, string paramName, NumberStyles styles)
        {
            ThrowIfNotNumber(value, paramName, styles, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a number.
        /// </exception>
        public static void ThrowIfNotNumber(string value, string paramName, NumberStyles styles, IFormatProvider provider)
        {
            ThrowIfNotNumber(value, paramName, "Value must be a number.", styles, provider);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a number.
        /// </exception>
        public static void ThrowIfNotNumber(string value, string paramName, string message)
        {
            ThrowIfNotNumber(value, paramName, message, NumberStyles.Number);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a number.
        /// </exception>
        public static void ThrowIfNotNumber(string value, string paramName, string message, NumberStyles styles)
        {
            ThrowIfNotNumber(value, paramName, message, styles, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not a number.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information about <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a number.
        /// </exception>
        public static void ThrowIfNotNumber(string value, string paramName, string message, NumberStyles styles, IFormatProvider provider)
        {
            try
            {
                ThrowIfNot(value, Condition.IsNumeric, styles, provider, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentNullException"/> if the specified <paramref name="value"/> is null.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public static void ThrowIfNull<T>(T value, string paramName)
        {
            ThrowIfNull(value, paramName, "Value cannot be null.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentNullException"/> if the specified <paramref name="value"/> is null.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public static void ThrowIfNull<T>(T value, string paramName, string message)
        {
            try
            {
                ThrowIf(value, Condition.IsNull, ExceptionUtility.CreateArgumentNullException, paramName, message);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="value" /> is <c>true</c>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value" /> must be <c>false</c>.
        /// </exception>
        public static void ThrowIfTrue(bool value, string paramName)
        {
            ThrowIfTrue(value, paramName, "Value must be false.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="value" /> is <c>true</c>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value" /> must be <c>false</c>.
        /// </exception>
        public static void ThrowIfTrue(bool value, string paramName, string message)
        {
            try
            {
                ThrowIf(value, Condition.IsTrue, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="value" /> is <c>false</c>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value" /> must be <c>true</c>.
        /// </exception>
        public static void ThrowIfFalse(bool value, string paramName)
        {
            ThrowIfFalse(value, paramName, "Value must be true.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="value" /> is <c>false</c>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value" /> must be <c>true</c>.
        /// </exception>
        public static void ThrowIfFalse(bool value, string paramName, string message)
        {
            try
            {
                ThrowIf(value, Condition.IsFalse, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentEmptyException"/> if the specified <paramref name="value"/> is empty.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public static void ThrowIfEmpty(string value, string paramName)
        {
            ThrowIfEmpty(value, paramName, "Value cannot be empty.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentEmptyException"/> if the specified <paramref name="value"/> is empty.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public static void ThrowIfEmpty(string value, string paramName, string message)
        {
            try
            {
                ThrowIf(value, Condition.IsEmpty, ExceptionUtility.CreateArgumentEmptyException, paramName, message);
            }
            catch (ArgumentEmptyException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentEmptyException"/> if the specified <paramref name="value"/> consist only of white-space characters.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot consist only of white-space characters.
        /// </exception>
        public static void ThrowIfWhiteSpace(string value, string paramName)
        {
            ThrowIfWhiteSpace(value, paramName, "Value cannot consist only of white-space characters.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> consist only of white-space characters.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot consist only of white-space characters.
        /// </exception>
        public static void ThrowIfWhiteSpace(string value, string paramName, string message)
        {
            try
            {
                ThrowIf(value, Condition.IsWhiteSpace, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentEmptyException"/> if the specified <paramref name="value"/> is respectively null or empty.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public static void ThrowIfNullOrEmpty(string value, string paramName)
        {
            try
            {
                ThrowIfNull(value, paramName);
                ThrowIfEmpty(value, paramName);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentEmptyException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentEmptyException"/> if the specified <paramref name="value"/> is respectively null or empty.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public static void ThrowIfNullOrEmpty(string value, string paramName, string message)
        {
            try
            {
                ThrowIfNull(value, paramName, message);
                ThrowIfEmpty(value, paramName, message);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentEmptyException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/>, <see cref="ArgumentEmptyException"/> or <see cref="ArgumentException"/> if the specified <paramref name="value"/> is respectively null, empty or consist only of white-space characters.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot consist only of white-space characters.
        /// </exception>
        public static void ThrowIfNullOrWhitespace(string value, string paramName)
        {
            try
            {
                ThrowIfNull(value, paramName);
                ThrowIfEmpty(value, paramName);
                ThrowIfWhiteSpace(value, paramName);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentEmptyException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/>, <see cref="ArgumentEmptyException"/> or <see cref="ArgumentException"/> if the specified <paramref name="value"/> is respectively null, empty or consist only of white-space characters.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentEmptyException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot consist only of white-space characters.
        /// </exception>
        public static void ThrowIfNullOrWhitespace(string value, string paramName, string message)
        {
            try
            {
                ThrowIfNull(value, paramName, message);
                ThrowIfEmpty(value, paramName, message);
                ThrowIfWhiteSpace(value, paramName, message);
            }
            catch (ArgumentNullException ex)
            {
                throw ex;
            }
            catch (ArgumentEmptyException ex)
            {
                throw ex;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are of the same instance as the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are not of the same instance.
        /// </exception>
        public static void ThrowIfSame<T>(T x, T y, string paramName)
        {
            ThrowIfSame(x, y, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are of the same instance as the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are of the same instance.
        /// </exception>
        public static void ThrowIfSame<T>(T x, T y, string paramName, string message)
        {
            try
            {
                ThrowIf(x, Condition.AreSame, y, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are not of the same instance as the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are of the same instance.
        /// </exception>
        public static void ThrowIfNotSame<T>(T x, T y, string paramName)
        {
            ThrowIfNotSame(x, y, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are not of the same instance as the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are not of the same instance.
        /// </exception>
        public static void ThrowIfNotSame<T>(T x, T y, string paramName, string message)
        {
            try
            {
                ThrowIf(x, Condition.AreNotSame, y, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are equal to one another.
        /// </exception>
        public static void ThrowIfEqual<T>(T x, T y, string paramName)
        {
            ThrowIfEqual(x, y, EqualityComparer<T>.Default, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are equal to one another.
        /// </exception>
        public static void ThrowIfEqual<T>(T x, T y, IEqualityComparer<T> comparer, string paramName)
        {
            ThrowIfEqual(x, y, comparer, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are equal to one another.
        /// </exception>
        public static void ThrowIfEqual<T>(T x, T y, string paramName, string message)
        {
            ThrowIfEqual(x, y, EqualityComparer<T>.Default, paramName, message);
        }


        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are equal to one another.
        /// </exception>
        public static void ThrowIfEqual<T>(T x, T y, IEqualityComparer<T> comparer, string paramName, string message)
        {
            try
            {
                ThrowIf(x, Condition.AreEqual, y, comparer, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are not equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are not equal to one another.
        /// </exception>
        public static void ThrowIfNotEqual<T>(T x, T y, string paramName)
        {
            ThrowIfNotEqual(x, y, EqualityComparer<T>.Default, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are not equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are not equal to one another.
        /// </exception>
        public static void ThrowIfNotEqual<T>(T x, T y, IEqualityComparer<T> comparer, string paramName)
        {
            ThrowIfNotEqual(x, y, comparer, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are not equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are not equal to one another.
        /// </exception>
        public static void ThrowIfNotEqual<T>(T x, T y, string paramName, string message)
        {
            ThrowIfNotEqual(x, y, EqualityComparer<T>.Default, paramName, message);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> object are not equal to the <paramref name="y" /> object.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="x"/> and <paramref name="y"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> and <paramref name="y"/> are not equal to one another.
        /// </exception>
        public static void ThrowIfNotEqual<T>(T x, T y, IEqualityComparer<T> comparer, string paramName, string message)
        {
            try
            {
                ThrowIf(x, Condition.AreNotEqual, y, comparer, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is greater than <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is greater than <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfGreaterThan<T>(T x, T y, string paramName) where T : struct, IConvertible
        {
            ThrowIfGreaterThan(x, y, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is greater than <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is greater than <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfGreaterThan<T>(T x, T y, string paramName, string message) where T : struct, IConvertible
        {
            try
            {
                ThrowIf(x, Condition.IsGreaterThan, y, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is greater than or equal to <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is greater than or equal to <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfGreaterThanOrEqual<T>(T x, T y, string paramName)
            where T : struct, IConvertible
        {
            ThrowIfGreaterThanOrEqual(x, y, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is greater than or equal to <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is greater than or equal to <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfGreaterThanOrEqual<T>(T x, T y, string paramName, string message) where T : struct, IConvertible
        {
            try
            {
                ThrowIf(x, Condition.IsGreaterThanOrEqual, y, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is lower than <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is lower than <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfLowerThan<T>(T x, T y, string paramName) where T : struct, IConvertible
        {
            ThrowIfLowerThan(x, y, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is lower than <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is lower than <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfLowerThan<T>(T x, T y, string paramName, string message) where T : struct, IConvertible
        {
            try
            {
                ThrowIf(x, Condition.IsLowerThan, y, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is lower than or equal to <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is lower than or equal to <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfLowerThanOrEqual<T>(T x, T y, string paramName) where T : struct, IConvertible
        {
            ThrowIfLowerThanOrEqual(x, y, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException" /> if the specified <paramref name="x" /> is lower than or equal to <paramref name="y" />.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="x" /> is lower than or equal to <paramref name="y"/>.
        /// </exception>
        public static void ThrowIfLowerThanOrEqual<T>(T x, T y, string paramName, string message) where T : struct, IConvertible
        {
            try
            {
                ThrowIf(x, Condition.IsLowerThanOrEqual, y, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is hexadecimal.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> cannot be hexadecimal.
        /// </exception>
        public static void ThrowIfHex(string value, string paramName)
        {
            ThrowIfHex(value, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is hexadecimal.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> cannot be hexadecimal.
        /// </exception>
        public static void ThrowIfHex(string value, string paramName, string message)
        {
            try
            {
                ThrowIf(value, Condition.IsHex, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not hexadecimal.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> must be hexadecimal.
        /// </exception>
        public static void ThrowIfNotHex(string value, string paramName)
        {
            ThrowIfNotHex(value, paramName, "Specified argument was out of the range of valid values.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not hexadecimal.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> must be hexadecimal.
        /// </exception>
        public static void ThrowIfNotHex(string value, string paramName, string message)
        {
            try
            {
                ThrowIfNot(value, Condition.IsHex, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of an email address.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be an email address.
        /// </exception>
        public static void ThrowIfEmailAddress(string value, string paramName)
        {
            ThrowIfEmailAddress(value, paramName, "Value cannot be an email address.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of an email address.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be an email address.
        /// </exception>
        public static void ThrowIfEmailAddress(string value, string paramName, string message)
        {
            try
            {
                ThrowIf(value, Condition.IsEmailAddress, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of an email address.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be an email address.
        /// </exception>
        public static void ThrowIfNotEmailAddress(string value, string paramName)
        {
            ThrowIfNotEmailAddress(value, paramName, "Value must be an email address.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of an email address.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be an email address.
        /// </exception>
        public static void ThrowIfNotEmailAddress(string value, string paramName, string message)
        {
            try
            {
                ThrowIfNot(value, Condition.IsEmailAddress, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a <see cref="Guid"/>.
        /// </exception>
        /// <remarks>
        /// This implementation only evaluates for GUID formats of: <see cref="GuidFormats.DigitFormat"/> | <see cref="GuidFormats.BraceFormat"/> | <see cref="GuidFormats.ParenthesisFormat"/>, eg. 32 digits separated by hyphens; 32 digits separated by hyphens, enclosed in brackets and 32 digits separated by hyphens, enclosed in parentheses.<br/>
        /// The reason not to include <see cref="GuidFormats.NumberFormat"/>, eg. 32 digits is the possible unintended GUID result of a MD5 string representation.
        /// </remarks>
        public static void ThrowIfGuid(string value, string paramName)
        {
            ThrowIfGuid(value, GuidFormats.BraceFormat | GuidFormats.DigitFormat | GuidFormats.ParenthesisFormat, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a <see cref="Guid"/>.
        /// </exception>
        public static void ThrowIfGuid(string value, GuidFormats format, string paramName)
        {
            ThrowIfGuid(value, format, paramName, "Value cannot be a Guid.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a <see cref="Guid"/>.
        /// </exception>
        public static void ThrowIfGuid(string value, GuidFormats format, string paramName, string message)
        {
            try
            {
                ThrowIf(value, Condition.IsGuid, format, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a <see cref="Guid"/>.
        /// </exception>
        /// <remarks>
        /// This implementation only evaluates for GUID formats of: <see cref="GuidFormats.DigitFormat"/> | <see cref="GuidFormats.BraceFormat"/> | <see cref="GuidFormats.ParenthesisFormat"/>, eg. 32 digits separated by hyphens; 32 digits separated by hyphens, enclosed in brackets and 32 digits separated by hyphens, enclosed in parentheses.<br/>
        /// The reason not to include <see cref="GuidFormats.NumberFormat"/>, eg. 32 digits is the possible unintended GUID result of a MD5 string representation.
        /// </remarks>
        public static void ThrowIfNotGuid(string value, string paramName)
        {
            ThrowIfNotGuid(value, GuidFormats.BraceFormat | GuidFormats.DigitFormat | GuidFormats.ParenthesisFormat, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a <see cref="Guid"/>.
        /// </exception>
        public static void ThrowIfNotGuid(string value, GuidFormats format, string paramName)
        {
            ThrowIfNotGuid(value, format, paramName, "Value must be a Guid.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of a <see cref="Guid"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="format">A bitmask comprised of one or more <see cref="GuidFormats"/> that specify how the GUID parsing is conducted.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a <see cref="Guid"/>.
        /// </exception>
        public static void ThrowIfNotGuid(string value, GuidFormats format, string paramName, string message)
        {
            try
            {
                ThrowIfNot(value, Condition.IsGuid, format, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of an <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be an <see cref="Uri"/>.
        /// </exception>
        public static void ThrowIfUri(string value, string paramName)
        {
            ThrowIfUri(value, UriKind.Absolute, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of an <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be an <see cref="Uri"/>.
        /// </exception>
        public static void ThrowIfUri(string value, UriKind uriKind, string paramName, string message = "Value cannot be an URI.")
        {
            try
            {
                ThrowIf(value, UriUtility.IsUri, uriKind, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of an <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be an <see cref="Uri"/>.
        /// </exception>
        public static void ThrowIfNotUri(string value, string paramName)
        {
            ThrowIfNotUri(value, UriKind.Absolute, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of an <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be an <see cref="Uri"/>.
        /// </exception>
        public static void ThrowIfNotUri(string value, UriKind uriKind, string paramName, string message = "Value must be an URI.")
        {
            try
            {
                ThrowIfNot(value, UriUtility.IsUri, uriKind, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfContainsType(Type value, string paramName, params Type[] types)
        {
            ThrowIfContainsType(value, paramName, "Specified argument was out of the range of valid values.", types);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfContainsType(Type value, string paramName, string message, params Type[] types)
        {
            ThrowIfNull(value, nameof(value));
            ThrowIfNull(types, nameof(types));
            try
            {
                ThrowIf(value, TypeUtility.ContainsType, types, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfNotContainsType(Type value, string paramName, params Type[] types)
        {
            ThrowIfNotContainsType(value, paramName, "Specified argument was out of the range of valid values.", types);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfNotContainsType(Type value, string paramName, string message, params Type[] types)
        {
            ThrowIfNull(value, nameof(value));
            ThrowIfNull(types, nameof(types));
            try
            {
                ThrowIfNot(value, TypeUtility.ContainsType, types, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfContainsType(object value, string paramName, params Type[] types)
        {
            ThrowIfContainsType(value, paramName, "Specified argument was out of the range of valid values.", types);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfContainsType(object value, string paramName, string message, params Type[] types)
        {
            ThrowIfNull(value, nameof(value));
            ThrowIfNull(types, nameof(types));
            try
            {
                ThrowIf(value, TypeUtility.ContainsType, types, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfNotContainsType(object value, string paramName, params Type[] types)
        {
            ThrowIfNotContainsType(value, paramName, "Specified argument was out of the range of valid values.", types);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfNotContainsType(object value, string paramName, string message, params Type[] types)
        {
            ThrowIfNull(value, nameof(value));
            ThrowIfNull(types, nameof(types));
            try
            {
                ThrowIfNot(value, TypeUtility.ContainsType, types, ExceptionUtility.CreateArgumentOutOfRangeException, paramName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfContainsType<T>(string typeParamName, params Type[] types)
        {
            ThrowIfContainsType<T>(typeParamName, "Specified type argument was out of the range of valid values.", types);
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfContainsType<T>(string typeParamName, string message, params Type[] types)
        {
            ThrowIfNull(types, nameof(types));
            try
            {
                ThrowIf(typeof(T), TypeUtility.ContainsType, types, ExceptionUtility.CreateTypeArgumentOutOfRangeException, typeParamName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfNotContainsType<T>(string typeParamName, params Type[] types)
        {
            ThrowIfNotContainsType<T>(typeParamName, "Specified type argument was out of the range of valid values.", types);
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        public static void ThrowIfNotContainsType<T>(string typeParamName, string message, params Type[] types)
        {
            ThrowIfNull(types, nameof(types));
            try
            {
                ThrowIfNot(typeof(T), TypeUtility.ContainsType, types, ExceptionUtility.CreateTypeArgumentException, typeParamName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnum<TEnum>(string value, string paramName) where TEnum : struct, IConvertible
        {
            ThrowIfEnum<TEnum>(value, true, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnum<TEnum>(string value, bool ignoreCase, string paramName) where TEnum : struct, IConvertible
        {
            ThrowIfEnum<TEnum>(value, ignoreCase, paramName, "Value represents an enumeration.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnum<TEnum>(string value, bool ignoreCase, string paramName, string message) where TEnum : struct, IConvertible
        {
            try
            {
                ThrowIf(value, EnumUtility.IsStringOf<TEnum>, ignoreCase, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnum<TEnum>(string value, string paramName) where TEnum : struct, IConvertible
        {
            ThrowIfNotEnum<TEnum>(value, true, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnum<TEnum>(string value, bool ignoreCase, string paramName) where TEnum : struct, IConvertible
        {
            ThrowIfNotEnum<TEnum>(value, ignoreCase, paramName, "Value does not represents an enumeration.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type of the enumeration.</typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore case; <c>false</c> to regard case.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnum<TEnum>(string value, bool ignoreCase, string paramName, string message) where TEnum : struct, IConvertible
        {
            try
            {
                ThrowIfNot(value, EnumUtility.IsStringOf<TEnum>, ignoreCase, ExceptionUtility.CreateArgumentException, paramName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentException"/> if the specified <typeparamref name="TEnum"/> represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type to check is an enumeration.</typeparam>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnumType<TEnum>(string typeParamName)
        {
            ThrowIfEnumType<TEnum>(typeParamName, "Value represents an enumeration.");
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentException"/> if the specified <typeparamref name="TEnum"/> represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type to check is an enumeration.</typeparam>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnumType<TEnum>(string typeParamName, string message)
        {
            try
            {
                ThrowIf(DelegateUtility.DynamicWrap(typeof(TEnum).GetTypeInfo().IsEnum), ExceptionUtility.CreateTypeArgumentException, typeParamName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentException"/> if the specified <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type to check is not an enumeration.</typeparam>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnumType<TEnum>(string typeParamName)
        {
            ThrowIfNotEnumType<TEnum>(typeParamName, "Value does not represents an enumeration.");
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentException"/> if the specified <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </summary>
        /// <typeparam name="TEnum">The type to check is not an enumeration.</typeparam>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="TypeArgumentException">
        /// <typeparamref name="TEnum"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnumType<TEnum>(string typeParamName, string message)
        {
            try
            {
                ThrowIfNot(DelegateUtility.DynamicWrap(typeof(TEnum).GetTypeInfo().IsEnum), ExceptionUtility.CreateTypeArgumentException, typeParamName, message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}