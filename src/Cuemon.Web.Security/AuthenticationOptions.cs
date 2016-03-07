namespace Cuemon.Web.Security
{
    /// <summary>
    /// Base options for all authentication middleware.
    /// </summary>
    public abstract class AuthenticationOptions
    {
        /// <summary>
        /// Gets the realm that defines the protection space.
        /// </summary>
        /// <value>The realm that defines the protection space.</value>
        public string Realm { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a HTTP connection is required to use secure sockets (that is, HTTPS).
        /// </summary>
        /// <value><c>true</c> if the HTTP connection is required to use secure sockets (that is, HTTPS); otherwise, <c>false</c>.</value>
        public bool RequireSecureConnection { get; set; }
    }
}