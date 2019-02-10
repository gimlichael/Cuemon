namespace Cuemon.AspNetCore.Mvc.Swagger
{
    /// <summary>
    /// Determines the format of the array if type array is used.
    /// </summary>
    public enum SwaggerCollectionFormat
    {
        /// <summary>
        /// Comma separated values foo,bar.
        /// </summary>
        Csv,
        /// <summary>
        /// Space separated values foo bar.
        /// </summary>
        Ssv,
        /// <summary>
        /// Tab separated values foo\tbar.
        /// </summary>
        Tsv,
        /// <summary>
        /// Pipe separated values foo|bar.
        /// </summary>
        Pipes
    }
}