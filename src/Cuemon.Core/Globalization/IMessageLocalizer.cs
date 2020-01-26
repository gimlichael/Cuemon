using System;

namespace Cuemon.Globalization
{
    /// <summary>
    /// Provides a generic way to support localized messages on attribute decorated methods.
    /// </summary>
    public interface IMessageLocalizer
    {
        /// <summary>
        /// Gets or sets and explicit message string.
        /// </summary>
        /// <value> This property is intended to be used for non-localizable messages. Use <see cref="MessageResourceType"/> and <see cref="MessageResourceName"/> for localizable messages.</value>
        string Message { get; set; }

        /// <summary>
        /// Gets or sets the resource name (property name) to use as the key for lookups on the resource type.
        /// </summary>
        /// <value>Use this property to set the name of the property within <see cref="MessageResourceType"/> that will provide a localized message.</value>
        string MessageResourceName { get; set; }

        /// <summary>
        /// Gets or sets the resource type to use for message lookups.
        /// </summary>
        /// <value>Use this property only in conjunction with <see cref="MessageResourceName"/>. They are used together to retrieve localized messages at runtime.</value>
        Type MessageResourceType { get; set; }
    }
}