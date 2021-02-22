namespace Cuemon.AspNetCore.Authentication
{
    /// <summary>
    /// Configuration options for <see cref="AuthorizationHeader"/>.
    /// </summary>
    public class AuthorizationHeaderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationHeaderOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="AuthorizationHeaderOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="CredentialsDelimiter"/></term>
        ///         <description>,</description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="CredentialsKeyValueDelimiter"/></term>
        ///         <description>=</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public AuthorizationHeaderOptions()
        {
            CredentialsDelimiter = ",";
            CredentialsKeyValueDelimiter = "=";
        }

        /// <summary>
        /// Gets or sets the credentials delimiter.
        /// </summary>
        /// <value>The credentials delimiter.</value>
        public string CredentialsDelimiter { get; set; }

        /// <summary>
        /// Gets or sets the credentials key value delimiter.
        /// </summary>
        /// <value>The credentials key value delimiter.</value>
        public string CredentialsKeyValueDelimiter { get; set; }
    }
}