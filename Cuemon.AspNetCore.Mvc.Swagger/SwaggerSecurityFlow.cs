namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Specifies the flow used by the OAuth2 security scheme.
    /// </summary>
    public enum SwaggerSecurityFlow
    {
        /// <summary>
        /// Implicit flow.
        /// </summary>
        Implicit,
        /// <summary>
        /// Password flow.
        /// </summary>
        Password,
        /// <summary>
        /// Application flow.
        /// </summary>
        Application,
        /// <summary>
        /// Access code flow.
        /// </summary>
        AccessCode
    }
}