namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Specifies the type of the security scheme.
    /// </summary>
    public enum SwaggerSecurityType
    {
        /// <summary>
        /// Basic Authentication.
        /// </summary>
        Basic,
        /// <summary>
        /// API key (either as a header or as a query parameter).
        /// </summary>
        ApiKey,
        /// <summary>
        /// OAuth2's common flows (implicit, password, application and access code).
        /// </summary>
        Oauth2
    }
}