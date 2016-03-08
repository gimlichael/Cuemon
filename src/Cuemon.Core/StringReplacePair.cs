using System.Text;

namespace Cuemon
{
    /// <summary>
    /// Defines a oldValue/newValue pair that can be set or retrieved for string replace operations.
    /// </summary>
    public struct StringReplacePair
    {
        private string _oldValue;
        private string _newValue;

        #region Constructors
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
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="string"/> value to be replaced.
        /// </summary>
        /// <value>The <see cref="string"/> value to be replaced.</value>
        public string OldValue
        {
            get { return _oldValue; }
            set { _oldValue = value; }
        }

        /// <summary>
        /// Gets the <see cref="string"/> value to replace all occurrences of <see cref="OldValue"/>.
        /// </summary>
        /// <value>The <see cref="string"/> value to replace all occurrences of <see cref="OldValue"/>.</value>
        public string NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }
        #endregion

        #region Methods
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
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
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
            StringBuilder builder = new StringBuilder();
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
        #endregion
    }
}