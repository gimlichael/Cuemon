using System;
using System.Collections.Generic;
using System.Linq;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Validator"/> class.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Validates and throws an <see cref="ReservedKeywordException"/> if the specified <paramref name="keyword"/> is found in the sequence of <paramref name="reservedKeywords"/>.
        /// </summary>
        /// <param name="validator">The <see cref="Validator"/> to extend.</param>
        /// <param name="keyword">The keyword to compare with <paramref name="reservedKeywords"/>.</param>
        /// <param name="reservedKeywords">The reserved keywords to compare with <paramref name="keyword"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ReservedKeywordException">
        /// The specified <paramref name="keyword"/> is contained within <paramref name="reservedKeywords"/>.
        /// </exception>
        public static void ContainsReservedKeyword(this Validator validator, string keyword, IEnumerable<string> reservedKeywords, string paramName, string message = null)
        {
            ContainsReservedKeyword(validator, keyword, reservedKeywords, null, paramName, message);
        }

        /// <summary>
        /// Validates and throws an <see cref="ReservedKeywordException"/> if the specified <paramref name="keyword"/> is found in the sequence of <paramref name="reservedKeywords"/>.
        /// </summary>
        /// <param name="validator">The <see cref="Validator"/> to extend.</param>
        /// <param name="keyword">The keyword to compare with <paramref name="reservedKeywords"/>.</param>
        /// <param name="reservedKeywords">The reserved keywords to compare with <paramref name="keyword"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="reservedKeywords"/> with <paramref name="keyword"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ReservedKeywordException">
        /// The specified <paramref name="keyword"/> is contained within <paramref name="reservedKeywords"/>.
        /// </exception>
        public static void ContainsReservedKeyword(this Validator validator, string keyword, IEnumerable<string> reservedKeywords, IEqualityComparer<string> comparer, string paramName, string message = null)
        {
            if (keyword == null || reservedKeywords == null) { return; }
            try
            {
                validator.ThrowWhenCondition(c => c.IsTrue(() => reservedKeywords.Contains(keyword, comparer)).Create(() => new ReservedKeywordException(paramName, keyword, message)).TryThrow());
            }
            catch (ReservedKeywordException ex)
            {
                throw ex;
            }
        }

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
        public static void HasDifference(this Validator validator, string first, string second, string paramName, string message = null)
        {
            message ??= FormattableString.Invariant($"Specified arguments has a difference between {nameof(second)} and {nameof(first)}.");
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
        public static void NoDifference(this Validator validator, string first, string second, string paramName, string message = null)
        {
            message ??= FormattableString.Invariant($"Specified arguments does not have a difference between {nameof(second)} and {nameof(first)}.");
            try
            {
                validator.ThrowWhenCondition(c => c.IsFalse(() => Condition.Query.HasDifference(first, second, out _)).Create(() => new ArgumentOutOfRangeException(paramName, message)).TryThrow());
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw ex;
            }
        }
    }
}