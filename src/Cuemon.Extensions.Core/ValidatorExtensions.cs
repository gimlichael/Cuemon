using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Cuemon.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="Validator"/> class.
    /// </summary>
    public static class ValidatorExtensions
    {
        /// <summary>
        /// Validates and throws a <see cref="ArgumentReservedKeywordException"/> if the specified <paramref name="keyword"/> is found in the sequence of <paramref name="reservedKeywords"/>.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="keyword">The keyword to compare with <paramref name="reservedKeywords"/>.</param>
        /// <param name="reservedKeywords">The reserved keywords to compare with <paramref name="keyword"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentReservedKeywordException">
        /// The specified <paramref name="keyword"/> is contained within <paramref name="reservedKeywords"/>.
        /// </exception>
        public static void ContainsReservedKeyword(this Validator _, string keyword, IEnumerable<string> reservedKeywords, [CallerArgumentExpression(nameof(keyword))] string paramName = null, string message = null)
        {
            ContainsReservedKeyword(_, keyword, reservedKeywords, null, paramName, message);
        }

        /// <summary>
        /// Validates and throws a <see cref="ArgumentReservedKeywordException"/> if the specified <paramref name="keyword"/> is found in the sequence of <paramref name="reservedKeywords"/>.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="keyword">The keyword to compare with <paramref name="reservedKeywords"/>.</param>
        /// <param name="reservedKeywords">The reserved keywords to compare with <paramref name="keyword"/>.</param>
        /// <param name="comparer">The <see cref="IEqualityComparer{T}"/> implementation to use when comparing <paramref name="reservedKeywords"/> with <paramref name="keyword"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentReservedKeywordException">
        /// The specified <paramref name="keyword"/> is contained within <paramref name="reservedKeywords"/>.
        /// </exception>
        public static void ContainsReservedKeyword(this Validator _, string keyword, IEnumerable<string> reservedKeywords, IEqualityComparer<string> comparer, [CallerArgumentExpression(nameof(keyword))] string paramName = null, string message = null)
        {
            if (keyword == null || reservedKeywords == null) { return; }
            if (reservedKeywords.Contains(keyword, comparer ?? EqualityComparer<string>.Default)) { throw new ArgumentReservedKeywordException(paramName, keyword, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is a difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="first">The value that specifies valid characters.</param>
        /// <param name="second">The value to compare with <paramref name="first"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is a difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </exception>
        public static void HasDifference(this Validator _, string first, string second, string paramName, string message = null)
        {
            message ??= FormattableString.Invariant($"Specified arguments has a difference between {nameof(second)} and {nameof(first)}.");
            if (Condition.Query.HasDifference(first, second, out var invalidCharacters)) { throw new ArgumentOutOfRangeException(paramName, invalidCharacters, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if there is no difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="first">The value that specifies valid characters.</param>
        /// <param name="second">The value to compare with <paramref name="first"/>.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <param name="message">A message that describes the error.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// There is no difference between <paramref name="second"/> and <paramref name="first"/>.
        /// </exception>
        public static void NoDifference(this Validator _, string first, string second, string paramName, string message = null)
        {
            message ??= FormattableString.Invariant($"Specified arguments does not have a difference between {nameof(second)} and {nameof(first)}.");
            if (!Condition.Query.HasDifference(first, second, out var _)) { throw new ArgumentOutOfRangeException(paramName, message); }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if any of the <paramref name="characters"/> occurs within the <paramref name="argument"/>.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="characters">The sequence of <see cref="char"/> to search within <paramref name="argument"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> contains one or more of the specified <paramref name="characters"/>.
        /// </exception>
        public static void ContainsAny(this Validator _, string argument, char[] characters, StringComparison comparison = StringComparison.OrdinalIgnoreCase, string message = "One or more character matches were found.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (argument?.ContainsAny(comparison, characters) ?? false)
            {
                throw new ArgumentOutOfRangeException(paramName, argument.Where(characters.Contains)
                    .Distinct()
                    .ToDelimitedString(o => o.StringConverter = c => $"'{c}'"), message);
            }
        }

        /// <summary>
        /// Validates and throws an <see cref="ArgumentOutOfRangeException"/> if any of the <paramref name="characters"/> does not occur within the <paramref name="argument"/>.
        /// </summary>
        /// <param name="_">The <see cref="Validator"/> to extend.</param>
        /// <param name="argument">The value to be evaluated.</param>
        /// <param name="characters">The sequence of <see cref="char"/> to search within <paramref name="argument"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison.</param>
        /// <param name="message">The message that describes the error.</param>
        /// <param name="paramName">The name of the parameter that caused the exception.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="argument"/> does not contain any of the specified <paramref name="characters"/>.
        /// </exception>
        public static void NotContainsAny(this Validator _, string argument, char[] characters, StringComparison comparison = StringComparison.OrdinalIgnoreCase, string message = "No matching characters were found.", [CallerArgumentExpression(nameof(argument))] string paramName = null)
        {
            if (!argument?.ContainsAny(comparison, characters) ?? true)
            {
                throw new ArgumentOutOfRangeException(paramName, characters
                    .Distinct()
                    .ToDelimitedString(o => o.StringConverter = c => $"'{c}'"), message);
            }
        }
    }
}
