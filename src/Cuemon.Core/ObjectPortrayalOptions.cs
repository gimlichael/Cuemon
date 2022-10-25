using System;
using System.Globalization;
using System.Reflection;

namespace Cuemon
{
    /// <summary>
    /// Configuration options for <see cref="Generate.ObjectPortrayal" />.
    /// </summary>
    /// <seealso cref="FormattingOptions{T}"/>
    public sealed class ObjectPortrayalOptions : FormattingOptions<CultureInfo>
    {
        private string _nullValue;
        private string _noGetterValue;
        private string _delimiter;
        private Func<PropertyInfo, bool> _propertiesPredicate;
        private Func<PropertyInfo, object, IFormatProvider, string> _propertyConverter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectPortrayalOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="ObjectPortrayalOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="BypassOverrideCheck"/></term>
        ///         <description><c>false</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="NullValue"/></term>
        ///         <description><c>&lt;null&gt;</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="NoGetterValue"/></term>
        ///         <description><c>&lt;no getter&gt;</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="FormattingOptions{T}.FormatProvider"/></term>
        ///         <description><see cref="CultureInfo.InvariantCulture"/></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Delimiter"/></term>
        ///         <description><c>,</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="PropertyConverter"/></term>
        ///         <description><code>
        ///(property, instance, provider) =>
        ///{
        ///    if (property.CanRead)
        ///    {
        ///        if (TypeUtility.IsComplex(property.PropertyType))
        ///        {
        ///            return string.Format(provider, "{0}={1}", property.Name, ConvertFactory.UseConverter&lt;TypeRepresentationConverter&gt;().ChangeType(property.PropertyType, o => o.FullName = true));
        ///        }
        ///        var instanceValue = ReflectionUtility.GetPropertyValue(instance, property);
        ///        return string.Format(provider, "{0}={1}", property.Name, instanceValue ?? NullValue);
        ///    }
        ///    return string.Format(provider, "{0}={1}", property.Name, NoGetterValue);
        ///};
        /// </code></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="PropertiesPredicate"/></term>
        ///         <description><c>property => property.PropertyType.IsPublic &amp;&amp; property.GetIndexParameters().Length == 0</c></description>
        ///     </item>
        /// </list>
        /// </remarks>
        public ObjectPortrayalOptions()
        {
            BypassOverrideCheck = false;
            NullValue = "<null>";
            NoGetterValue = "<no getter>";
            FormatProvider = CultureInfo.InvariantCulture;
            Delimiter = ",";
            PropertyConverter = (property, instance, provider) =>
            {
                if (property.CanRead)
                {
                    if (Decorator.Enclose(property.PropertyType).IsComplex())
                    {
                        return string.Format(provider, "{0}={1}", property.Name, Decorator.Enclose(property.PropertyType).ToFriendlyName(o => o.FullName = true));
                    }
                    var instanceValue = Infrastructure.DefaultPropertyValueResolver(instance, property);
                    return string.Format(provider, "{0}={1}", property.Name, instanceValue ?? NullValue);
                }
                return string.Format(provider, "{0}={1}", property.Name, NoGetterValue);
            };
            PropertiesPredicate = property => property.PropertyType.IsPublic && property.GetIndexParameters().Length == 0;
        }

        /// <summary>
        /// Gets or sets the string representation of a <c>null</c> value.
        /// </summary>
        /// <value>The string representation of a <c>null</c> value.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public string NullValue
        {
            get => _nullValue;
            set
            {
                Validator.ThrowIfNullOrEmpty(value);
                _nullValue = value;
            }
        }

        /// <summary>
        /// Gets or sets the string representation of a missing <c>getter</c> method of a property.
        /// </summary>
        /// <value>The string representation of a missing <c>getter</c> method of a property.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="value"/> cannot be empty.
        /// </exception>
        public string NoGetterValue
        {
            get => _noGetterValue;
            set
            {
                Validator.ThrowIfNullOrEmpty(value);
                _noGetterValue = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an overriden method will return without further processing.
        /// </summary>
        /// <value><c>true</c> to bypass the check that evaluates if a ToString() method is overriden; otherwise, <c>false</c>.</value>
        /// <remarks>If <see cref="Generate.ObjectPortrayal"/> is called from within an overriden <see cref="object.ToString"/> method, this property should have a value of <c>true</c> to avoid <see cref="StackOverflowException"/>.</remarks>
        public bool BypassOverrideCheck { get; set; }

        /// <summary>
        /// Gets or sets the delimiter specification that is used together with <see cref="PropertyConverter"/>.
        /// </summary>
        /// <value>The delimiter specification that is used together with <see cref="PropertyConverter"/>.</value>
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

        /// <summary>
        /// Gets or sets the function delegate that convert a <see cref="PropertyInfo"/> object into a human-readable string.
        /// </summary>
        /// <value>The function delegate that convert a <see cref="PropertyInfo"/> object into a human-readable string.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public Func<PropertyInfo, object, IFormatProvider, string> PropertyConverter
        {
            get => _propertyConverter;
            set
            {
                Validator.ThrowIfNull(value);
                _propertyConverter = value;
            }
        }

        /// <summary>
        /// Gets or sets the function delegate that defines a set of criteria and determines whether the specified <see cref="PropertyInfo"/> meets those criteria.
        /// </summary>
        /// <value>The function delegate that defines a set of criteria and determines whether the specified <see cref="PropertyInfo"/> meets those criteria.</value>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> cannot be null.
        /// </exception>
        public Func<PropertyInfo, bool> PropertiesPredicate
        {
            get => _propertiesPredicate;
            set
            {
                Validator.ThrowIfNull(value);
                _propertiesPredicate = value;
            }
        }
    }
}