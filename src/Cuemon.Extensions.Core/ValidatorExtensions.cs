using System;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Validator"/> class.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is a difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="validator">The <see cref="Validator"/> to extend.</param>
        /// <param name="first">The value that specifies valid characters.</param>
        /// <param name="second">The value to compare with <paramref name="first"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is a difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </exception>
        public static void IfHasDifference(this Validator validator, string first, string second, string paramName, string message = null)
        {
            if (message == null) { message = FormattableString.Invariant($"Specified arguments has a difference between {nameof(second)} and {nameof(first)}."); }
            try
            {
                validator.ThrowWhenCondition(c => c.IsTrue((out string invalidCharacters) => Condition.Query.HasDifference(first, second, out invalidCharacters)).Create(invalidCharacters => new ArgumentOutOfRangeException(paramName, invalidCharacters, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is a distinct difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="validator">The <see cref="Validator"/> to extend.</param>
        /// <param name="first">The value that specifies valid characters.</param>
        /// <param name="second">The value to distinctively compare with <paramref name="first"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is a distinct difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </exception>
        public static void IfHasDistinctDifference(this Validator validator, string first, string second, string paramName, string message = null)
        {
            if (message == null) { message = FormattableString.Invariant($"Specified arguments has a distinct difference between {nameof(second)} and {nameof(first)}."); }
            try
            {
                validator.ThrowWhenCondition(c => c.IsTrue((out string invalidCharacters) => Condition.Query.HasDistinctDifference(first, second, out invalidCharacters)).Create(invalidCharacters => new ArgumentOutOfRangeException(paramName, invalidCharacters, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is no difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="validator">The <see cref="Validator"/> to extend.</param>
        /// <param name="first">The value that specifies valid characters.</param>
        /// <param name="second">The value to compare with <paramref name="first"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is no difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </exception>
        public static void IfHasNotDifference(this Validator validator, string first, string second, string paramName, string message = null)
        {
            if (message == null) { message = FormattableString.Invariant($"Specified arguments does not have a difference between {nameof(second)} and {nameof(first)}."); }
            try
            {
                validator.ThrowWhenCondition(c => c.IsFalse(() => Condition.Query.HasDifference(first, second, out _)).Create(() => new ArgumentOutOfRangeException(paramName, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is not a distinct difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="validator">The <see cref="Validator"/> to extend.</param>
        /// <param name="first">The value that specifies valid characters.</param>
        /// <param name="second">The value to distinctively compare with <paramref name="first"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is not a distinct difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </exception>
        public static void IfHasNotDistinctDifference(this Validator validator, string first, string second, string paramName, string message = null)
        {
            if (message == null) { message = FormattableString.Invariant($"Specified arguments does not have a distinct difference between {nameof(second)} and {nameof(first)}."); }
            try
            {
                validator.ThrowWhenCondition(c => c.IsFalse(() => Condition.Query.HasDistinctDifference(first, second, out _)).Create(() => new ArgumentOutOfRangeException(paramName, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }
    }
}