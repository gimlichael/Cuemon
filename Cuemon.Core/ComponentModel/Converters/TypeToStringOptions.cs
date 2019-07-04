using System;
using System.Globalization;
using System.Linq;
using Cuemon.Reflection;

namespace Cuemon.ComponentModel.Converters
{
    /// <summary>
    /// Configuration options for <see cref="TypeToStringConverter" />.
    /// </summary>
    /// <seealso cref="FormattingOptions{T}"/>
    public sealed class TypeToStringOptions : FormattingOptions<CultureInfo>
    {
        private static readonly char[] InvalidCharacters = Alphanumeric.PunctuationMarks.Replace(".", "").ToCharArray();
        private Func<Type, IFormatProvider, bool, string> _typeConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeToStringOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ObjectToStringOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="ExcludeGenericArguments"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FullName"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FormattingOptions{T}.FormatProvider"/></term>
        ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="TypeConverter"/></term>
        ///         <description><code>(type, provider, fullname) => fullname ? type.FullName : type.Name;</code></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public TypeToStringOptions()
        {
            ExcludeGenericArguments = false;
            FormatProvider = CultureInfo.InvariantCulture;
            FullName = false;
            TypeConverter = (type, provider, fullname) => fullname ? type.FullName : type.Name;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to exclude generic arguments from a <see cref="Type"/>.
        /// </summary>
        /// <value><c>true</c> to exclude generic arguments from a <see cref="Type"/>; otherwise, <c>false</c>.</value>
        public bool ExcludeGenericArguments { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the fully qualified name of a <see cref="Type"/>.
        /// </summary>
        /// <value><c>true</c> to use the fully qualified name of a <see cref="Type"/>; otherwise, <c>false</c>.</value>
        public bool FullName { get; set; }

        /// <summary>
        /// Gets or sets the function delegate that convert a <see cref="Type"/> object into a human-readable string.
        /// </summary>
        /// <value>The function delegate that convert a <see cref="Type"/> object into a human-readable string.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public Func<Type, IFormatProvider, bool, string> TypeConverter
        {
            get => _typeConverter;
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _typeConverter = value;
            }
        }
    }
}