using System;
using Microsoft.AspNetCore.Http;

namespace Cuemon.Extensions.AspNetCore
{
    /// <summary>
    /// Extension methods for the <see cref="Int32"/> struct.
    /// </summary>
    public static class Int32Extensions
    {
        /// <summary>
        /// Determines whether the specified <paramref name="statusCode"/> is within the informational range.
        /// </summary>
        /// <param name="statusCode">The <see cref="int"/> to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="statusCode"/> was in the <b>Information</b> range (100-199); otherwise, <c>false</c>.</returns>
        public static bool IsInformationStatusCode(this int statusCode)
        {
            return (statusCode >= StatusCodes.Status100Continue && statusCode <= 199);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="statusCode"/> is within the successful range.
        /// </summary>
        /// <param name="statusCode">The <see cref="int"/> to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="statusCode"/> was in the <b>Successful</b> range (200-299); otherwise, <c>false</c>.</returns>
        public static bool IsSuccessStatusCode(this int statusCode)
        {
            return (statusCode >= StatusCodes.Status200OK && statusCode <= 299);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="statusCode"/> is within the redirecting range.
        /// </summary>
        /// <param name="statusCode">The <see cref="int"/> to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="statusCode"/> was in the <b>Redirection</b> range (300-399); otherwise, <c>false</c>.</returns>
        public static bool IsRedirectionStatusCode(this int statusCode)
        {
            return (statusCode >= StatusCodes.Status300MultipleChoices && statusCode <= 399);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="statusCode"/> equals a 304 Not Modified.
        /// </summary>
        /// <param name="statusCode">The <see cref="int"/> to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="statusCode"/> is <b>NotModified</b> (304); otherwise, <c>false</c>.</returns>
        public static bool IsNotModifiedStatusCode(this int statusCode)
        {
            return (statusCode == StatusCodes.Status304NotModified);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="statusCode"/> is within the client error related range.
        /// </summary>
        /// <param name="statusCode">The <see cref="int"/> to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="statusCode"/> was in the <b>Client Error</b> range (400-499); otherwise, <c>false</c>.</returns>
        public static bool IsClientErrorStatusCode(this int statusCode)
        {
            return (statusCode >= StatusCodes.Status400BadRequest && statusCode <= 499);
        }

        /// <summary>
        /// Determines whether the specified <paramref name="statusCode"/> is within the server error related range.
        /// </summary>
        /// <param name="statusCode">The <see cref="int"/> to evaluate.</param>
        /// <returns><c>true</c> if <paramref name="statusCode"/> was in the <b>Server Error</b> range (500-599); otherwise, <c>false</c>.</returns>
        public static bool IsServerErrorStatusCode(this int statusCode)
        {
            return (statusCode >= StatusCodes.Status500InternalServerError && statusCode <= 599);
        }
    }
}