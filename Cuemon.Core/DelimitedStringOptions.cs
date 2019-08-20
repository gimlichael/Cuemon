using System;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="DelimitedString"/>.
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
        /// <paramref name="value"/> is empty or consist only of white-space characters.
        /// </exception>
        public string Delimiter
        {
            get => _delimiter;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value, nameof(value));
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
        /// <paramref name="value"/> is empty or consist only of white-space characters.
        /// </exception>
        public string Qualifier
        {
            get => _qualifier;
            set
            {
                Validator.ThrowIfNullOrWhitespace(value, nameof(value));
                _qualifier = value;
            }
        }
    }
}