using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpDeleteAsync(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpDeleteAsync(location);
            }
        }

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpDeleteAsync(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpDeleteAsync(location, timeout);
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpGet(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpGetAsync(location);
            }
        }

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpGet(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpGetAsync(location, timeout);
            }
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpHeadAsync(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpHeadAsync(location);
            }
        }

        /// <summary>
        /// Send a HEAD request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpHeadAsync(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpHeadAsync(location, timeout);
            }
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpOptionsAsync(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpOptionsAsync(location);
            }
        }

        /// <summary>
        /// Send an OPTIONS request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpOptionsAsync(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpOptionsAsync(location, timeout);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPostAsync(this Uri location, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPostAsync(location, contentType, content);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPostAsync(this Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPostAsync(location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPostAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPostAsync(location, contentType, content);
            }
        }

        /// <summary>
        /// Send a POST request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPostAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPostAsync(location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPutAsync(this Uri location, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPutAsync(location, contentType, content);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPutAsync(this Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPutAsync(location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPutAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPutAsync(location, contentType, content);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPutAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPutAsync(location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPatchAsync(this Uri location, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPatchAsync(location, contentType, content);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPatchAsync(this Uri location, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPatchAsync(location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPatchAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPatchAsync(location, contentType, content);
            }
        }

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpPatchAsync(this Uri location, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpPatchAsync(location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpTraceAsync(this Uri location)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpTraceAsync(location);
            }
        }

        /// <summary>
        /// Send a TRACE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpTraceAsync(this Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpTraceAsync(location, timeout);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, string method)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, string method, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, timeout);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, string method, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, contentType, content);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, string method, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method, string contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, contentType, content);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method, string contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, string method, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, contentType, content);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, string method, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="method">The HTTP method.</param>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method, MediaTypeHeaderValue contentType, Stream content)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, contentType, content);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="contentType">The Content-Type header of the HTTP request sent to the server.</param>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method, MediaTypeHeaderValue contentType, Stream content, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, contentType, content, timeout);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this Uri location, HttpMethod method)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location);
            }
        }

        /// <summary>
        /// Send an HTTP request as an asynchronous operation.
        /// </summary>
        /// <param name="location">The Uri the request is sent to.</param>
        /// <param name="method">The HTTP method.</param>
        /// <param name="timeout">The timespan to wait before the request times out.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public static Task<HttpResponseMessage> HttpAsync(this HttpMethod method, Uri location, TimeSpan timeout)
        {
            using (HttpManager manager = new HttpManager())
            {
                return manager.HttpAsync(method, location, timeout);
            }
        }
    }
}