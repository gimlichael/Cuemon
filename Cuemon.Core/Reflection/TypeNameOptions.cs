using System;
using System.Globalization;

namespace Cuemon.Reflection
{
    /// <summary>
    /// Configuration options for <see cref="TypeInsight.ToHumanReadableString" />.
    /// </summary>
    public sealed class TypeNameOptions : FormattingOptions<CultureInfo>
    {
        private Func<Type, IFormatProvider, bool, string> _humanReadableStringConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNameOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="TypeNameOptions"/>.
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
        ///         <term><see cref="HumanReadableStringConverter"/></term>
        ///         <description><code>(type, provider, fullname) => fullname ? type.FullName : type.Name;</code></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public TypeNameOptions()
        {
            ExcludeGenericArguments = false;
            FormatProvider = CultureInfo.InvariantCulture;
            FullName = false;
            HumanReadableStringConverter = (type, provider, fullname) => fullname ? type.FullName : type.Name;
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
        public Func<Type, IFormatProvider, bool, string> HumanReadableStringConverter
        {
            get => _humanReadableStringConverter;
            set
            {
                Validator.ThrowIfNull(value, nameof(value));
                _humanReadableStringConverter = value;
            }
        }
    }
}