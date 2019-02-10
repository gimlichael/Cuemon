using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace Cuemon.Net.Http
{
    /// <summary>
    /// This is an extension implementation of the most common methods on the <see cref="HttpManager"/> class.
    /// </summary>
    public static class HttpManagerExtensions
    {
        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpDeleteAsync(this Uri location, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpDeleteAsync(location, ct);
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpGetAsync(this Uri location, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpGetAsync(location, ct);
            }
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpHeadAsync(this Uri location, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpHeadAsync(location, ct);
            }
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpOptionsAsync(this Uri location, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpOptionsAsync(location, ct);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpPostAsync(this Uri location, string contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpPostAsync(location, contentType, content, ct);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpPostAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpPostAsync(location, contentType, content, ct);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpPutAsync(this Uri location, string contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpPutAsync(location, contentType, content, ct);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpPutAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpPutAsync(location, contentType, content, ct);
            }
        }

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpPatchAsync(this Uri location, string contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpPatchAsync(location, contentType, content, ct);
            }
        }

        /// <summary>
        /// Send a PATCH request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpPatchAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpPatchAsync(location, contentType, content, ct);
            }
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpTraceAsync(this Uri location, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpTraceAsync(location, ct);
            }
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method, string contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpAsync(method, location, contentType, content, ct);
            }
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="ct">The cancellation token to cancel operation.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method, MediaTypeHeaderValue contentType, Stream content, CancellationToken ct = default(CancellationToken))
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpAsync(method, location, contentType, content, ct);
            }
        }

        /// <summary>
        /// Send a request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The <see cref="Uri"/> to request.</param>
        /// <param name="setup">The <see cref="HttpRequestOptions"/> which need to be configured.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> HttpAsync(this Uri location, Action<HttpRequestOptions> setup)
        {
            using (var manager = new HttpManager())
            {
                return await manager.HttpAsync(location, setup);
            }
        }
    }
}