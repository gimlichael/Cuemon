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
        /// Validates and throws an <see cref="ArgumentException"/> (or a derived counterpart) from the specified delegate <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">The delegate that evaluates, creates and ultimately throws an <see cref="ArgumentException"/> (or a derived counterpart) from within a given scenario.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null.
        /// </exception>
        public static void ThrowIf(Action<ExceptionCondition<ArgumentException>> condition)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            condition.CreateInstance();
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
                ThrowIf(c => c.IsTrue(() => Condition.IsNumeric(value, styles, provider)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
                ThrowIf(c => c.IsFalse(() => Condition.IsNumeric(value, styles, provider)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
                ThrowIf(c => c.IsTrue(() => Condition.IsNull(value)).Create(() => new ArgumentNullException(paramName, message)).TryThrow());
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
                ThrowIf(c => c.IsTrue(() => Condition.IsTrue(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
                ThrowIf(c => c.IsFalse(() => Condition.IsFalse(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is empty.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public static void ThrowIfEmpty(string value, string paramName, string message)
        {
            try
            {
                ThrowIf(c => c.IsTrue(() => Condition.IsEmpty(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> consist only of white-space characters.
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
                ThrowIf(c => c.IsTrue(() => Condition.IsWhiteSpace(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if the specified <paramref name="value"/> is respectively null or empty.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public static void ThrowIfNullOrEmpty(string value, string paramName)
        {
            try
            {
                ThrowIfNull(value, paramName);
                ThrowIfEmpty(value, paramName);
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if the specified <paramref name="value"/> is respectively null or empty.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public static void ThrowIfNullOrEmpty(string value, string paramName, string message)
        {
            try
            {
                ThrowIfNull(value, paramName, message);
                ThrowIfEmpty(value, paramName, message);
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if the specified <paramref name="value"/> is respectively null, empty or consist only of white-space characters.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public static void ThrowIfNullOrWhitespace(string value, string paramName)
        {
            try
            {
                ThrowIfNull(value, paramName);
                ThrowIfEmpty(value, paramName);
                ThrowIfWhiteSpace(value, paramName);
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if the specified <paramref name="value"/> is respectively null, empty or consist only of white-space characters.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty or consist only of white-space characters.
        /// </exception>
        public static void ThrowIfNullOrWhitespace(string value, string paramName, string message)
        {
            try
            {
                ThrowIfNull(value, paramName, message);
                ThrowIfEmpty(value, paramName, message);
                ThrowIfWhiteSpace(value, paramName, message);
            }
            catch (Exception ex) when (ex is ArgumentException)
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
        /// <paramref name="x" /> and <paramref name="y"/> are of the same instance.
        /// </exception>
        public static void ThrowIfSame<T>(T x, T y, string paramName)
        {
            ThrowIfSame(x, y, paramName, $"Specified arguments {nameof(x)} and {nameof(y)} are of the same instance.");
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
                ThrowIf(c => c.IsTrue(() => Condition.AreSame(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, y, message)).TryThrow());
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
        /// <paramref name="x" /> and <paramref name="y"/> are not of the same instance.
        /// </exception>
        public static void ThrowIfNotSame<T>(T x, T y, string paramName)
        {
            ThrowIfNotSame(x, y, paramName, $"Specified arguments {nameof(x)} and {nameof(y)} are not of the same instance.");
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
                ThrowIf(c => c.IsTrue(() => Condition.AreNotSame(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, y, message)).TryThrow());
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
            ThrowIfEqual(x, y, comparer, paramName, $"Specified arguments {nameof(x)} and {nameof(y)} are equal to one another.");
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
                ThrowIf(c => c.IsTrue(() => Condition.AreEqual(x, y, comparer)).Create(() => new ArgumentOutOfRangeException(paramName, y, message)).TryThrow());
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
            ThrowIfNotEqual(x, y, comparer, paramName, $"Specified arguments {nameof(x)} and {nameof(y)} are not equal to one another.");
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
                ThrowIf(c => c.IsTrue(() => Condition.AreNotEqual(x, y, comparer)).Create(() => new ArgumentOutOfRangeException(paramName, y, message)).TryThrow());
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
            ThrowIfGreaterThan(x, y, paramName, $"Specified arguments {nameof(x)} is greater than {nameof(y)}.");
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
                ThrowIf(c => c.IsTrue(() => Condition.IsGreaterThan(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, y, message)).TryThrow());
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
            ThrowIfGreaterThanOrEqual(x, y, paramName, $"Specified arguments {nameof(x)} is greater than or equal to {nameof(y)}.");
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
                ThrowIf(c => c.IsTrue(() => Condition.IsGreaterThanOrEqual(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, y, message)).TryThrow());
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
            ThrowIfLowerThan(x, y, paramName, $"Specified arguments {nameof(x)} is lower than {nameof(y)}.");
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
                ThrowIf(c => c.IsTrue(() => Condition.IsLowerThan(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, y, message)).TryThrow());
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
            ThrowIfLowerThanOrEqual(x, y, paramName, $"Specified arguments {nameof(x)} is lower than or equal to {nameof(y)}.");
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
                ThrowIf(c => c.IsTrue(() => Condition.IsLowerThanOrEqual(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, y, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is hexadecimal.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be hexadecimal.
        /// </exception>
        public static void ThrowIfHex(string value, string paramName)
        {
            ThrowIfHex(value, paramName, "Specified argument cannot be hexadecimal.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is hexadecimal.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be hexadecimal.
        /// </exception>
        public static void ThrowIfHex(string value, string paramName, string message)
        {
            try
            {
                ThrowIf(c => c.IsTrue(() => Condition.IsHex(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not hexadecimal.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be hexadecimal.
        /// </exception>
        public static void ThrowIfNotHex(string value, string paramName)
        {
            ThrowIfNotHex(value, paramName, "Value must be hexadecimal.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> is not hexadecimal.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be hexadecimal.
        /// </exception>
        public static void ThrowIfNotHex(string value, string paramName, string message)
        {
            try
            {
                ThrowIf(c => c.IsFalse(() => Condition.IsHex(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
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
                ThrowIf(c => c.IsTrue(() => Condition.IsEmailAddress(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
                ThrowIf(c => c.IsFalse(() => Condition.IsEmailAddress(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
                ThrowIf(c => c.IsTrue(() => Condition.IsGuid(value, format)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
                ThrowIf(c => c.IsFalse(() => Condition.IsGuid(value, format)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
                ThrowIf(c => c.IsTrue(() => UriUtility.IsUri(value, uriKind)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
                ThrowIf(c => c.IsFalse(() => UriUtility.IsUri(value, uriKind)).Create(() => new ArgumentException(message, paramName)).TryThrow());
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
            ThrowIfContainsType(value, paramName, $"Specified argument is contained within at least one of {nameof(types)}.", types);
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
                ThrowIf(c => c.IsTrue(() => TypeUtility.ContainsType(value, types)).Create(() => new ArgumentOutOfRangeException(paramName, types, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
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
            ThrowIfNotContainsType(value, paramName, $"Specified argument is not contained within at least one of {nameof(types)}.", types);
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
                ThrowIf(c => c.IsFalse(() => TypeUtility.ContainsType(value, types)).Create(() => new ArgumentOutOfRangeException(paramName, types, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
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
            ThrowIfContainsType(value, paramName, $"Specified argument is contained within at least one of {nameof(types)}.", types);
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
                ThrowIf(c => c.IsTrue(() => TypeUtility.ContainsType(value, types)).Create(() => new ArgumentOutOfRangeException(paramName, types, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
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
            ThrowIfNotContainsType(value, paramName, $"Specified argument is not contained within at least one of {nameof(types)}.", types);
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
                ThrowIf(c => c.IsFalse(() => TypeUtility.ContainsType(value, types)).Create(() => new ArgumentOutOfRangeException(paramName, types, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
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
            ThrowIfContainsType<T>(typeParamName, $"Specified argument is contained within at least one of {nameof(types)}.", types);
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
                ThrowIf(c => c.IsTrue(() => TypeUtility.ContainsType(typeof(T), types)).Create(() => new TypeArgumentOutOfRangeException(typeParamName, types, message)).TryThrow());
            }
            catch (TypeArgumentOutOfRangeException ex)
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
            ThrowIfNotContainsType<T>(typeParamName, $"Specified argument is not contained within at least one of {nameof(types)}.", types);
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
                ThrowIf(c => c.IsFalse(() => TypeUtility.ContainsType(typeof(T), types)).Create(() => new TypeArgumentOutOfRangeException(typeParamName, types, message)).TryThrow());
            }
            catch (TypeArgumentOutOfRangeException ex)
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
                ThrowIf(c => c.IsTrue(() => EnumUtility.IsStringOf<TEnum>(value, ignoreCase)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
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
                ThrowIf(c => c.IsFalse(() => EnumUtility.IsStringOf<TEnum>(value, ignoreCase)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
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
                ThrowIf(c => c.IsTrue(() => typeof(TEnum).GetTypeInfo().IsEnum).Create(() => new TypeArgumentException(typeParamName, message)).TryThrow());
            }
            catch (TypeArgumentException ex)
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
                ThrowIf(c => c.IsFalse(() => typeof(TEnum).GetTypeInfo().IsEnum).Create(() => new TypeArgumentException(typeParamName, message)).TryThrow());
            }
            catch (TypeArgumentException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>.
        /// </summary>
        /// <param name="definite">The value that specifies valid characters.</param>
        /// <param name="arbitrary">The value to distinctively compare with <paramref name="definite"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>.
        /// </exception>
        public static void ThrowIfDistinctDifference(string definite, string arbitrary, string paramName)
        {
            ThrowIfDistinctDifference(definite, arbitrary, paramName, $"Specified arguments has a distinct difference between {nameof(arbitrary)} and {nameof(definite)}.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>.
        /// </summary>
        /// <param name="definite">The value that specifies valid characters.</param>
        /// <param name="arbitrary">The value to distinctively compare with <paramref name="definite"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>.
        /// </exception>
        public static void ThrowIfDistinctDifference(string definite, string arbitrary, string paramName, string message)
        {
            try
            {
                ThrowIf(c => c.IsTrue((out string invalidCharacters) => StringUtility.ParseDistinctDifference(definite, arbitrary, out invalidCharacters)).Create(invalidCharacters => new ArgumentOutOfRangeException(paramName, invalidCharacters, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is not a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>.
        /// </summary>
        /// <param name="definite">The value that specifies valid characters.</param>
        /// <param name="arbitrary">The value to distinctively compare with <paramref name="definite"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is not a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>.
        /// </exception>
        public static void ThrowIfNotDistinctDifference(string definite, string arbitrary, string paramName)
        {
            ThrowIfNotDistinctDifference(definite, arbitrary, paramName, $"Specified arguments does not have a distinct difference between {nameof(arbitrary)} and {nameof(definite)}.");
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is not a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>.
        /// </summary>
        /// <param name="definite">The value that specifies valid characters.</param>
        /// <param name="arbitrary">The value to distinctively compare with <paramref name="definite"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is not a distinct difference between <paramref name="arbitrary"/> and <paramref name="definite"/>.
        /// </exception>
        public static void ThrowIfNotDistinctDifference(string definite, string arbitrary, string paramName, string message)
        {
            try
            {
                ThrowIf(c => c.IsFalse(() => StringUtility.ParseDistinctDifference(definite, arbitrary, out _)).Create(() => new ArgumentOutOfRangeException(paramName, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }
    }
}