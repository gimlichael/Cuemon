using System;
using System.Collections.Generic;
using System.Text;
using Cuemon.Collections.Generic;

namespace Cuemon
{
    /// <summary>
    /// Defines a oldValue/newValue pair that can be set or retrieved for string replace operations.
    /// </summary>
    public struct StringReplacePair
    {
        /// <summary>
        /// Replaces all occurrences of <paramref name="oldValue"/> in <paramref name="value"/>, with <paramref name="newValue"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the replacement on.</param>
        /// <param name="oldValue">The <see cref="string"/> value to be replaced.</param>
        /// <param name="newValue">The <see cref="string"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison. Default is <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
        /// <returns>A <see cref="string"/> equivalent to <paramref name="value"/> but with all instances of <paramref name="oldValue"/> replaced with <paramref name="newValue"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null -or-
        /// <paramref name="oldValue"/> is null.
        /// </exception>
        public static string ReplaceAll(string value, string oldValue, string newValue, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(oldValue, nameof(oldValue));
            return ReplaceAll(value, Arguments.Yield(new StringReplacePair(oldValue, newValue)), comparison);
        }

        /// <summary>
        /// Replaces all occurrences of the <see cref="StringReplacePair.OldValue"/> with <see cref="StringReplacePair.NewValue"/> of the <paramref name="replacePairs"/> sequence in <paramref name="value"/>.
        /// </summary>
        /// <param name="value">The <see cref="string"/> value to perform the replacement on.</param>
        /// <param name="replacePairs">A sequence of <see cref="StringReplacePair"/> values.</param>
        /// <param name="comparison">One of the enumeration values that specifies the rules to use in the comparison. Default is <see cref="StringComparison.OrdinalIgnoreCase"/>.</param>
        /// <returns>A <see cref="string"/> equivalent to <paramref name="value"/> but with all instances of <see cref="OldValue"/> replaced with <see cref="NewValue"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null -or-
        /// <paramref name="replacePairs"/> is null.
        /// </exception>
        public static string ReplaceAll(string value, IEnumerable<StringReplacePair> replacePairs, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            Validator.ThrowIfNull(value, nameof(value));
            Validator.ThrowIfNull(replacePairs, nameof(replacePairs));
            var replaceEngine = new StringReplaceEngine(value, replacePairs, comparison);
            return replaceEngine.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReplacePair"/> struct.
        /// </summary>
        /// <param name="oldValue">The <see cref="string"/> value to be replaced.</param>
        /// <param name="newValue">The <see cref="string"/> value to replace all occurrences of <paramref name="oldValue"/>.</param>
        public StringReplacePair(string oldValue, string newValue) : this()
        {
            NewValue = newValue;
            OldValue = oldValue;
        }

        /// <summary>
        /// Gets or sets the <see cref="string"/> value to be replaced.
        /// </summary>
        /// <value>The <see cref="string"/> value to be replaced.</value>
        public string OldValue { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="string"/> value to replace all occurrences of <see cref="OldValue"/>.
        /// </summary>
        /// <value>The <see cref="string"/> value to replace all occurrences of <see cref="OldValue"/>.</value>
        public string NewValue { get; set; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return OldValue.GetHashCode() ^ NewValue.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is StringReplacePair)) { return false; }
            return Equals((StringReplacePair)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise, <c>false</c>. </returns>
        public bool Equals(StringReplacePair other)
        {
            if ((OldValue != other.OldValue)) { return false; }
            return (NewValue == other.NewValue);
        }

        /// <summary>
        /// Indicates whether two <see cref="StringReplacePair"/> instances are equal.
        /// </summary>
        /// <param name="replacePair1">The first date interval to compare.</param>
        /// <param name="replacePair2">The second date interval to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="replacePair1"/> and <paramref name="replacePair2"/> are equal; otherwise, false. </returns>
        public static bool operator ==(StringReplacePair replacePair1, StringReplacePair replacePair2)
        {
            return replacePair1.Equals(replacePair2);
        }

        /// <summary>
        /// Indicates whether two <see cref="DateSpan"/> instances are not equal.
        /// </summary>
        /// <param name="replacePair1">The first date interval to compare.</param>
        /// <param name="replacePair2">The second date interval to compare.</param>
        /// <returns><c>true</c> if the values of <paramref name="replacePair1"/> and <paramref name="replacePair2"/> are not equal; otherwise, false.</returns>
        public static bool operator !=(StringReplacePair replacePair1, StringReplacePair replacePair2)
        {
            return !replacePair1.Equals(replacePair2);
        }

        /// <summary>
        /// Returns a string representation of the <see cref="StringReplacePair"/>, using the string representations of the oldValue and newValue.
        /// </summary>
        /// <returns>A string representation of the <see cref="StringReplacePair"/>, which includes the string representations of the oldValue and newValue.</returns>
        /// <remarks>The string representation consists of the string representations of the oldValue and newValue, separated by a comma and a space, and enclosed in square brackets. For example, the ToString method for a <see cref="StringReplacePair"/> structure with the string OldValue "Test1" and the string NewValue "Test2" returns the string "[Test1, Test2]".</remarks>
        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append('[');
            if (OldValue != null)
            {
                builder.Append(OldValue);
            }
            builder.Append(", ");
            if (NewValue != null)
            {
                builder.Append(NewValue);
            }
            builder.Append(']');
            return builder.ToString();
        }
    }
}