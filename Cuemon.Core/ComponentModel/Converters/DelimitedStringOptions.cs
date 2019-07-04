using System;

namespace Cuemon.ComponentModel.Converters
{
    /// <summary>
    /// Configuration options for <see cref="DelimitedStringConverter{T}" />.
    /// </summary>
    /// <typeparam name="T">The type of the object to convert.</typeparam>
    public class DelimitedStringOptions<T>
    {
        private string _delimiter;
        private Func<T, string> _stringConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedStringOptions{T}"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="DelimitedStringOptions{T}"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Delimiter"/></term>
        ///         <description><c>,</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="StringConverter"/></term>
        ///         <description><c>o => o.ToString()</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public DelimitedStringOptions()
        {
            Delimiter = ",";
            StringConverter = o => o.ToString();
        }

        /// <summary>
        /// Gets or sets the function delegate that converts <typeparamref name="T"/> to a string representation.
        /// </summary>
        /// <value>The function delegate that converts <typeparamref name="T"/> to a string representation.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public Func<T, string> StringConverter
        {
            get => _stringConverter;
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _stringConverter = value;
            }
        }

        /// <summary>
        /// Gets or sets the delimiter specification.
        /// </summary>
        /// <value>The delimiter specification.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public string Delimiter
        {
            get => _delimiter;
            set
            {
                Validator.ThrowIfNullOrEmpty(value, nameof(value));
                _delimiter = value;
            }
        }
    }
}