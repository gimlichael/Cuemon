using System;
using System.IO;
using Cuemon.Integrity;
using Cuemon.Security.Cryptography;
using Microsoft.AspNetCore.Http;

namespace Cuemon.AspNetCore.Mvc.Filters
{
    /// <summary>
    /// Specifies options that is related to <see cref="EntityTagHeaderFilter" /> operations.
    /// </summary>
    public class EntityTagHeaderOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTagHeaderOptions"/> class.
        /// </summary>
        /// <remarks>
        /// The following table shows the initial property values for an instance of <see cref="EntityTagHeaderOptions"/>.
        /// <list type="table">
        ///     <listheader>
        ///         <term>Property</term>
        ///         <description>Initial Value</description>
        ///     </listheader>
        ///     <item>
        ///         <term><see cref="EntityTagParser"/></term>
        ///         <description>A delegate that computes a MD5 hash of the body and add or update the necessary HTTP response headers needed to provide entity tag header information.</description>
        ///     </item>
        /// </list>
        /// </remarks>
        public EntityTagHeaderOptions()
        {
            EntityTagParser = (body, request, response) =>
            {
                var builder = new ChecksumBuilder(body.ComputeHash(o =>
                {
                    o.AlgorithmType = HashAlgorithmType.MD5;
                    o.LeaveStreamOpen = true;
                }).Value);
                response.SetEntityTagHeaderInformation(request, builder);
            };
        }

        /// <summary>
        /// Gets or sets the callback delegate that is invoked asynchronously before the action result.
        /// </summary>
        /// <value>An <see cref="Action{Stream, HttpRequest, HttpResponse}"/>. The default value computes a MD5 hash of the body and add or update the necessary HTTP response headers needed to provide entity tag header information.</value>
        public Action<Stream, HttpRequest, HttpResponse> EntityTagParser { get; set; }
    }
}