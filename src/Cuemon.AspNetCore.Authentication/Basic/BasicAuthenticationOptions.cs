namespace Cuemon.AspNetCore.Authentication.Basic
{
    /// <summary>
    /// Configuration options for <see cref="BasicAuthenticationMiddleware"/>. This class cannot be inherited.
    /// </summary>
    /// <seealso cref="AuthenticationOptions" />
    public sealed class BasicAuthenticationOptions : AuthenticationOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicAuthenticationOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="BasicAuthenticationOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="Authenticator"/></term>
        ///         <description><c>null</c></description>
        ///     </item>
        ///     <item>
        ///         <term><see cref="Realm"/></term>
        ///         <description>AuthenticationServer</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public BasicAuthenticationOptions()
        {
            Realm = "AuthenticationServer";
        }

        /// <summary>
        /// Gets or sets the function delegate that will perform the authentication from the specified <c>username</c> and <c>password</c>.
        /// </summary>
        /// <value>The function delegate that will perform the authentication.</value>
        public BasicAuthenticator Authenticator { get; set; }

        /// <summary>
        /// Gets the realm that defines the protection space.
        /// </summary>
        /// <value>The realm that defines the protection space.</value>
        public string Realm { get; set; }
    }
}