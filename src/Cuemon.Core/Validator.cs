using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Cuemon.Collections.Generic;
using Cuemon.Configuration;
using System.Runtime.CompilerServices;
using Cuemon.Text;

namespace Cuemon
{
    /// <summary>
    /// Provides a generic way to validate different types of arguments passed to members.
    /// </summary>
    public sealed class Validator
    {
        private static readonly Validator ExtendedValidator = new();

        /// <summary>
        /// Gets the singleton instance of the Validator functionality allowing for extensions methods like: <c>Validator.ThrowIf.InvalidJsonDocument()</c>.
        /// </summary>
        /// <value>The singleton instance of the Validator functionality.</value>
        public static Validator ThrowIf { get; } = ExtendedValidator;

        /// <summary>
        /// Provides a convenient way to validate a parameter while returning the specified <paramref name="value"/> unaltered.
        /// </summary>
        /// <typeparam name="T">The type of the object to evaluate.</typeparam>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="validator">The delegate that must throw an <see cref="Exception"/> if the specified <paramref name="value"/> is not valid.</param>
        /// <returns>The specified <paramref name="value"/> unaltered.</returns>
        public static T CheckParameter<T>(T value, Action validator)
        {
            ThrowIfNull(validator, nameof(validator));
            validator();
            return value;
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="setup"/> results in an instance of invalid <paramref name="options"/>.
        /// </summary>
        /// <typeparam name="TOptions">The type of the object implementing the <seealso cref="IValidatableParameterObject"/> interface.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <paramref name="options"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="options">The default parameter-less constructed instance of <typeparamref name="TOptions"/> configured with <paramref name="setup"/> delegate.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance of <paramref name="options"/> in a valid state.
        /// </exception>
        public static void ThrowIfInvalidConfigurator<TOptions>(Action<TOptions> setup, string paramName, out TOptions options) where TOptions : class, IValidatableParameterObject, new()
        {
            ThrowIfInvalidConfigurator(setup, paramName, "Delegate must configure the public read-write properties to be in a valid state.", out options);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="setup"/> results in an instance of invalid <paramref name="options"/>.
        /// </summary>
        /// <typeparam name="TOptions">The type of the object implementing the <seealso cref="IValidatableParameterObject"/> interface.</typeparam>
        /// <param name="setup">The delegate that will configure the public read-write properties of <paramref name="options"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="options">The default parameter-less constructed instance of <typeparamref name="TOptions"/> configured with <paramref name="setup"/> delegate.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="setup"/> failed to configure an instance <paramref name="options"/> in a valid state.
        /// </exception>
        public static void ThrowIfInvalidConfigurator<TOptions>(Action<TOptions> setup, string paramName, string message, out TOptions options) where TOptions : class, IValidatableParameterObject, new()
        {
            options = Patterns.Configure(setup);
            ThrowIfInvalidOptions(options, paramName, message);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="options"/> are not in a valid state.
        /// </summary>
        /// <typeparam name="TOptions">The type of the object implementing the <seealso cref="IValidatableParameterObject"/> interface.</typeparam>
        /// <param name="options">The configured options to validate.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="options"/> are not in a valid state.
        /// </exception>
        /// <remarks><paramref name="message"/> will have the name of the <typeparamref name="TOptions"/> if possible; otherwise Options.</remarks>
        public static void ThrowIfInvalidOptions<TOptions>(TOptions options, string paramName, string message = "{0} are not in a valid state.") where TOptions : class, IValidatableParameterObject, new()
        {
            ThrowIfNull(options, nameof(options));
            try
            {
                options.ValidateOptions();
            }
            catch (Exception e)
            {
                if (message?.Equals("{0} are not in a valid state.", StringComparison.InvariantCulture) ?? false) { message = string.Format(message, Patterns.InvokeOrDefault(() => typeof(TOptions).Name, "Options")); }
                throw ExceptionInsights.Embed(new ArgumentException(message, paramName, e), MethodBase.GetCurrentMethod(), Arguments.ToArray(options, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="InvalidOperationException" /> if the specified <paramref name="predicate" /> is <c>true</c>.
        /// </summary>
        /// <param name="predicate">The value that determines if an <see cref="InvalidOperationException"/> is thrown.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="expression">The <paramref name="predicate"/> expressed as a string.</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="predicate" /> is <c>true</c>.
        /// </exception>
        public static void ThrowIfObjectInDistress(bool predicate, string message = "Operation is not valid due to the current state of the object.", [CallerArgumentExpression("predicate")] string expression = null)
        {
            ThrowIfObjectInDistress(() => predicate, message, expression);
        }

        /// <summary>
        /// Validates and throws an <see cref="InvalidOperationException" /> if the specified <paramref name="predicate" /> returns <c>true</c>.
        /// </summary>
        /// <param name="predicate">The function delegate that determines if an <see cref="InvalidOperationException"/> is thrown.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="expression">The <paramref name="predicate"/> expressed as a string.</param>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="predicate" /> returned <c>true</c>.
        /// </exception>
        public static void ThrowIfObjectInDistress(Func<bool> predicate, string message = "Operation is not valid due to the current state of the object.", [CallerArgumentExpression("predicate")] string expression = null)
        {
            if (predicate?.Invoke() ?? false) { throw new InvalidOperationException($"{message} (Expression '{expression}')"); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> (or a derived counterpart) from the specified delegate <paramref name="condition"/>.
        /// </summary>
        /// <param name="condition">The delegate that evaluates, creates and ultimately throws an <see cref="ArgumentException"/> (or a derived counterpart) from within a given scenario.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="condition"/> is null.
        /// </exception>
        public void ThrowWhenCondition(Action<ExceptionCondition<ArgumentException>> condition)
        {
            ThrowWhen(condition);
        }

        internal static void ThrowWhen(Action<ExceptionCondition<ArgumentException>> condition)
        {
            if (condition == null) { throw new ArgumentNullException(nameof(condition)); }
            Patterns.Configure(condition);
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
        public static void ThrowIfNumber(string value, string paramName, NumberStyles styles = NumberStyles.Number)
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
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a number.
        /// </exception>
        public static void ThrowIfNumber(string value, string paramName, string message, NumberStyles styles = NumberStyles.Number)
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
                ThrowWhen(c => c.IsTrue(() => Condition.IsNumeric(value, styles, provider)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message, styles, provider));
            }
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
        public static void ThrowIfNotNumber(string value, string paramName, NumberStyles styles = NumberStyles.Number)
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
        /// <param name="styles">A bitwise combination of <see cref="NumberStyles"/> values that indicates the permitted format of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a number.
        /// </exception>
        public static void ThrowIfNotNumber(string value, string paramName, string message, NumberStyles styles = NumberStyles.Number)
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
                ThrowWhen(c => c.IsFalse(() => Condition.IsNumeric(value, styles, provider)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message, styles, provider));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentNullException"/> if the specified <paramref name="decorator"/> is null.
        /// </summary>
        /// <typeparam name="T">The type of the inner object denoted by <paramref name="decorator"/>.</typeparam>
        /// <param name="decorator">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="inner">The inner object of <paramref name="decorator"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null - or -
        /// <see cref="P:IDecorator.Inner"/> property of <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static void ThrowIfNull<T>(IDecorator<T> decorator, string paramName, out T inner)
        {
            ThrowIfNull(decorator, paramName, "Decorator or Inner cannot be null.", out inner);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentNullException"/> if the specified <paramref name="decorator"/> is null.
        /// </summary>
        /// <typeparam name="T">The type of the inner object denoted by <paramref name="decorator"/>.</typeparam>
        /// <param name="decorator">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="inner">The inner object of <paramref name="decorator"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="decorator"/> cannot be null - or -
        /// <see cref="P:IDecorator.Inner"/> property of <paramref name="decorator"/> cannot be null.
        /// </exception>
        public static void ThrowIfNull<T>(IDecorator<T> decorator, string paramName, string message, out T inner)
        {
            ThrowIfNull(decorator, paramName, message);
            ThrowIfNull(decorator.Inner, nameof(decorator.Inner), message);
            inner = decorator.Inner;
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
        public static void ThrowIfNull<T>(T value, [CallerArgumentExpression("value")] string paramName = null, string message = "Value cannot be null.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsNull(value)).Create(() => new ArgumentNullException(paramName, message)).TryThrow());
            }
            catch (ArgumentNullException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="predicate" /> returns <c>false</c>.
        /// </summary>
        /// <param name="predicate">The function delegate that determines if an <see cref="ArgumentException"/> is thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="predicate" /> returned <c>false</c>.
        /// </exception>
        public static void ThrowIfFalse(Func<bool> predicate, string paramName, string message)
        {
            try
            {
                ThrowWhen(c => c.IsFalse(predicate).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(predicate, paramName, message));
            }
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
        public static void ThrowIfFalse(bool value, string paramName, string message = "Value must be true.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsFalse(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException" /> if the specified <paramref name="predicate" /> returns <c>true</c>.
        /// </summary>
        /// <param name="predicate">The function delegate that determines if an <see cref="ArgumentException"/> is thrown.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="predicate" /> returned <c>true</c>.
        /// </exception>
        public static void ThrowIfTrue(Func<bool> predicate, string paramName, string message)
        {
            try
            {
                ThrowWhen(c => c.IsTrue(predicate).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(predicate, paramName, message));
            }
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
        public static void ThrowIfTrue(bool value, string paramName, string message = "Value must be false.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsTrue(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has no elements.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> contains no elements.
        /// </exception>
        public static void ThrowIfSequenceEmpty<T>(IEnumerable<T> value, string paramName, string message = "Value contains no elements.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(value.Any).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws either an <see cref="ArgumentNullException"/> or <see cref="ArgumentException"/> if the specified <paramref name="value"/> is respectively null or has no elements.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> contains no elements.
        /// </exception>
        public static void ThrowIfSequenceNullOrEmpty<T>(IEnumerable<T> value, string paramName, string message = "Value is either null or contains no elements.")
        {
            try
            {
                ThrowIfNull(value, paramName, message);
                ThrowIfSequenceEmpty(value, paramName, message);
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
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
        public static void ThrowIfEmpty(string value, string paramName, string message = "Value cannot be empty.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsEmpty(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
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
        public static void ThrowIfWhiteSpace(string value, string paramName, string message = "Value cannot consist only of white-space characters.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsWhiteSpace(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
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
        public static void ThrowIfNullOrEmpty(string value, [CallerArgumentExpression("value")] string paramName = null)
        {
            try
            {
                ThrowIfNull(value, paramName);
                ThrowIfEmpty(value, paramName);
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName));
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
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
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
        public static void ThrowIfNullOrWhitespace(string value, [CallerArgumentExpression("value")] string paramName = null)
        {
            try
            {
                ThrowIfNull(value, paramName);
                ThrowIfEmpty(value, paramName);
                ThrowIfWhiteSpace(value, paramName);
            }
            catch (Exception ex) when (ex is ArgumentException)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName));
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
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
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
            ThrowIfSame(x, y, paramName, FormattableString.Invariant($"Specified arguments {nameof(x)} and {nameof(y)} are of the same instance."));
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
                ThrowWhen(c => c.IsTrue(() => Condition.AreSame(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} <==> {y}"), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(x, y, paramName, message));
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
            ThrowIfNotSame(x, y, paramName, FormattableString.Invariant($"Specified arguments {nameof(x)} and {nameof(y)} are not of the same instance."));
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
                ThrowWhen(c => c.IsTrue(() => Condition.AreNotSame(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} <!=> {y}"), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(x, y, paramName, message));
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
            ThrowIfEqual(x, y, comparer, paramName, FormattableString.Invariant($"Specified arguments {nameof(x)} and {nameof(y)} are equal to one another."));
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
                ThrowWhen(c => c.IsTrue(() => Condition.AreEqual(x, y, comparer)).Create(() => new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} == {y}"), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(x, y, comparer, paramName, message));
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
            ThrowIfNotEqual(x, y, comparer, paramName, FormattableString.Invariant($"Specified arguments {nameof(x)} and {nameof(y)} are not equal to one another."));
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
                ThrowWhen(c => c.IsTrue(() => Condition.AreNotEqual(x, y, comparer)).Create(() => new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} != {y}"), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(x, y, comparer, paramName, message));
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
            ThrowIfGreaterThan(x, y, paramName, FormattableString.Invariant($"Specified arguments {nameof(x)} is greater than {nameof(y)}."));
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
                ThrowWhen(c => c.IsTrue(() => Condition.IsGreaterThan(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} > {y}"), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(x, y, paramName, message));
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
        public static void ThrowIfGreaterThanOrEqual<T>(T x, T y, string paramName) where T : struct, IConvertible
        {
            ThrowIfGreaterThanOrEqual(x, y, paramName, FormattableString.Invariant($"Specified arguments {nameof(x)} is greater than or equal to {nameof(y)}."));
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
                ThrowWhen(c => c.IsTrue(() => Condition.IsGreaterThanOrEqual(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} >= {y}"), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(x, y, paramName, message));
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
            ThrowIfLowerThan(x, y, paramName, FormattableString.Invariant($"Specified arguments {nameof(x)} is lower than {nameof(y)}."));
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
                ThrowWhen(c => c.IsTrue(() => Condition.IsLowerThan(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} < {y}"), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(x, y, paramName, message));
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
            ThrowIfLowerThanOrEqual(x, y, paramName, FormattableString.Invariant($"Specified arguments {nameof(x)} is lower than or equal to {nameof(y)}."));
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
                ThrowWhen(c => c.IsTrue(() => Condition.IsLowerThanOrEqual(x, y)).Create(() => new ArgumentOutOfRangeException(paramName, FormattableString.Invariant($"{x} <= {y}"), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(x, y, paramName, message));
            }
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
        public static void ThrowIfHex(string value, string paramName, string message = "Specified argument cannot be hexadecimal.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsHex(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
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
        public static void ThrowIfNotHex(string value, string paramName, string message = "Value must be hexadecimal.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => Condition.IsHex(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
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
        public static void ThrowIfEmailAddress(string value, string paramName, string message = "Value cannot be an email address.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsEmailAddress(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
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
        public static void ThrowIfNotEmailAddress(string value, string paramName, string message = "Value must be an email address.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => Condition.IsEmailAddress(value)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
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
        /// This implementation only evaluates for GUID formats of: <see cref="GuidFormats.D"/> | <see cref="GuidFormats.B"/> | <see cref="GuidFormats.P"/>, eg. 32 digits separated by hyphens; 32 digits separated by hyphens, enclosed in brackets and 32 digits separated by hyphens, enclosed in parentheses.<br/>
        /// The reason not to include <see cref="GuidFormats.N"/>, eg. 32 digits is the possible unintended GUID result of a MD5 string representation.
        /// </remarks>
        public static void ThrowIfGuid(string value, string paramName)
        {
            ThrowIfGuid(value, GuidFormats.B | GuidFormats.D | GuidFormats.P, paramName);
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
        public static void ThrowIfGuid(string value, GuidFormats format, string paramName, string message = "Value cannot be a Guid.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsGuid(value, format)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, format, paramName, message));
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
        /// This implementation only evaluates for GUID formats of: <see cref="GuidFormats.D"/> | <see cref="GuidFormats.B"/> | <see cref="GuidFormats.P"/>, eg. 32 digits separated by hyphens; 32 digits separated by hyphens, enclosed in brackets and 32 digits separated by hyphens, enclosed in parentheses.<br/>
        /// The reason not to include <see cref="GuidFormats.N"/>, eg. 32 digits is the possible unintended GUID result of a MD5 string representation.
        /// </remarks>
        public static void ThrowIfNotGuid(string value, string paramName)
        {
            ThrowIfNotGuid(value, GuidFormats.B | GuidFormats.D | GuidFormats.P, paramName);
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
        public static void ThrowIfNotGuid(string value, GuidFormats format, string paramName, string message = "Value must be a Guid.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => Condition.IsGuid(value, format)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, format, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a <see cref="Uri"/>.
        /// </exception>
        public static void ThrowIfUri(string value, string paramName)
        {
            ThrowIfUri(value, UriKind.Absolute, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> has the format of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be a <see cref="Uri"/>.
        /// </exception>
        public static void ThrowIfUri(string value, UriKind uriKind, string paramName, string message = "Value cannot be a URI.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsUri(value, o => o.Kind = uriKind)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, uriKind, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a <see cref="Uri"/>.
        /// </exception>
        public static void ThrowIfNotUri(string value, string paramName)
        {
            ThrowIfNotUri(value, UriKind.Absolute, paramName);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not have the format of a <see cref="Uri"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="uriKind">The type of the URI.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> must be a <see cref="Uri"/>.
        /// </exception>
        public static void ThrowIfNotUri(string value, UriKind uriKind, string paramName, string message = "Value must be a URI.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => Condition.IsUri(value, o => o.Kind = uriKind)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, uriKind, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfContainsInterface(Type value, string paramName, params Type[] types)
        {
            ThrowIfContainsInterface(value, paramName, FormattableString.Invariant($"Specified argument is contained within at least one of {nameof(types)}."), types);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfContainsInterface(Type value, string paramName, string message, params Type[] types)
        {
            ThrowIfNull(value, nameof(value));
            ThrowIfNull(types, nameof(types));
            ThrowIfFalse(types.All(it => it.IsInterface), nameof(types), $"At least one of the specified {nameof(types)} is not an interface.");
            try
            {
                ThrowWhen(c => c.IsTrue(() => Decorator.Enclose(value).HasInterface(types)).Create(() => new ArgumentOutOfRangeException(paramName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message, types));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfContainsInterface<T>(string typeParamName, params Type[] types)
        {
            ThrowIfContainsInterface<T>(typeParamName, FormattableString.Invariant($"Specified argument is contained within at least one of {nameof(types)}."), types);
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfContainsInterface<T>(string typeParamName, string message, params Type[] types)
        {
            ThrowIfNull(types, nameof(types));
            ThrowIfFalse(types.All(it => it.IsInterface), nameof(types), $"At least one of the specified {nameof(types)} is not an interface.");
            try
            {
                ThrowWhen(c => c.IsTrue(() => Decorator.Enclose(typeof(T)).HasInterface(types)).Create(() => new TypeArgumentOutOfRangeException(typeParamName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (TypeArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(typeParamName, message, types));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfNotContainsInterface<T>(string typeParamName, params Type[] types)
        {
            ThrowIfNotContainsInterface<T>(typeParamName, FormattableString.Invariant($"Specified argument is not contained within at least one of {nameof(types)}."), types);
        }

        /// <summary>
        /// Validates and throws an <see cref="TypeArgumentOutOfRangeException"/> if the specified <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="typeParamName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <typeparamref name="T"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="TypeArgumentOutOfRangeException">
        /// <typeparamref name="T"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfNotContainsInterface<T>(string typeParamName, string message, params Type[] types)
        {
            ThrowIfNull(types, nameof(types));
            ThrowIfFalse(types.All(it => it.IsInterface), nameof(types), $"At least one of the specified {nameof(types)} is not an interface.");
            try
            {
                ThrowWhen(c => c.IsFalse(() => Decorator.Enclose(typeof(T)).HasInterface(types)).Create(() => new TypeArgumentOutOfRangeException(typeParamName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (TypeArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(typeParamName, message, types));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfNotContainsInterface(Type value, string paramName, params Type[] types)
        {
            ThrowIfNotContainsInterface(value, paramName, FormattableString.Invariant($"Specified argument is not contained within at least one of {nameof(types)}."), types);
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <param name="types">A variable number of <see cref="Type"/> arguments (that must be an interface) to match with the type of <paramref name="value"/>.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null - or - <paramref name="types"/> is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> is not contained within at least one of the specified <paramref name="types"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="types"/> does not satisfy the condition of being an interface.
        /// </exception>
        public static void ThrowIfNotContainsInterface(Type value, string paramName, string message, params Type[] types)
        {
            ThrowIfNull(value, nameof(value));
            ThrowIfNull(types, nameof(types));
            ThrowIfFalse(types.All(it => it.IsInterface), nameof(types), $"At least one of the specified {nameof(types)} is not an interface.");
            try
            {
                ThrowWhen(c => c.IsFalse(() => Decorator.Enclose(value).HasInterface(types)).Create(() => new ArgumentOutOfRangeException(paramName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message, types));
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
            ThrowIfContainsType(value, paramName, FormattableString.Invariant($"Specified argument is contained within at least one of {nameof(types)}."), types);
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
                ThrowWhen(c => c.IsTrue(() => Decorator.Enclose(value.GetType()).HasTypes(types)).Create(() => new ArgumentOutOfRangeException(paramName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message, types));
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
            ThrowIfContainsType(value, paramName, FormattableString.Invariant($"Specified argument is contained within at least one of {nameof(types)}."), types);
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
                ThrowWhen(c => c.IsTrue(() => Decorator.Enclose(value).HasTypes(types)).Create(() => new ArgumentOutOfRangeException(paramName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message, types));
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
            ThrowIfContainsType<T>(typeParamName, FormattableString.Invariant($"Specified argument is contained within at least one of {nameof(types)}."), types);
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
                ThrowWhen(c => c.IsTrue(() => Decorator.Enclose(typeof(T)).HasTypes(types)).Create(() => new TypeArgumentOutOfRangeException(typeParamName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (TypeArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(typeParamName, message, types));
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
            ThrowIfNotContainsType(value, paramName, FormattableString.Invariant($"Specified argument is not contained within at least one of {nameof(types)}."), types);
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
                ThrowWhen(c => c.IsFalse(() => Decorator.Enclose(value).HasTypes(types)).Create(() => new ArgumentOutOfRangeException(paramName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message, types));
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
            ThrowIfNotContainsType(value, paramName, FormattableString.Invariant($"Specified argument is not contained within at least one of {nameof(types)}."), types);
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
                ThrowWhen(c => c.IsFalse(() => Decorator.Enclose(value.GetType()).HasTypes(types)).Create(() => new ArgumentOutOfRangeException(paramName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message, types));
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
            ThrowIfNotContainsType<T>(typeParamName, FormattableString.Invariant($"Specified argument is not contained within at least one of {nameof(types)}."), types);
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
                ThrowWhen(c => c.IsFalse(() => Decorator.Enclose(typeof(T)).HasTypes(types)).Create(() => new TypeArgumentOutOfRangeException(typeParamName, DelimitedString.Create(types), message)).TryThrow());
            }
            catch (TypeArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(typeParamName, message, types));
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
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnum<TEnum>(string value, bool ignoreCase, string paramName, string message = "Value represents an enumeration.") where TEnum : struct, IConvertible
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => Condition.IsEnum<TEnum>(value, o => o.IgnoreCase = ignoreCase)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, ignoreCase, paramName, message));
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
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnum<TEnum>(string value, bool ignoreCase, string paramName, string message = "Value does not represents an enumeration.") where TEnum : struct, IConvertible
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => Condition.IsEnum<TEnum>(value, o => o.IgnoreCase = ignoreCase)).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, ignoreCase, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> represents an enumeration.
        /// </summary>
        /// <param name="value">The type to check is an enumeration.</param>
        /// <param name="paramName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> represents an enumeration.
        /// </exception>
        public static void ThrowIfEnumType(Type value, string paramName, string message = "Value represents an enumeration.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => value.GetTypeInfo().IsEnum).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
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
        public static void ThrowIfEnumType<TEnum>(string typeParamName, string message = "Value represents an enumeration.")
        {
            try
            {
                ThrowWhen(c => c.IsTrue(() => typeof(TEnum).GetTypeInfo().IsEnum).Create(() => new TypeArgumentException(typeParamName, message)).TryThrow());
            }
            catch (TypeArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(typeParamName, message));
            }
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
        public static void ThrowIfNotEnumType<TEnum>(string typeParamName, string message = "Value does not represents an enumeration.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => typeof(TEnum).GetTypeInfo().IsEnum).Create(() => new TypeArgumentException(typeParamName, message)).TryThrow());
            }
            catch (TypeArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(typeParamName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentException"/> if the specified <paramref name="value"/> does not represents an enumeration.
        /// </summary>
        /// <param name="value">The type to check is not an enumeration.</param>
        /// <param name="paramName">The name of the type parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> does not represents an enumeration.
        /// </exception>
        public static void ThrowIfNotEnumType(Type value, string paramName, string message = "Value does not represents an enumeration.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => value.GetTypeInfo().IsEnum).Create(() => new ArgumentException(message, paramName)).TryThrow());
            }
            catch (ArgumentException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> consist of anything besides binary digits.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> must consist only of binary digits.
        /// </exception>
        public static void ThrowIfNotBinaryDigits(string value, string paramName, string message = "Value must consist only of binary digits.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => Condition.IsBinaryDigits(value)).Create(() => new ArgumentOutOfRangeException(paramName, value, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if the specified <paramref name="value"/> consist of anything besides a base-64 structure.
        /// </summary>
        /// <param name="value">The value to be evaluated.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="value"/> must consist only of base-64 digits.
        /// </exception>
        public static void ThrowIfNotBase64String(string value, string paramName, string message = "Value must consist only of base-64 digits.")
        {
            try
            {
                ThrowWhen(c => c.IsFalse(() => Condition.IsBase64(value)).Create(() => new ArgumentOutOfRangeException(paramName, value, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ExceptionInsights.Embed(ex, MethodBase.GetCurrentMethod(), Arguments.ToArray(value, paramName, message));
            }
        }
    }
}