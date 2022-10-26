using System;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="DelimitedString.Split"/>.
    /// </summary>
    public class DelimitedStringOptions
    {
        private string _delimiter;
        private string _qualifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelimitedStringOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="DelimitedStringOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Delimiter"/></term>
        ///         <description>,</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Qualifier"/></term>
        ///         <description>"</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public DelimitedStringOptions()
        {
            Delimiter = ",";
            Qualifier = "\"";
        }

        /// <summary>
        /// Gets or sets the delimiter that separates the fields. Default is comma (,).
        /// </summary>
        /// <value>The delimiter that separates the fields.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public string Delimiter
        {
            get => _delimiter;
            set
            {
                Validator.ThrowIfNullOrEmpty(value);
                _delimiter = value;
            }
        }

        /// <summary>
        /// Gets or sets the qualifier placed around each field to signify that it is the same field. Default is quotation mark (").
        /// </summary>
        /// <value>The qualifier placed around each field to signify that it is the same field.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public string Qualifier
        {
            get => _qualifier;
            set
            {
                Validator.ThrowIfNullOrEmpty(value);
                _qualifier = value;
            }
        }
    }

    /// <summary>
    /// Configuration options for <see cref="DelimitedString" />.
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
                Validator.ThrowIfNull(value);
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
                Validator.ThrowIfNullOrEmpty(value);
                _delimiter = value;
            }
        }
    }
}