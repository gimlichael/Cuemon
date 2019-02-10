namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Specifies the location of the API key.
    /// </summary>
    public enum SwaggerSecurityIn
    {
        /// <summary>
        /// Parameters that are appended to the URL. For example, in /items?id=###, the query parameter is id.
        /// </summary>
        Query,
        /// <summary>
        /// Custom headers that are expected as part of the request.
        /// </summary>
        Header
    }
}