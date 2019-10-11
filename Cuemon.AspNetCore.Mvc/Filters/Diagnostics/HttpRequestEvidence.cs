using System;
using System.IO;
using Cuemon.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Cuemon.AspNetCore.Mvc.Filters.Diagnostics
{
    /// <summary>
    /// Provides detailed information about a given <seealso cref="HttpRequest"/>.
    /// </summary>
    public class HttpRequestEvidence
    {
        internal HttpRequestEvidence(HttpRequest request, Func<Stream, string> bodyParser = null)
        {
            var hasMultipartContentType = request.GetMultipartBoundary().Length > 0;
            if (bodyParser == null) { bodyParser = body => hasMultipartContentType ? null : ConvertFactory.UseCodec<StreamToStringCodec>().Encode(body); }
            Location = request.GetDisplayUrl();
            Method = request.Method;
            Headers = request.Headers;
            Query = request.Query;
            if (request.HasFormContentType && !hasMultipartContentType) { Form = request.Form; }
            Cookies = request.Cookies;
            Body = bodyParser(request.Body);
        }

        /// <summary>
        /// Gets the request URL in a fully un-escaped form (except for the QueryString).
        /// </summary>
        /// <value>The request URL in a fully un-escaped form (except for the QueryString).</value>
        public string Location { get; }

        /// <summary>
        /// Gets the request HTTP method.
        /// </summary>
        /// <value>The HTTP method.</value>
        public string Method { get; }

        /// <summary>
        /// Gets the request headers.
        /// </summary>
        /// <value>The headers of the request.</value>
        public IHeaderDictionary Headers { get; }

        /// <summary>
        /// Gets the associated keys and values collection parsed from the <see cref="HttpRequest.QueryString"/>.
        /// </summary>
        /// <value>The associated keys and values collection parsed from the <see cref="HttpRequest.QueryString"/>.</value>
        public IQueryCollection Query { get; }

        /// <summary>
        /// Gets the associated keys and values collection from the <see cref="HttpRequest.Form"/>.
        /// </summary>
        /// <value>The associated keys and values collection parsed from the <see cref="HttpRequest.Form"/>.</value>
        public IFormCollection Form { get; }

        /// <summary>
        /// Gets the collection of cookies for the request.
        /// </summary>
        /// <value>The collection of cookies for the request.</value>
        public IRequestCookieCollection Cookies { get; }

        /// <summary>
        /// Gets the body of the request.
        /// </summary>
        /// <value>The body of the request.</value>
        public string Body { get; }
    }
}