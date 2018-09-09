using System;

namespace Cuemon.Diagnostics
{
    /// <summary>
    /// Specifies options that is related to <see cref="ExceptionDescriptor" /> operations.
    /// </summary>
    /// <seealso cref="ExceptionDescriptor"/>.
    public class ExceptionDescriptorOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDescriptorOptions"/> class.
        /// </summary>
        public ExceptionDescriptorOptions()
        {
            UseBaseException = true;
            HelpLink = null;
        }

        /// <summary>
        /// Gets or sets the optional link to a help page.
        /// </summary>
        /// <value>The optional link to a help page.</value>
        public Uri HelpLink { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to expose only the base exception that caused the faulted operation.
        /// </summary>
        /// <value><c>true</c> if only the base exception is exposed; otherwise, <c>false</c>, to include the entire exception tree.</value>
        public bool UseBaseException { get; set; }
    }
}